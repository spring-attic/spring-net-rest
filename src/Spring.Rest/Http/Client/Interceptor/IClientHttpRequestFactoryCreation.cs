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

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// Represents the context of a client-side HTTP request creation, 
    /// given to an interceptor.
    /// </summary>
    /// <seealso cref="IClientHttpRequestFactoryInterceptor"/>
    /// <author>Bruno Baia</author>
    public interface IClientHttpRequestFactoryCreation
    {
        /// <summary>
        /// Gets or sets the HTTP method of the request.
        /// </summary>
        HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the URI of the request.
        /// </summary>
        Uri Uri { get; set; }

        /// <summary>
        /// Create the request with the current context.
        /// </summary>
        /// <remarks>
        /// Used to invoke the next interceptor in the interceptor chain, 
        /// or - if the calling interceptor is last - create the request itself.
        /// </remarks>
        /// <returns>The created request.</returns>
        IClientHttpRequest Create();
    }
}
