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
    /// Intercepts client-side HTTP requests creation. 
    /// </summary>
    /// <seealso cref="IClientHttpRequestFactoryCreation"/>
    /// <author>Bruno Baia</author>
    public interface IClientHttpRequestFactoryInterceptor : IClientHttpRequestInterceptor
    {
        /// <summary>
        /// Intercept the given request creation, and return the created request. 
        /// The given <see cref="IClientHttpRequestFactoryCreation"/> allows the interceptor 
        /// to pass on the request to the next entity in the chain.
        /// </summary>
        /// <remarks>
        /// A typical implementation of this method would follow the following pattern: 
        /// <ul>
        /// <li>Examine the HTTP uri and method.</li>
        /// <li>Optionally modify the request uri.</li>
        /// <li>Optionally modify the request method.</li>
        /// <li><strong>Either</strong>
        /// <ul>
        /// <li>create the request using the <see cref="M:IClientHttpRequestFactoryCreation.Create()"/> method,</li>
        /// <strong>or</strong>
        /// <li>create your own request.</li>
        /// </ul>
        /// </li>
        /// <li>Optionally modify the created request.</li>
        /// </ul>
        /// </remarks>
        /// <param name="creation">The request creation context.</param>
        /// <returns>The created request.</returns>
        IClientHttpRequest Create(IClientHttpRequestFactoryCreation creation);
    }
}
