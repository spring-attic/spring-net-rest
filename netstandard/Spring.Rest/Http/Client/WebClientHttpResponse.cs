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
using System.IO;
using System.Net;

using Spring.Util;

namespace Spring.Http.Client
{
    /// <summary>
    /// <see cref="IClientHttpResponse"/> implementation that uses 
    /// .NET <see cref="HttpWebResponse"/>'s class to read responses.
    /// </summary>
    /// <author>Bruno Baia</author>
    public class WebClientHttpResponse : IClientHttpResponse
    {
        private HttpHeaders headers;
        private HttpWebResponse httpWebResponse;

        /// <summary>
        /// Gets the <see cref="HttpWebResponse"/> instance used by the response.
        /// </summary>
        public HttpWebResponse HttpWebResponse
        {
            get { return this.httpWebResponse; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="WebClientHttpResponse"/> 
        /// with the given <see cref="HttpWebResponse"/> instance.
        /// </summary>
        /// <param name="response">The <see cref="HttpWebResponse"/> instance to use.</param>
        public WebClientHttpResponse(HttpWebResponse response)
        {
            ArgumentUtils.AssertNotNull(response, "response");

            this.httpWebResponse = response;
            this.headers = new HttpHeaders();

            this.Initialize();
        }

        #region IClientHttpResponse Members

        /// <summary>
        /// Gets the message headers.
        /// </summary>
        public HttpHeaders Headers
        {
            get { return this.headers; }
        }

        /// <summary>
        /// Gets the body of the message as a stream.
        /// </summary>
        public Stream Body
        {
            get
            {
                return this.httpWebResponse.GetResponseStream();
            }
        }

        /// <summary>
        /// Gets the HTTP status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.httpWebResponse.StatusCode;
            }
        }

        /// <summary>
        /// Gets the HTTP status description of the response.
        /// </summary>
        public string StatusDescription
        {
            get
            {
                return this.httpWebResponse.StatusDescription;
            }
        }

        /// <summary>
        /// Closes this response, freeing any resources created.
        /// </summary>
        public void Close()
        {
            this.httpWebResponse.Close();
        }

        void IDisposable.Dispose()
        {
            ((IDisposable)this.httpWebResponse).Dispose();
        }

        #endregion

        /// <summary>
        /// Initialize the response.
        /// </summary>
        /// <remarks>
        /// Default implementation copies headers from the .NET response. Can be overridden in subclasses.
        /// </remarks>
        protected virtual void Initialize()
        {
            foreach (string header in this.httpWebResponse.Headers)
            {
                this.headers[header] = this.httpWebResponse.Headers[header];
            }
        }
    }
}
