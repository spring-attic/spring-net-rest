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

namespace Spring.Http
{
    /// <summary>
    /// Represents an HTTP response message, as defined in the HTTP specification. 
    /// <a href="http://tools.ietf.org/html/rfc2616#section-6">HTTP 1.1, section 6</a>
    /// </summary>
    /// <typeparam name="T">The type of the response body.</typeparam>
    /// <author>Bruno Baia</author>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class HttpResponseMessage<T> : HttpResponseMessage where T : class
    {
        private T body;

        /// <summary>
        /// Gets the response body. May be null.
        /// </summary>
        public T Body
        {
            get { return this.body; }
        }

         /// <summary>
        /// Creates a new instance of <see cref="HttpResponseMessage{T}"/> with the given body, status code and status description.
        /// </summary>
        /// <param name="body">The response body.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="statusDescription">The HTTP status description.</param>
        public HttpResponseMessage(T body, HttpStatusCode statusCode, string statusDescription) :
            this(body, new HttpHeaders(), statusCode, statusDescription)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpResponseMessage{T}"/> with the given body, headers, status code and status description.
        /// </summary>
        /// <param name="body">The response body.</param>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="statusDescription">The HTTP status description.</param>
        public HttpResponseMessage(T body, HttpHeaders headers, HttpStatusCode statusCode, string statusDescription) :
            base(headers, statusCode, statusDescription)
        {
            this.body = body;
        }
    }
}
