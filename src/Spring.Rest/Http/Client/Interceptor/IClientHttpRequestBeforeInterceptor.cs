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
    /// Intercepts client-side HTTP requests before their execution. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interceptor cannot prevent the request execution, 
    /// unless an exception is thrown.
    /// </para>
    /// <para>
    /// The main advantage is that it can be used 
    /// for both synchronous and asynchronous request execution.
    /// </para>
    /// </remarks>
    /// <seealso cref="IClientHttpRequestContext"/>
    /// <author>Bruno Baia</author>
    public interface IClientHttpRequestBeforeInterceptor : IClientHttpRequestInterceptor
    {
        /// <summary>
        /// The callback method before the given request is executed.
        /// </summary>
        /// <remarks>
        /// A typical implementation of this method would follow the following pattern: 
        /// <ul>
        /// <li>Examine the HTTP request.</li>
        /// <li>Optionally modify the request headers.</li>
        /// <li>Optionally modify the request body.</li>
        /// </ul>
        /// </remarks>
        /// <param name="request">The request context.</param>
        void BeforeExecute(IClientHttpRequestContext request);
    }
}
