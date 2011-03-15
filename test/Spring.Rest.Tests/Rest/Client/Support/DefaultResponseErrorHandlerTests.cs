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
using System.Text;

using Spring.Http;
using Spring.Http.Client;

using NUnit.Framework;
using Rhino.Mocks;

namespace Spring.Rest.Client.Support
{
    /// <summary>
    /// Unit tests for the DefaultResponseErrorHandler class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class DefaultResponseErrorHandlerTests
    {
        private MockRepository mocks;

        private IClientHttpResponse response;
        private DefaultResponseErrorHandler responseErrorHandler;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            response = mocks.StrictMock<IClientHttpResponse>();

            responseErrorHandler = new DefaultResponseErrorHandler();
        }

        [TearDown]
        public void TearDown()
        {
            mocks.VerifyAll();
        }

        [Test]
        public void ThrowsClientException()
        {
            ExpectResponse("ClientError", Encoding.UTF8, HttpStatusCode.NotFound, "NotFound");

            mocks.ReplayAll();

            Assert.IsTrue(responseErrorHandler.HasError(response));
            try
            {
                responseErrorHandler.HandleError(response);
                Assert.Fail("DefaultResponseErrorHandler.HandleError should throw an exception");
            }
            catch (Exception ex)
            {
                HttpClientErrorException clientErrorException = ex as HttpClientErrorException;
                Assert.IsNotNull(clientErrorException, "Exception HttpClientErrorException expected");
                Assert.IsNotNull(clientErrorException.Response);
                Assert.IsTrue(clientErrorException.Response.Body.Length > 0);
                Assert.IsTrue(clientErrorException.Response.Headers.ContentLength > 0);
                Assert.AreEqual(new MediaType("text", "plain", "utf-8"), clientErrorException.Response.Headers.ContentType);
                Assert.AreEqual("ClientError", clientErrorException.GetResponseBodyAsString());
                Assert.AreEqual(HttpStatusCode.NotFound, clientErrorException.Response.StatusCode);
                Assert.AreEqual("NotFound", clientErrorException.Response.StatusDescription);
            }
        }

        [Test]
        public void ThrowsClientExceptionWithNoBody()
        {
            ExpectResponse(null, Encoding.UTF8, HttpStatusCode.NotFound, "NotFound");

            mocks.ReplayAll();

            Assert.IsTrue(responseErrorHandler.HasError(response));
            try
            {
                responseErrorHandler.HandleError(response);
                Assert.Fail("DefaultResponseErrorHandler.HandleError should throw an exception");
            }
            catch (Exception ex)
            {
                HttpClientErrorException clientErrorException = ex as HttpClientErrorException;
                Assert.IsNotNull(clientErrorException, "Exception HttpClientErrorException expected");
                Assert.IsNotNull(clientErrorException.Response);
                Assert.AreEqual(-1, clientErrorException.Response.Headers.ContentLength);
                Assert.IsNull (clientErrorException.Response.Body);
                Assert.IsNull(clientErrorException.GetResponseBodyAsString());
            }
        }

        [Test]
        public void ThrowsClientExceptionWithEmptyBody()
        {
            ExpectResponse(String.Empty, Encoding.UTF8, HttpStatusCode.NotFound, "NotFound");

            mocks.ReplayAll();

            Assert.IsTrue(responseErrorHandler.HasError(response));
            try
            {
                responseErrorHandler.HandleError(response);
                Assert.Fail("DefaultResponseErrorHandler.HandleError should throw an exception");
            }
            catch (Exception ex)
            {
                HttpClientErrorException clientErrorException = ex as HttpClientErrorException;
                Assert.IsNotNull(clientErrorException, "Exception HttpClientErrorException expected");
                Assert.IsNotNull(clientErrorException.Response);
                Assert.AreEqual(0, clientErrorException.Response.Headers.ContentLength);
                Assert.AreEqual(0, clientErrorException.Response.Body.Length);
                Assert.AreEqual(String.Empty, clientErrorException.GetResponseBodyAsString());
            }
        }

        [Test]
        public void ThrowsServerException()
        {
            ExpectResponse("ServerError", Encoding.UTF8, HttpStatusCode.InternalServerError, "Internal Server Error");

            mocks.ReplayAll();

            Assert.IsTrue(responseErrorHandler.HasError(response));
            try
            {
                responseErrorHandler.HandleError(response);
                Assert.Fail("DefaultResponseErrorHandler.HandleError should throw an exception");
            }
            catch (Exception ex)
            {
                HttpServerErrorException serverErrorException = ex as HttpServerErrorException;
                Assert.IsNotNull(serverErrorException, "Exception HttpServerErrorException expected");
                Assert.IsNotNull(serverErrorException.Response);
                Assert.IsTrue(serverErrorException.Response.Body.Length > 0);
                Assert.IsTrue(serverErrorException.Response.Headers.ContentLength > 0);
                Assert.AreEqual(new MediaType("text", "plain", "utf-8"), serverErrorException.Response.Headers.ContentType);
                Assert.AreEqual("ServerError", serverErrorException.GetResponseBodyAsString());
                Assert.AreEqual(HttpStatusCode.InternalServerError, serverErrorException.Response.StatusCode);
                Assert.AreEqual("Internal Server Error", serverErrorException.Response.StatusDescription);
            }
        }

        [Test]
        public void DoesNotThrowException()
        {
            Expect.Call<HttpStatusCode>(response.StatusCode).Return(HttpStatusCode.OK);

            mocks.ReplayAll();

            Assert.IsFalse(responseErrorHandler.HasError(response));
        }

        #region Private methods

        private void ExpectResponse(string body, Encoding charSet, HttpStatusCode statusCode, string statusDescription)
        {
            MemoryStream mStream = new MemoryStream();
            HttpHeaders headers = new HttpHeaders();
            headers.ContentType = new MediaType("text", "plain", charSet);
            if (body != null)
            {
                byte[] bytes = charSet.GetBytes(body);
                mStream = new MemoryStream(bytes);
                headers.ContentLength = bytes.Length;
            }

            Expect.Call<Stream>(response.Body).Return(mStream).Repeat.Any();
            Expect.Call<HttpHeaders>(response.Headers).Return(headers).Repeat.Any();
            Expect.Call<HttpStatusCode>(response.StatusCode).Return(statusCode).Repeat.Any();
            Expect.Call<string>(response.StatusDescription).Return(statusDescription).Repeat.Any();
        }

        #endregion
    }
}
