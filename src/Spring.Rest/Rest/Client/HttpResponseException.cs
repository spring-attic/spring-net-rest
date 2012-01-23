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
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

using Spring.Http;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Base class for exceptions based on a response message.
    /// </summary>
    /// <author>Bruno Baia</author>
#if !SILVERLIGHT && !CF_3_5
    [Serializable]
#endif
    public class HttpResponseException : RestClientException
    {
        /// <summary>
        /// Default encoding for responses as string.
        /// </summary>
#if SILVERLIGHT || CF_3_5
        protected static readonly Encoding DEFAULT_CHARSET = new UTF8Encoding(false); // Remove byte Order Mask (BOM)
#else
        protected static readonly Encoding DEFAULT_CHARSET = Encoding.GetEncoding("ISO-8859-1");
#endif

        private Uri requestUri; 
        private HttpMethod requestMethod;
        private HttpResponseMessage<byte[]> response;

        /// <summary>
        /// Gets the HTTP request URI.
        /// </summary>
        public Uri RequestUri
        {
            get { return this.requestUri; }
        }

        /// <summary>
        /// Gets the HTTP request method.
        /// </summary>
        public HttpMethod RequestMethod
        {
            get { return this.requestMethod; }
        }

        /// <summary>
        /// Gets the HTTP response message.
        /// </summary>
        public HttpResponseMessage<byte[]> Response
        {
            get { return this.response; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpResponseException"/> 
        /// based on an HTTP response message.
        /// </summary>
        /// <param name="requestUri">The HTTP request URI.</param>
        /// <param name="requestMethod">The HTTP request method.</param>
        /// <param name="response">The HTTP response message.</param>
        public HttpResponseException(Uri requestUri, HttpMethod requestMethod, HttpResponseMessage<byte[]> response)
            : base(String.Format("{0} request for '{1}' resulted in {2:d} - {2} ({3}).", requestMethod, requestUri, response.StatusCode, response.StatusDescription))
        {
            this.requestUri = requestUri;
            this.requestMethod = requestMethod;
            this.response = response;
        }

#if !SILVERLIGHT && !CF_3_5
        /// <summary>
        /// Creates a new instance of the <see cref="RestClientException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/>
        /// that contains contextual information about the source or destination.
        /// </param>
        protected HttpResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.requestUri = (Uri)info.GetValue("RequestUri", typeof(Uri));
                this.requestMethod = (HttpMethod)info.GetValue("RequestMethod", typeof(HttpMethod));
                this.response = (HttpResponseMessage<byte[]>)info.GetValue("Response", typeof(HttpResponseMessage<byte[]>));
            }
        }

        /// <summary>
        /// Populates the <see cref="System.Runtime.Serialization.SerializationInfo"/> with 
        /// information about the exception.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds 
        /// the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual 
        /// information about the source or destination.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (info != null)
            {
                info.AddValue("RequestUri", this.requestUri);
                info.AddValue("RequestMethod", this.requestMethod);
                info.AddValue("Response", this.response);
            }
        }
#endif

        /// <summary>
        /// Returns the response body as a string.
        /// </summary>
        /// <returns>The response body.</returns>
        public string GetResponseBodyAsString()
        {
            if (this.response.Body == null)
            {
                return null;
            }
            MediaType contentType = this.response.Headers.ContentType;
            Encoding charset = (contentType != null && contentType.CharSet != null) ? contentType.CharSet : DEFAULT_CHARSET;
            return charset.GetString(this.response.Body, 0, this.response.Body.Length);
        }
    }
}
