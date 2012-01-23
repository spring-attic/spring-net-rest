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

using Spring.Http;
using Spring.Http.Client;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Strategy interface used by the <see cref="RestTemplate"/> to determine 
    /// whether a particular response has an error or not.
    /// </summary>
    /// <author>Bruno Baia</author>
    public interface IResponseErrorHandler
    {
        /// <summary>
        /// Handles the error in the given response. 
        /// <para/>
        /// Implementations will typically throw an exception if the response has any errors.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestMethod">The request method.</param>
        /// <param name="response">The response with the error.</param>
        void HandleError(Uri requestUri, HttpMethod requestMethod, IClientHttpResponse response);
    }
}
