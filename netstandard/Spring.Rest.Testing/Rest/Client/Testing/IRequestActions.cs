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

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Allows for setting up responses and additional expectations on requests.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are returned by <see cref="M:MockRestServiceServer.ExpectNewRequest()"/>.
    /// </remarks>
    /// <author>Lukas Krecan</author>
    /// <author>Arjen Poutsma</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public interface IRequestActions
    {
        /// <summary>
        /// Allows for further expectations to be set on the request.
        /// </summary>
        /// <param name="requestMatcher">The request expectations.</param>
        /// <returns>The request expectations.</returns>
        IRequestActions AndExpect(RequestMatcher requestMatcher);

        /// <summary>
        /// Allows for reponse creation on the request.
        /// </summary>
        /// <param name="responseCreator">The response creator.</param>
        void AndRespond(ResponseCreator responseCreator);
    }
}
