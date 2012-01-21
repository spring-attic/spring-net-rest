#if !SILVERLIGHT
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
    /// Intercepts synchronous client-side HTTP requests execution. 
    /// </summary>
    /// <seealso cref="IClientHttpRequestSyncExecution"/>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia</author>
    public interface IClientHttpRequestSyncInterceptor : IClientHttpRequestInterceptor
    {
        /// <summary>
        /// Intercept the given synchronous request execution. 
        /// The given <see cref="IClientHttpRequestSyncExecution"/> allows the interceptor 
        /// to pass on the request and the response to the next entity in the chain.
        /// </summary>
        /// <remarks>
        /// A typical implementation of this method would follow the following pattern: 
        /// <ul>
        /// <li>Examine the HTTP request.</li>
        /// <li>Optionally modify the request headers.</li>
        /// <li>Optionally modify the request body.</li>
        /// <li><strong>Either</strong>
        /// <ul>
        /// <li>execute the request synchronous using the <see cref="M:IClientHttpRequestExecution.Execute()"/> method,</li>
        /// <strong>or</strong>
        /// <li>do not execute the request to block the execution altogether.</li>
        /// </ul>
        /// </li>
        /// <li>Optionally wrap the response.</li>
        /// </ul>
        /// </remarks>
        /// <param name="execution">The request execution context.</param>
        /// <returns>The response result of the execution</returns>
        IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution);
    }
}
#endif
