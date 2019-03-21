#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.Collections.Generic;

using Spring.Util;
using Spring.Http;
using Spring.Http.Client;

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Mock implementation of <see cref="IClientHttpRequestFactory"/>. 
    /// Contains a list of expected <see cref="MockClientHttpRequest"/>s, and iterates over those.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Lukas Krecan</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public class MockClientHttpRequestFactory : IClientHttpRequestFactory
    {
        private IList<MockClientHttpRequest> expectedRequests = new List<MockClientHttpRequest>();
        private IEnumerator<MockClientHttpRequest> requestEnumerator;

        /// <summary>
        /// Create a new <see cref="IClientHttpRequest"/> for the specified URI and HTTP method.
        /// </summary>
        /// <param name="uri">The URI to create a request for.</param>
        /// <param name="method">The HTTP method to execute.</param>
        /// <returns>The created request.</returns>
        public IClientHttpRequest CreateRequest(Uri uri, HttpMethod method)
        {
            ArgumentUtils.AssertNotNull(uri, "uri");
            ArgumentUtils.AssertNotNull(method, "method");

            if (this.requestEnumerator == null)
            {
                requestEnumerator = expectedRequests.GetEnumerator();
            }
            if (!this.requestEnumerator.MoveNext())
            {
                throw new AssertionException("No further requests expected");
            }

            MockClientHttpRequest currentRequest = this.requestEnumerator.Current;
            currentRequest.Uri = uri;
            currentRequest.Method = method;
            return currentRequest;
        }

        internal MockClientHttpRequest ExpectNewRequest()
        {
            if (this.requestEnumerator != null)
            {
                throw new InvalidCastException("Can not expect another request, the test is already underway");
            }

            MockClientHttpRequest request = new MockClientHttpRequest();
            this.expectedRequests.Add(request);
            return request;
        }

        internal void VerifyRequests()
        {
            if (this.expectedRequests.Count == 0)
            {
                return;
            }
            if (this.requestEnumerator == null || this.requestEnumerator.MoveNext())
            {
                throw new AssertionException("Further request(s) expected");
            }
        }
    }
}
