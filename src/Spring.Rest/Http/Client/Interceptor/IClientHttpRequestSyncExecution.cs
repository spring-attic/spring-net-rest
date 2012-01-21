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
using System.IO;

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// Represents the context of a synchronous client-side HTTP request execution, 
    /// given to an interceptor.
    /// </summary>
    /// <seealso cref="IClientHttpRequestSyncInterceptor"/>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia</author>
    public interface IClientHttpRequestSyncExecution : IClientHttpRequestContext
    {
        /// <summary>
        /// Execute the request synchronously with the current context, and return the response.
        /// </summary>
        /// <remarks>
        /// Used to invoke the next interceptor in the interceptor chain, 
        /// or - if the calling interceptor is last - execute the request itself.
        /// </remarks>
        /// <returns>The response result of the execution</returns>
        IClientHttpResponse Execute();
    }
}
#endif