﻿#region License

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

using Spring.Util;

namespace Spring.Http
{
    /// <summary>
    /// Represents an HTTP entity message, as defined in the HTTP specification. 
    /// <a href="http://tools.ietf.org/html/rfc2616#section-7">HTTP 1.1, section 7</a>
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia</author>
    public class HttpEntity
    {
        private HttpHeaders headers;
        private object body;

        /// <summary>
        /// Gets the entity headers.
        /// </summary>
        public HttpHeaders Headers
        {
            get { return this.headers; }
        }

        /// <summary>
        /// Gets the entity body. May be null.
        /// </summary>
        public object Body
        {
            get { return this.body; }
        }

        /// <summary>
        /// Indicates whether this entity has a body.
        /// </summary>
        /// <returns></returns>
        public bool HasBody
        {
            get { return (this.body != null); }
        }

        /// <summary>
        /// Creates a new, empty instance of <see cref="HttpEntity"/> with no body or headers.
        /// </summary>
        public HttpEntity()
            : this(null, new HttpHeaders())
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpEntity"/> with the given body.
        /// </summary>
        /// <param name="body">The entity body.</param>
        public HttpEntity(object body)
            : this(body, new HttpHeaders())
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpEntity"/> with the given headers.
        /// </summary>
        /// <param name="headers">The entity headers.</param>
        public HttpEntity(HttpHeaders headers)
            : this(null, headers)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpEntity"/> with the given body and headers.
        /// </summary>
        /// <param name="body">The entity body.</param>
        /// <param name="headers">The entity headers.</param>
        public HttpEntity(object body, HttpHeaders headers)
        {
            ArgumentUtils.AssertNotNull(headers, "headers");

            this.body = body;
            this.headers = headers;
        }
    }
}