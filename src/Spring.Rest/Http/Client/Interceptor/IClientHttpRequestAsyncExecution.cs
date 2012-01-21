#if !CF_3_5
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
    /// Represents the context of an asynchronous client-side HTTP request execution, 
    /// given to an interceptor.
    /// </summary>
    /// <seealso cref="IClientHttpRequestAsyncInterceptor"/>
    /// <author>Bruno Baia</author>
    public interface IClientHttpRequestAsyncExecution : IClientHttpRequestContext
    {
        /// <summary>
        /// Gets or sets the optional user-defined object that is passed to the method invoked 
        /// when the asynchronous operation completes.
        /// </summary>
        object AsyncState { get; set; }

        /// <summary>
        /// Execute the request asynchronously with the current context.
        /// </summary>
        /// <remarks>
        /// Used to invoke the next interceptor in the interceptor chain, 
        /// or - if the calling interceptor is last - execute the request itself.
        /// </remarks>
        /// <seealso cref="M:ExecuteAsync(Action{IClientHttpResponseAsyncContext} executeCompleted)"/>
        void ExecuteAsync();

        /// <summary>
        /// Execute the request asynchronously with the current context and handle the response result.
        /// </summary>
        /// <remarks>
        /// Used to invoke the next interceptor in the interceptor chain, 
        /// or - if the calling interceptor is last - execute the request itself.
        /// </remarks>
        /// <param name="executeCompleted">
        /// The <see cref="Action{IClientHttpResponseAsyncContext}"/> to perform when the asynchronous execution completes.
        /// </param>
        /// <seealso cref="M:ExecuteAsync()"/>
        void ExecuteAsync(Action<IClientHttpResponseAsyncContext> executeCompleted);
    }
}
#endif
