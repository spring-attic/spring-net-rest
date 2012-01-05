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

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Factory methods for <see cref="RequestMatcher"/> classes. 
    /// Typically used to provide input for <see cref="M:IRequestActions.AndExpect(RequestMatcher)"/>.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public static class RequestMatchers
    {
        /// <summary>
        /// Creates a <see cref="RequestMatcher"/> that expect the given request <see cref="HttpMethod"/>.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <returns>The request matcher.</returns>
        public static RequestMatcher MatchMethod(HttpMethod method)
        {
            return delegate(IClientHttpRequest request)
            {
                AssertionUtils.AreEqual(method, request.Method, "Unexpected HttpMethod");
            };
        }

        /// <summary>
        /// Creates a <see cref="RequestMatcher"/> that expect the given request URI.
        /// </summary>
        /// <param name="uri">the request URI.</param>
        /// <returns>The request matcher.</returns>
        public static RequestMatcher MatchUri(string uri)
        {
            return MatchUri(new Uri(uri));
        }

        /// <summary>
        /// Creates a <see cref="RequestMatcher"/> that expect the given request URI.
        /// </summary>
        /// <param name="uri">the request URI.</param>
        /// <returns>The request matcher.</returns>
        public static RequestMatcher MatchUri(Uri uri)
        {
            return delegate(IClientHttpRequest request)
            {
                AssertionUtils.AreEqual(uri, request.Uri, "Unexpected request URI");
            };
        }

        /// <summary>
        /// Creates a <see cref="RequestMatcher"/> that expect the given request header.
        /// </summary>
        /// <param name="header">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <returns>The request matcher.</returns>
        public static RequestMatcher MatchHeader(string header, string value)
        {
            return delegate(IClientHttpRequest request)
            {
                string[] actualHeaderValues = request.Headers.GetValues(header);
                AssertionUtils.IsTrue(actualHeaderValues != null, "Expected header in request: " + header);

                bool foundMatch = false;
                foreach (string actualHeaderValue in actualHeaderValues)
                {
                    if (actualHeaderValue.Equals(value))
                    {
                        foundMatch = true;
                        break;
                    }
                }
                AssertionUtils.IsTrue(foundMatch, "Unexpected header value");
            };
        }

        /// <summary>
        /// Creates a <see cref="RequestMatcher"/> that expect that the specified request header contains a subtring.
        /// </summary>
        /// <param name="header">The header name.</param>
        /// <param name="value">The substring that must appear in the header.</param>
        /// <returns>The request matcher.</returns>
        public static RequestMatcher MatchHeaderContains(string header, string value)
        {
            return delegate(IClientHttpRequest request)
            {
                string[] actualHeaderValues = request.Headers.GetValues(header);
                AssertionUtils.IsTrue(actualHeaderValues != null, "Expected header in request: " + header);

                bool foundMatch = false;
                foreach (string actualHeaderValue in actualHeaderValues)
                {
                    if (actualHeaderValue.Contains(value))
                    {
                        foundMatch = true;
                        break;
                    }
                }
                AssertionUtils.IsTrue(foundMatch, "Header \"" + header + "\" didn't contain expected text <" + value + ">");
            };
        }

        /// <summary>
        /// Creates a <see cref="RequestMatcher"/> that expect the given request body content.
        /// </summary>
        /// <param name="body">The request body.</param>
        /// <returns>The request matcher.</returns>
        public static RequestMatcher MatchBody(string body)
        {
            return delegate(IClientHttpRequest request)
            {
                MockClientHttpRequest mockRequest = request as MockClientHttpRequest;
                AssertionUtils.AreEqual(body, mockRequest.GetBodyContent(), "Unexpected body content");
            };
        }
    }
}