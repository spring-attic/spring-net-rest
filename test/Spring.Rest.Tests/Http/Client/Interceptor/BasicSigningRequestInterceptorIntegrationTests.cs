#if NET_3_5
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
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Security;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;

using NUnit.Framework;

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// Integration tests for the BasicSigningRequestInterceptor class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class BasicSigningRequestInterceptorIntegrationTests
    {
        private const string BASE_URL = "http://localhost:1337";
        private WebServiceHost webServiceHost;

        [SetUp]
        public void SetUp()
        {
            webServiceHost = new WebServiceHost(typeof(TestService), new Uri(BASE_URL));

            WebHttpBinding httpBinding1 = new WebHttpBinding();
            httpBinding1.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly;
            httpBinding1.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            webServiceHost.AddServiceEndpoint(typeof(TestService), httpBinding1, "/basic");

            webServiceHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
            webServiceHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserNamePasswordValidator();

            webServiceHost.Open();
        }

        [TearDown]
        public void TearDown()
        {
            webServiceHost.Close();
        }

        private IClientHttpRequest CreateRequest(BasicSigningRequestInterceptor requestInterceptor, string path, HttpMethod method)
        {
            IClientHttpRequestFactory requestFactory = new InterceptingClientHttpRequestFactory(
                new WebClientHttpRequestFactory(),
                new IClientHttpRequestInterceptor[] { requestInterceptor });

            Uri uri = new Uri(BASE_URL + path);
            IClientHttpRequest request = requestFactory.CreateRequest(uri, method);
            Assert.AreEqual(method, request.Method, "Invalid HTTP method");
            Assert.AreEqual(uri, request.Uri, "Invalid HTTP URI");

            return request;
        }

        [Test]
        public void BasicAuthorizationKO()
        {
            IClientHttpRequest request = this.CreateRequest(
                new BasicSigningRequestInterceptor("bruno", "baia"), "/basic/echo", HttpMethod.PUT);
            request.Headers.ContentLength = 0;
            using (IClientHttpResponse response = request.Execute())
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode, "Invalid status code");
            }
        }

        [Test]
        public void BasicAuthorizationOK()
        {
            IClientHttpRequest request = this.CreateRequest(
                new BasicSigningRequestInterceptor("login", "password"), "/basic/echo", HttpMethod.PUT);
            request.Headers.ContentLength = 0;
            using (IClientHttpResponse response = request.Execute())
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Invalid status code");
            }
        }


        #region Test classes

        public class CustomUserNamePasswordValidator : UserNamePasswordValidator
        {
            public override void Validate(string userName, string password)
            {
                if (userName == "login" && password == "password")
                {
                    return;
                }
                throw new SecurityTokenException("Unknown username or incorrect password");
            }
        }

        #endregion

        #region Test service

        [ServiceContract]
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class TestService
        {
            [OperationContract]
            [WebInvoke(UriTemplate = "echo", Method = "PUT")]
            public Stream Echo(Stream message)
            {
                WebOperationContext context = WebOperationContext.Current;
                foreach (string headerName in context.IncomingRequest.Headers)
                {
                    context.OutgoingResponse.Headers[headerName] = context.IncomingRequest.Headers[headerName];
                }
                context.OutgoingResponse.StatusCode = HttpStatusCode.OK;

                return message;
            }
        }

        #endregion
    }
}
#endif