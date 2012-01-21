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

using Spring.Util;

namespace Spring.Http
{
    /// <summary>
    /// Represents an HTTP response message, with no body, as defined in the HTTP specification. 
    /// <a href="http://tools.ietf.org/html/rfc2616#section-6">HTTP 1.1, section 6</a>
    /// </summary>
    /// <author>Bruno Baia</author>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class HttpResponseMessage
    {
        private HttpHeaders headers;
        private HttpStatusCode statusCode;
        private string statusDescription;

        /// <summary>
        /// Gets the response headers.
        /// </summary>
        public HttpHeaders Headers
        {
            get { return this.headers; }
        }

        /// <summary>
        /// Gets the HTTP status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return statusCode; }
        }

        /// <summary>
        /// Gets the HTTP status description of the response.
        /// </summary>
        public string StatusDescription
        {
            get { return statusDescription; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpResponseMessage"/> with the given status code and status description.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="statusDescription">The HTTP status description.</param>
        public HttpResponseMessage(HttpStatusCode statusCode, string statusDescription) :
            this(new HttpHeaders(), statusCode, statusDescription)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpResponseMessage"/> with the given headers, status code and status description.
        /// </summary>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="statusDescription">The HTTP status description.</param>
        public HttpResponseMessage(HttpHeaders headers, HttpStatusCode statusCode, string statusDescription)
        {
            ArgumentUtils.AssertNotNull(headers, "headers");

            this.headers = headers;
            this.statusCode = statusCode;
            this.statusDescription = statusDescription;
        }
    }
}
