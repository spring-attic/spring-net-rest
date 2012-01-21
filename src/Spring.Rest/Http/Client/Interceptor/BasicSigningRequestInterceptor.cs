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
using System.Text;

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// <see cref="IClientHttpRequestBeforeInterceptor"/> implementation that forces 
    /// HTTP Basic authentication for the request.
    /// </summary>
    /// <remarks>
    /// HTTP Basic authentication can also be configured using <see cref="WebClientHttpRequestFactory"/>, 
    /// but this implementation will not wait the challenge response from server 
    /// before to send the 'Authorization' header value.
    /// </remarks>
    /// <author>Bruno Baia</author>
    public class BasicSigningRequestInterceptor : IClientHttpRequestBeforeInterceptor
    {
        private string authorizationHeaderValue;

        /// <summary>
        /// Creates a new instance of <see cref="BasicSigningRequestInterceptor"/> 
        /// with the given user name and password.
        /// </summary>
        /// <param name="userName">The user name for HTTP authentication.</param>
        /// <param name="password">The password for HTTP authentication.</param>
        public BasicSigningRequestInterceptor(string userName, string password)
        {
            string authInfo = String.Format("{0}:{1}", userName, password);
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            this.authorizationHeaderValue = "Basic " + authInfo;
        }

        #region IClientHttpRequestBeforeInterceptor Members

        /// <summary>
        /// The callback method before the given request is executed.
        /// </summary>
        /// <remarks>
        /// This implementation adds the 'Authorization' header to the created request.
        /// </remarks>
        /// <param name="request">The request context.</param>
        public void BeforeExecute(IClientHttpRequestContext request)
        {
            request.Headers["Authorization"] = this.authorizationHeaderValue;
        }

        #endregion
    }
}
