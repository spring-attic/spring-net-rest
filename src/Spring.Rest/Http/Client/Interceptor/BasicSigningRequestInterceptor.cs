#region License

/*
 * Copyright 2002-2011 the original author or authors.
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
    /// <see cref="IClientHttpRequestInterceptor"/> implementation that forces 
    /// HTTP Basic authentication for the request.
    /// </summary>
    /// <remarks>
    /// HTTP Basic authentication can also be configured using <see cref="WebClientHttpRequestFactory"/>, 
    /// but this implementation will not wait the challenge response from server 
    /// before to send the 'Authorization' header value.
    /// </remarks>
    /// <author>Bruno Baia</author>
    public class BasicSigningRequestInterceptor : IClientHttpRequestInterceptor
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

        #region IClientHttpRequestInterceptor Membres

        /// <summary>
        /// Intercept the given request creation, and return the created request. 
        /// The given <see cref="IClientHttpRequestCreation"/> allows the interceptor 
        /// to pass on the request to the next entity in the chain.
        /// </summary>
        /// <remarks>
        /// This implementation adds the 'Authorization' header to the created request.
        /// </remarks>
        /// <param name="creation">The request creation context.</param>
        /// <returns>The created and authenticated request.</returns>
        public IClientHttpRequest Create(IClientHttpRequestCreation creation)
        {
            IClientHttpRequest request = creation.Create();
            request.Headers["Authorization"] = this.authorizationHeaderValue;
            return request;
        }

        /// <summary>
        /// Intercept the given request execution. 
        /// The given <see cref="IClientHttpRequestExecution"/> allows the interceptor 
        /// to pass on the request and the response to the next entity in the chain.
        /// </summary>
        /// <remarks>
        /// This implementation proceeds to the next interceptor in the chain 
        /// by calling the <see cref="M:IClientHttpRequestExecution.Execute()"/> method.
        /// </remarks>
        /// <param name="execution">The request execution context.</param>
        public void Execute(IClientHttpRequestExecution execution)
        {
            execution.Execute();
        }

        #endregion
    }
}
