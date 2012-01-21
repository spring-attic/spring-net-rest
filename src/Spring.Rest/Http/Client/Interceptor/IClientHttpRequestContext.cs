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

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// Represents the context of a client-side HTTP request, 
    /// given to an interceptor.
    /// </summary>
    /// <seealso cref="IClientHttpRequestBeforeInterceptor"/>
    /// <author>Bruno Baia</author>
    public interface IClientHttpRequestContext
    {
        /// <summary>
        /// Gets the HTTP method of the request.
        /// </summary>
        HttpMethod Method { get; }

        /// <summary>
        /// Gets the URI of the request.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Gets the request message headers.
        /// </summary>
        HttpHeaders Headers { get; }

        /// <summary>
        /// Gets or sets the delegate that writes the request body message as a stream.
        /// </summary>
        Action<Stream> Body { get;  set; }
    }
}
