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
using System.Net;

using Spring.IO;
using Spring.Http;

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Extensions methods for <see cref="IRequestActions"/> interface. 
    /// </summary>
    /// <author>Bruno Baia</author>
    public static class RequestActionsExtensions
    {
        #region AndExpect Extensions

        /// <summary>
        /// Expects the given request <see cref="HttpMethod"/>.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up expectation on.</param>
        /// <param name="method">The HTTP method.</param>
        /// <returns>
        /// The <see cref="IRequestActions"/> to set up responses and additional expectations on the request.
        /// </returns>
        public static IRequestActions AndExpectMethod(this IRequestActions requestActions, HttpMethod method)
        {
            return requestActions.AndExpect(RequestMatchers.MatchMethod(method));
        }

        /// <summary>
        /// Expects the given request URI.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up expectation on.</param>
        /// <param name="uri">the request URI.</param>
        /// <returns>
        /// The <see cref="IRequestActions"/> to set up responses and additional expectations on the request.
        /// </returns>
        public static IRequestActions AndExpectUri(this IRequestActions requestActions, string uri)
        {
            return requestActions.AndExpect(RequestMatchers.MatchUri(uri));
        }

        /// <summary>
        /// Expects the given request URI.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up expectation on.</param>
        /// <param name="uri">the request URI.</param>
        /// <returns>
        /// The <see cref="IRequestActions"/> to set up responses and additional expectations on the request.
        /// </returns>
        public static IRequestActions AndExpectUri(this IRequestActions requestActions, Uri uri)
        {
            return requestActions.AndExpect(RequestMatchers.MatchUri(uri));
        }

        /// <summary>
        /// Expects the given request header.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up expectation on.</param>
        /// <param name="header">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <returns>
        /// The <see cref="IRequestActions"/> to set up responses and additional expectations on the request.
        /// </returns>
        public static IRequestActions AndExpectHeader(this IRequestActions requestActions, string header, string value)
        {
            return requestActions.AndExpect(RequestMatchers.MatchHeader(header, value));
        }

        /// <summary>
        /// Expects that the specified request header contains the given substring.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up expectation on.</param>
        /// <param name="header">The header name.</param>
        /// <param name="value">The substring that must appear in the header.</param>
        /// <returns>
        /// The <see cref="IRequestActions"/> to set up responses and additional expectations on the request.
        /// </returns>
        public static IRequestActions AndExpectHeaderContains(this IRequestActions requestActions, string header, string value)
        {
            return requestActions.AndExpect(RequestMatchers.MatchHeaderContains(header, value));
        }

        /// <summary>
        /// Expects the given request body content.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up expectation on.</param>
        /// <param name="body">The request body.</param>
        /// <returns>
        /// The <see cref="IRequestActions"/> to set up responses and additional expectations on the request.
        /// </returns>
        public static IRequestActions AndExpectBody(this IRequestActions requestActions, string body)
        {
            return requestActions.AndExpect(RequestMatchers.MatchBody(body));
        }

        /// <summary>
        /// Expects that the request body contains the given substring.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up expectation on.</param>
        /// <param name="body">The request body.</param>
        /// <returns>
        /// The <see cref="IRequestActions"/> to set up responses and additional expectations on the request.
        /// </returns>
        public static IRequestActions AndExpectBodyContains(this IRequestActions requestActions, string body)
        {
            return requestActions.AndExpect(RequestMatchers.MatchBodyContains(body));
        }

        #endregion

        #region AndRespond Extensions

        /// <summary>
        /// Responds with the given response body, headers, status code, and status description.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up response on.</param>
        /// <param name="body">The body of the response.</param>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="statusDescription">The response status description.</param>
        public static void AndRespondWith(this IRequestActions requestActions,
            string body, HttpHeaders headers, HttpStatusCode statusCode, string statusDescription)
        {
            requestActions.AndRespond(ResponseCreators.CreateWith(body, headers, statusCode, statusDescription));
        }

        /// <summary>
        /// Responds with the given response body and headers. The response status code is HTTP 200 (OK).
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up response on.</param>
        /// <param name="body">The body of the response.</param>
        /// <param name="headers">The response headers.</param>
        public static void AndRespondWith(this IRequestActions requestActions,
            string body, HttpHeaders headers)
        {
            requestActions.AndRespond(ResponseCreators.CreateWith(body, headers, HttpStatusCode.OK, "OK"));
        }

        /// <summary>
        /// Responds with the given response body (from a <see cref="IResource"/>), headers, status code, and status description.
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up response on.</param>
        /// <param name="body">The <see cref="IResource"/> containing the body of the response.</param>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="statusDescription">The response status description.</param>
        public static void AndRespondWith(this IRequestActions requestActions, 
            IResource body, HttpHeaders headers, HttpStatusCode statusCode, string statusDescription) 
        {
            requestActions.AndRespond(ResponseCreators.CreateWith(body, headers, statusCode, statusDescription));
        }
	
        /// <summary>
        /// Responds with the given response body (from a <see cref="IResource"/>) and headers. The response status code is HTTP 200 (OK).
        /// </summary>
        /// <param name="requestActions">The <see cref="IRequestActions"/> to set up response on.</param>
        /// <param name="body">The <see cref="IResource"/> containing the body of the response.</param>
        /// <param name="headers">The response headers.</param>
        public static void AndRespondWith(this IRequestActions requestActions, 
            IResource body, HttpHeaders headers) 
        {
            requestActions.AndRespond(ResponseCreators.CreateWith(body, headers, HttpStatusCode.OK, "OK"));
        }

        #endregion
    }
}

#if NET_2_0 && !NET_3_5
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Manufactured Extension Attribute to permit .NET 2.0 to support extension methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtensionAttribute : Attribute
    {
    }
}
#endif
