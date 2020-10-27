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

using System.IO;
using System.Text;
using System.Net;

using Spring.IO;
using Spring.Util;
using Spring.Http;
using Spring.Http.Client;

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Factory methods for <see cref="ResponseCreator"/> classes. 
    /// Typically used to provide input for <see cref="M:IRequestActions.AndRespond(ResponseCreator)"/>.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public static class ResponseCreators
    {
        /// <summary>
        /// Creates a <see cref="ResponseCreator"/> that respond with 
        /// the given response body, headers, status code, and status description.
        /// </summary>
        /// <param name="body">The body of the response.</param>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="statusDescription">The response status description.</param>
        /// <returns>A <see cref="ResponseCreator"/>.</returns>
        public static ResponseCreator CreateWith(
            string body, HttpHeaders headers, HttpStatusCode statusCode, string statusDescription)
        {
            ArgumentUtils.AssertNotNull(body, "body");

            return delegate(IClientHttpRequest request)
            {
                return new MockClientHttpResponse(new MemoryStream(Encoding.UTF8.GetBytes(body)), headers, statusCode, statusDescription);
            };
        }

        /// <summary>
        /// Creates a <see cref="ResponseCreator"/> that respond with 
        /// the given response body and headers. 
        /// The response status code is HTTP 200 (OK).
        /// </summary>
        /// <param name="body">The body of the response.</param>
        /// <param name="headers">The response headers.</param>
        /// <returns>A <see cref="ResponseCreator"/>.</returns>
        public static ResponseCreator CreateWith(string body, HttpHeaders headers)
        {
            return CreateWith(body, headers, HttpStatusCode.OK, "OK");
        }

        /// <summary>
        /// Creates a <see cref="ResponseCreator"/> that respond with 
        /// the given response body (from a <see cref="IResource"/>), headers, status code, and status description.
        /// </summary>
        /// <param name="body">The <see cref="IResource"/> containing the body of the response.</param>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="statusDescription">The response status description.</param>
        /// <returns>A <see cref="ResponseCreator"/>.</returns>
        public static ResponseCreator CreateWith(
            IResource body, HttpHeaders headers, HttpStatusCode statusCode, string statusDescription)
        {
            ArgumentUtils.AssertNotNull(body, "body");

            return delegate(IClientHttpRequest request)
            {
                return new MockClientHttpResponse(body.GetStream(), headers, statusCode, statusDescription);
            };
        }

        /// <summary>
        /// Creates a <see cref="ResponseCreator"/> that respond with 
        /// the given response body (from a <see cref="IResource"/>) and headers. 
        /// The response status code is HTTP 200 (OK).
        /// </summary>
        /// <param name="body">The <see cref="IResource"/> containing the body of the response.</param>
        /// <param name="headers">The response headers.</param>
        /// <returns>A <see cref="ResponseCreator"/>.</returns>
        public static ResponseCreator CreateWith(IResource body, HttpHeaders headers)
        {
            return CreateWith(body, headers, HttpStatusCode.OK, "OK");
        }
    }
}