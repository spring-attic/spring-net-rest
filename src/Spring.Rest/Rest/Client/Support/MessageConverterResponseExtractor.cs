#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.Net;
using System.Collections.Generic;

using Spring.Http;
using Spring.Http.Client;
using Spring.Http.Converters;

namespace Spring.Rest.Client.Support
{
    /// <summary>
    /// Response extractor that uses the given HTTP message converters to convert the response into a type.
    /// </summary>
    /// <typeparam name="T">The response body type.</typeparam>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia (.NET)</author>
    public class MessageConverterResponseExtractor<T> : IResponseExtractor<T> where T : class
    {
        #region Logging
#if !SILVERLIGHT && !CF_3_5
        private static readonly Common.Logging.ILog LOG = Common.Logging.LogManager.GetLogger(typeof(MessageConverterResponseExtractor<T>));
#endif
        #endregion

        /// <summary>
        /// The underlying list of <see cref="IHttpMessageConverter"/> used to extract data from the response.
        /// </summary>
        protected IList<IHttpMessageConverter> messageConverters;

        /// <summary>
        /// Creates a new instance of the <see cref="MessageConverterResponseExtractor{T}"/> class.
        /// </summary>
        /// <param name="messageConverters">The list of <see cref="IHttpMessageConverter"/> to use.</param>
        public MessageConverterResponseExtractor(IList<IHttpMessageConverter> messageConverters)
        {
            this.messageConverters = messageConverters;
        }

        /// <summary>
        /// Gets called by <see cref="RestTemplate"/> with an opened <see cref="IClientHttpResponse"/> to extract data. 
        /// Does not need to care about closing the request or about handling errors: 
        /// this will all be handled by the <see cref="RestTemplate"/> class.
        /// </summary>
        /// <param name="response">The active HTTP request.</param>
        public virtual T ExtractData(IClientHttpResponse response)
        {
            if (!this.HasMessageBody(response)) 
            {
 	 	 	    return null;
 	 	 	}
            MediaType mediaType = response.Headers.ContentType;
            if (mediaType == null)
            {
                #region Instrumentation
#if !SILVERLIGHT && !CF_3_5
                if (LOG.IsWarnEnabled)
                {
                    LOG.Warn("No Content-Type header found, defaulting to 'application/octet-stream'");
                }
#endif
                #endregion

                mediaType = MediaType.APPLICATION_OCTET_STREAM;
            }
            foreach(IHttpMessageConverter messageConverter in messageConverters) 
            {
                if (messageConverter.CanRead(typeof(T), mediaType))
                {
                    #region Instrumentation
#if !SILVERLIGHT && !CF_3_5
                    if (LOG.IsDebugEnabled) 
                    {
                        LOG.Debug(String.Format(
                            "Reading [{0}] as '{1}' using [{2}]", 
                            typeof(T).FullName, mediaType, messageConverter));
                    }
#endif
                    #endregion

                    return messageConverter.Read<T>(response);
                }
            }
            throw new RestClientException(String.Format(
                "Could not extract response: no suitable HttpMessageConverter found for response type [{0}] and content type [{1}]", 
                typeof(T).FullName, mediaType));
        }

        /// <summary>
        /// Indicates whether or not the given response has a message body.
        /// </summary>
        /// <remarks>
        /// Default implementation returns false for a response status of 204 or 304, or a 'Content-Length' of 0.
        /// </remarks>
        /// <param name="response">The response to check for a message body.</param>
        /// <returns><see langword="true"/> if the response has a body; otherwise <see langword="false"/>.</returns>
        protected virtual bool HasMessageBody(IClientHttpResponse response) 
        {
            if (response.StatusCode == HttpStatusCode.NoContent || 
                response.StatusCode == HttpStatusCode.NotModified)
            {
                return false;
            }
            return response.Headers.ContentLength != 0;
        }
    }
}
