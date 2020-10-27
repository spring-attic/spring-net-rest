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

using Spring.Http.Client;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Callback interface for code that operates on a <see cref="IClientHttpRequest"/>. 
    /// Allows to manipulate the request headers, and write to the request body.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Callback interface used by <see cref="RestTemplate"/>'s senders methods. 
    /// Implementations of this interface perform the actual work of writing data 
    /// to a <see cref="IClientHttpRequest"/>, but don't need to worry about exception 
    /// handling or closing resources.
    /// </para>
    /// <para>
    /// Used internally by the <see cref="RestTemplate"/>, but also useful for application code.
    /// </para>
    /// </remarks>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia (.NET)</author>
    public interface IRequestCallback
    {
        /// <summary>
        /// Gets called by <see cref="RestTemplate"/> with an <see cref="IClientHttpRequest"/> to write data. 
        /// </summary>
        /// <remarks>
        /// Does not need to care about closing the request or about handling errors: 
        /// this will all be handled by the <see cref="RestTemplate"/> class.
        /// </remarks>
        /// <param name="request">The active HTTP request.</param>
        void DoWithRequest(IClientHttpRequest request);
    }
}
