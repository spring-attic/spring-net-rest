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
    /// Represents the asynchronous context of a client-side HTTP response, 
    /// given to an interceptor.
    /// </summary>
    /// <seealso cref="IClientHttpRequestAsyncExecution"/>
    /// <author>Bruno Baia</author>
    public interface IClientHttpResponseAsyncContext
    {
        /// <summary>
        /// Gets or sets the <see cref="IClientHttpResponse">response</see> result of the execution.
        /// </summary>
        IClientHttpResponse Response { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the asynchronous request execution has been cancelled.
        /// </summary>
        bool Cancelled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which error occurred during the asynchronous request execution.
        /// </summary>
        Exception Error { get; set; }

        /// <summary>
        /// Gets or sets the state object passed to the asynchronous request execution.
        /// </summary>
        object UserState { get; set; }
    }
}
#endif