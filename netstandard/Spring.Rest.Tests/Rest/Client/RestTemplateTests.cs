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
using System.Collections.Generic;

using Spring.Http;
using Spring.Http.Client;
using Spring.Http.Converters;

using NUnit.Framework;
using Rhino.Mocks;
using Spring.Http.Converters.Json;
using Spring.Rest.Tests.Rest.Client.DTO;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Unit tests for the RestTemplate class.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia (.NET)</author>
    [TestFixture]
    public class RestTemplateTests
    {
        #region Logging

        private static readonly Common.Logging.ILog LOG = Common.Logging.LogManager.GetLogger(typeof(RestTemplateTests));

        #endregion

        private MockRepository mocks;
        private RestTemplate template;
	    private IClientHttpRequestFactory requestFactory;
        private IClientHttpRequest request;
        private IClientHttpResponse response;
        private IResponseErrorHandler errorHandler;
	    private IHttpMessageConverter converter;

	    [SetUp]
	    public void SetUp() 
        {
            mocks = new MockRepository();
            requestFactory = mocks.StrictMock<IClientHttpRequestFactory>();
            request = mocks.StrictMock<IClientHttpRequest>();
            response = mocks.StrictMock<IClientHttpResponse>();
            errorHandler = mocks.StrictMock<IResponseErrorHandler>();
            converter = mocks.StrictMock<IHttpMessageConverter>();
            
            IList<IHttpMessageConverter> messageConverters = new List<IHttpMessageConverter>(1);
            messageConverters.Add(converter);

            template = new RestTemplate();
            template.RequestFactory = requestFactory;
            template.MessageConverters = messageConverters;
            template.ErrorHandler = errorHandler;
	    }

        [TearDown]
        public void TearDown()
        {
            mocks.VerifyAll();
        }

	    [Test]
	    public void VarArgsTemplateVariables() 
        {
            Uri requestUri = new Uri("http://example.com/hotels/42/bookings/21");
            HttpMethod requestMethod = HttpMethod.GET;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);

		    mocks.ReplayAll();

		    template.Execute<object>("http://example.com/hotels/{hotel}/bookings/{booking}", HttpMethod.GET, null, null, "42", "21");
	    }

        [Test]
        public void DictionaryTemplateVariables()
        {
            Uri requestUri = new Uri("http://example.com/hotels/42/bookings/21");
            HttpMethod requestMethod = HttpMethod.GET;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);

		    mocks.ReplayAll();

            IDictionary<string, string> variables = new Dictionary<string, string>();
            variables.Add("booking", "41");
            variables.Add("hotel", "42");
            template.Execute<object>("http://example.com/hotels/{hotel}/bookings/{booking}", HttpMethod.GET, null, null, "42", "21");
        }

        [Test]
        public void BaseAddressTemplate()
        {
            Uri requestUri = new Uri("http://example.com/hotels/42/bookings/21");
            HttpMethod requestMethod = HttpMethod.GET;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);

            mocks.ReplayAll();

            template.BaseAddress = new Uri("http://example.com");
            template.Execute<object>("hotels/{hotel}/bookings/{booking}", HttpMethod.GET, null, null, "42", "21");
        }

        [Test]
        [ExpectedException(typeof(HttpServerErrorException),
            ExpectedMessage = "GET request for 'http://example.com/' resulted in 500 - InternalServerError (Internal Server Error).")]
        public void ErrorHandling()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.GET;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(true);
            Expect.Call(delegate() { errorHandler.HandleError(requestUri, requestMethod, response); }).Throw(new HttpServerErrorException(
                requestUri, requestMethod, new HttpResponseMessage<byte[]>(new byte[0], HttpStatusCode.InternalServerError, "Internal Server Error")));

            mocks.ReplayAll();

            template.Execute<object>("http://example.com", HttpMethod.GET, null, null);
        }

        [Test]
        public void GetForObject() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.GET;
            Expect.Call<bool>(converter.CanRead(typeof(string), null)).Return(true);
            MediaType textPlain = new MediaType("text", "plain");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(textPlain);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            responseHeaders.ContentType = textPlain;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Expect.Call<bool>(converter.CanRead(typeof(string), textPlain)).Return(true);
            String expected = "Hello World";
            Expect.Call<string>(converter.Read<string>(response)).Return(expected);

            mocks.ReplayAll();

            string result = template.GetForObject<string>("http://example.com");
            Assert.AreEqual(expected, result, "Invalid GET result");
            Assert.AreEqual(textPlain.ToString(), requestHeaders.GetSingleValue("Accept"), "Invalid Accept header");
        }

        [Test]
        [ExpectedException(typeof(RestClientException),
            ExpectedMessage = "Could not extract response: no suitable HttpMessageConverter found for response type [System.String] and content type [bar/baz]")]
        public void GetUnsupportedMediaType() 
        {
            Uri requestUri = new Uri("http://example.com/resource");
            HttpMethod requestMethod = HttpMethod.GET;
            Expect.Call<bool>(converter.CanRead(typeof(string), null)).Return(true);
            MediaType textPlain = new MediaType("foo", "bar");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(textPlain);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            MediaType contentType = new MediaType("bar", "baz");
            responseHeaders.ContentType = contentType;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Expect.Call<bool>(converter.CanRead(typeof(string), contentType)).Return(false);

            mocks.ReplayAll();

            template.GetForObject<string>("http://example.com/{p}", "resource");
        }

        [Test]
        public void GetNoContentType()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.GET;
            Expect.Call<bool>(converter.CanRead(typeof(string), null)).Return(true);
            MediaType applicationOctetStream = new MediaType("application", "octet-stream");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(applicationOctetStream);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            // No content-type
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Expect.Call<bool>(converter.CanRead(typeof(string), applicationOctetStream)).Return(true);
            String expected = "Hello World";
            Expect.Call<string>(converter.Read<string>(response)).Return(expected);

            mocks.ReplayAll();

            string result = template.GetForObject<string>("http://example.com");
            Assert.AreEqual(expected, result, "Invalid GET result");
            Assert.AreEqual(applicationOctetStream.ToString(), requestHeaders.GetSingleValue("Accept"), "Invalid Accept header");
        }

        [Test]
        [ExpectedException(typeof(RestClientException),
            ExpectedMessage = "Could not write request: no suitable IHttpMessageConverter found for request type [System.String] and content type [foo/bar]")]
        public void PostUnsupportedMediaType()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            string helloWorld = "Hello World";
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            MediaType contentType = new MediaType("foo", "bar");
            Expect.Call<bool>(converter.CanWrite(typeof(string), contentType)).Return(false);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();

            mocks.ReplayAll();

            HttpEntity entity = new HttpEntity(helloWorld);
            entity.Headers.ContentType = contentType;
            template.PostForLocation("http://example.com", entity);
        }

        [Test]
        public void GetForMessage() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.GET;
            Expect.Call<bool>(converter.CanRead(typeof(string), null)).Return(true);
            MediaType textPlain = new MediaType("text", "plain");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(textPlain);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            responseHeaders.ContentType = textPlain;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Expect.Call<bool>(converter.CanRead(typeof(string), textPlain)).Return(true);
            String expected = "Hello World";
            Expect.Call<string>(converter.Read<string>(response)).Return(expected);
            Expect.Call<HttpStatusCode>(response.StatusCode).Return(HttpStatusCode.OK);
            Expect.Call<string>(response.StatusDescription).Return("OK");

            mocks.ReplayAll();

            HttpResponseMessage<String> result = template.GetForMessage<string>("http://example.com");
            Assert.AreEqual(expected, result.Body, "Invalid GET result");
            Assert.AreEqual(textPlain, result.Headers.ContentType, "Invalid Content-Type");
            Assert.AreEqual(textPlain.ToString(), requestHeaders.GetSingleValue("Accept"), "Invalid Accept header");
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid status code");
            Assert.AreEqual("OK", result.StatusDescription, "Invalid status description");

            mocks.ReplayAll();
        }

        [Test]
        public void HeadForHeaders()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.HEAD;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();

            mocks.ReplayAll();

            HttpHeaders result = template.HeadForHeaders("http://example.com");

            Assert.AreSame(responseHeaders, result, "Invalid headers returned");
        }

        [Test]
        public void PostForLocation() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            string helloWorld = "Hello World";
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            Expect.Call<bool>(converter.CanWrite(typeof(string), null)).Return(true);
            converter.Write(helloWorld, null, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            Uri expected = new Uri("http://example.com/hotels");
            responseHeaders.Location = expected;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();

            mocks.ReplayAll();

            Uri result = template.PostForLocation("http://example.com", helloWorld);
            Assert.AreEqual(expected, result, "Invalid POST result");
        }

        [Test]
        public void PostForLocationMessageContentType() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            string helloWorld = "Hello World";
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            MediaType contentType = new MediaType("text", "plain");
            Expect.Call<bool>(converter.CanWrite(typeof(string), contentType)).Return(true);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            converter.Write(helloWorld, contentType, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            Uri expected = new Uri("http://example.com/hotels");
            responseHeaders.Location = expected;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();

            mocks.ReplayAll();

            HttpHeaders entityHeaders = new HttpHeaders();
            entityHeaders.ContentType = contentType;
            HttpEntity entity = new HttpEntity(helloWorld, entityHeaders);

            Uri result = template.PostForLocation("http://example.com", entity);
            Assert.AreEqual(expected, result, "Invalid POST result");
        }

        [Test]
        public void PostForLocationMessageCustomHeader() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            string helloWorld = "Hello World";
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            Expect.Call<bool>(converter.CanWrite(typeof(string), null)).Return(true);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            converter.Write(helloWorld, null, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            Uri expected = new Uri("http://example.com/hotels");
            responseHeaders.Location = expected;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();

            mocks.ReplayAll();

            HttpHeaders entityHeaders = new HttpHeaders();
            entityHeaders.Add("MyHeader", "MyValue");
            HttpEntity entity = new HttpEntity(helloWorld, entityHeaders);

            Uri result = template.PostForLocation("http://example.com", entity);
            Assert.AreEqual(expected, result, "Invalid POST result");
            Assert.AreEqual("MyValue", requestHeaders.Get("MyHeader"), "No custom header set");
        }

        [Test]
        public void PostForLocationNoLocation() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            string helloWorld = "Hello World";
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            Expect.Call<bool>(converter.CanWrite(typeof(string), null)).Return(true);
            converter.Write(helloWorld, null, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();

            mocks.ReplayAll();

            Uri result = template.PostForLocation("http://example.com", helloWorld);
            Assert.IsNull(result, "Invalid POST result");

            mocks.ReplayAll();
        }

        [Test]
        public void PostForLocationNull()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();

            mocks.ReplayAll();

            template.PostForLocation("http://example.com", null);

            Assert.AreEqual(0, requestHeaders.ContentLength, "Invalid content length");
        }

        [Test]
        public void PostForObject() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            Expect.Call<bool>(converter.CanRead(typeof(Version), null)).Return(true);
            MediaType textPlain = new MediaType("text", "plain");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(textPlain);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            string helloWorld = "Hello World";
            Expect.Call<bool>(converter.CanWrite(typeof(string), null)).Return(true);
            converter.Write(helloWorld, null, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            responseHeaders.ContentType = textPlain;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Version expected = new Version(1, 0);
            Expect.Call<bool>(converter.CanRead(typeof(Version), textPlain)).Return(true);
            Expect.Call<Version>(converter.Read<Version>(response)).Return(expected);

            mocks.ReplayAll();

            Version result = template.PostForObject<Version>("http://example.com", helloWorld);
            Assert.AreEqual(expected, result, "Invalid POST result");
            Assert.AreEqual(textPlain.ToString(), requestHeaders.GetSingleValue("Accept"), "Invalid Accept header");
        }

        [Test]
        public void PostForMessage() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            Expect.Call<bool>(converter.CanRead(typeof(Version), null)).Return(true);
            MediaType textPlain = new MediaType("text", "plain");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(textPlain);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            string helloWorld = "Hello World";
            Expect.Call<bool>(converter.CanWrite(typeof(string), null)).Return(true);
            converter.Write(helloWorld, null, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            responseHeaders.ContentType = textPlain;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Version expected = new Version(1, 0);
            Expect.Call<bool>(converter.CanRead(typeof(Version), textPlain)).Return(true);
            Expect.Call<Version>(converter.Read<Version>(response)).Return(expected);
            Expect.Call<HttpStatusCode>(response.StatusCode).Return(HttpStatusCode.OK);
            Expect.Call<string>(response.StatusDescription).Return("OK");

            mocks.ReplayAll();

            HttpResponseMessage<Version> result = template.PostForMessage<Version>("http://example.com", helloWorld);
            Assert.AreEqual(expected, result.Body, "Invalid POST result");
            Assert.AreEqual(textPlain, result.Headers.ContentType, "Invalid Content-Type");
            Assert.AreEqual(textPlain.ToString(), requestHeaders.GetSingleValue("Accept"), "Invalid Accept header");
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid status code");
            Assert.AreEqual("OK", result.StatusDescription, "Invalid status description");
        }

        [Test]
        public void PostForMessageNoBody()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            string helloWorld = "Hello World";
            Expect.Call<bool>(converter.CanWrite(typeof(string), null)).Return(true);
            converter.Write(helloWorld, null, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders);
            Expect.Call<HttpStatusCode>(response.StatusCode).Return(HttpStatusCode.Created);
            Expect.Call<string>(response.StatusDescription).Return("CREATED");

            mocks.ReplayAll();

            HttpResponseMessage result = template.PostForMessage("http://example.com", helloWorld);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, "Invalid status code");
            Assert.AreEqual("CREATED", result.StatusDescription, "Invalid status description");
        }

        [Test]
        public void PostForObjectNull() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            Expect.Call<bool>(converter.CanRead(typeof(Version), null)).Return(true);
            MediaType textPlain = new MediaType("text", "plain");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(textPlain);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            responseHeaders.ContentType = textPlain;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Expect.Call<bool>(converter.CanRead(typeof(Version), textPlain)).Return(true);
            Expect.Call<Version>(converter.Read<Version>(response)).Return(null);

            mocks.ReplayAll();

            Version result = template.PostForObject<Version>("http://example.com", null);
            Assert.IsNull(result, "Invalid POST result");
            Assert.AreEqual(textPlain.ToString(), requestHeaders.GetSingleValue("Accept"), "Invalid Accept header");

            Assert.AreEqual(0, requestHeaders.ContentLength, "Invalid content length");
        }
    	
        [Test]
        public void PostForEntityNull() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            Expect.Call<bool>(converter.CanRead(typeof(Version), null)).Return(true);
            MediaType textPlain = new MediaType("text", "plain");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(textPlain);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            responseHeaders.ContentType = textPlain;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Expect.Call<bool>(converter.CanRead(typeof(Version), textPlain)).Return(true);
            Expect.Call<Version>(converter.Read<Version>(response)).Return(null);
            Expect.Call<HttpStatusCode>(response.StatusCode).Return(HttpStatusCode.OK);
            Expect.Call<string>(response.StatusDescription).Return("OK");

            mocks.ReplayAll();

            HttpResponseMessage<Version> result = template.PostForMessage<Version>("http://example.com", null);
            Assert.IsNull(result.Body, "Invalid POST result");
            Assert.AreEqual(textPlain, result.Headers.ContentType, "Invalid Content-Type");
            Assert.AreEqual(textPlain.ToString(), requestHeaders.GetSingleValue("Accept"), "Invalid Accept header");

            Assert.AreEqual(0, requestHeaders.ContentLength, "Invalid content length");
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid status code");
            Assert.AreEqual("OK", result.StatusDescription, "Invalid status description");
        }

        [Test]
        public void Put() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.PUT;
            Expect.Call<bool>(converter.CanWrite(typeof(string), null)).Return(true);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            string helloWorld = "Hello World";
            converter.Write(helloWorld, null, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);

            mocks.ReplayAll();

            template.Put("http://example.com", helloWorld);
        }

        [Test]
        public void PutNull()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.PUT;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);

            mocks.ReplayAll();

            template.Put("http://example.com", null);

            Assert.AreEqual(0, requestHeaders.ContentLength, "Invalid content length");
        }

        [Test]
        public void Delete()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.DELETE;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);

            mocks.ReplayAll();

            template.Delete("http://example.com");
        }

        [Test]
        public void OptionsForAllow()
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.OPTIONS;
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            responseHeaders.Add("Allow", "GET");
            responseHeaders.Add("Allow", "POST");
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();

            mocks.ReplayAll();

            IList<HttpMethod> result = template.OptionsForAllow("http://example.com");
            Assert.AreEqual(2, result.Count, "Invalid OPTIONS result");
            Assert.IsTrue(result.Contains(HttpMethod.GET), "Invalid OPTIONS result");
            Assert.IsTrue(result.Contains(HttpMethod.POST), "Invalid OPTIONS result");
        }

        [Test]
        public void Exchange() 
        {
            Uri requestUri = new Uri("http://example.com");
            HttpMethod requestMethod = HttpMethod.POST;
            Expect.Call<bool>(converter.CanRead(typeof(Version), null)).Return(true);
            MediaType textPlain = new MediaType("text", "plain");
            IList<MediaType> mediaTypes = new List<MediaType>(1);
            mediaTypes.Add(textPlain);
            Expect.Call<IList<MediaType>>(converter.SupportedMediaTypes).Return(mediaTypes);
            Expect.Call<IClientHttpRequest>(requestFactory.CreateRequest(requestUri, requestMethod)).Return(request);
            HttpHeaders requestHeaders = new HttpHeaders();
            Expect.Call<HttpHeaders>(request.Headers).Return(requestHeaders).Repeat.Any();
            string helloWorld = "Hello World";
            Expect.Call<bool>(converter.CanWrite(typeof(string), null)).Return(true);
            converter.Write(helloWorld, null, request);
            ExpectGetResponse();
            Expect.Call<bool>(errorHandler.HasError(requestUri, requestMethod, response)).Return(false);
            HttpHeaders responseHeaders = new HttpHeaders();
            responseHeaders.ContentType = textPlain;
            Expect.Call<HttpHeaders>(response.Headers).Return(responseHeaders).Repeat.Any();
            ExpectHasMessageBody(responseHeaders);
            Version expected = new Version(1, 0);
            Expect.Call<bool>(converter.CanRead(typeof(Version), textPlain)).Return(true);
            Expect.Call<Version>(converter.Read<Version>(response)).Return(expected);
            Expect.Call<HttpStatusCode>(response.StatusCode).Return(HttpStatusCode.OK);
            Expect.Call<string>(response.StatusDescription).Return("OK");

            mocks.ReplayAll();

            HttpHeaders requestMessageHeaders = new HttpHeaders();
            requestMessageHeaders.Add("MyHeader", "MyValue");
            HttpEntity requestEntity = new HttpEntity(helloWorld, requestMessageHeaders);
            HttpResponseMessage<Version> result = template.Exchange<Version>("http://example.com", HttpMethod.POST, requestEntity);
            Assert.AreEqual(expected, result.Body, "Invalid POST result");
            Assert.AreEqual(textPlain, result.Headers.ContentType, "Invalid Content-Type");
            Assert.AreEqual(textPlain.ToString(), requestHeaders.GetSingleValue("Accept"), "Invalid Accept header");
            Assert.AreEqual("MyValue", requestHeaders.Get("MyHeader"), "No custom header set");
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid status code");
            Assert.AreEqual("OK", result.StatusDescription, "Invalid status description");
        }

        //[Test]
        //public void TestGetFormNetCore() 
        //{
        //    var client = new RestTemplate("http://localhost:55622/");
        //    client.MessageConverters = new List<IHttpMessageConverter>();
        //    client.MessageConverters.Add(new NJsonHttpMessageConverter());

        //    var data = client.GetForObject<PagedList<BusinessLoginLogDto>>("BusinessLoginLog/GetList");
        //}

        #region Utility methods

        private void ExpectGetResponse()
        {
            Expect.Call<IClientHttpResponse>(request.Execute()).Return(response);
            #region Instrumentation
            if (LOG.IsDebugEnabled)
            {
                Expect.Call<HttpStatusCode>(response.StatusCode).Return(HttpStatusCode.OK);
                Expect.Call<string>(response.StatusDescription).Return("OK");
            }
            #endregion
            Expect.Call(response.Dispose);
        }

        private void ExpectHasMessageBody(HttpHeaders responseHeaders)
        {
            responseHeaders.ContentLength = 1;
            Expect.Call<HttpStatusCode>(response.StatusCode).Return(HttpStatusCode.OK).Repeat.Twice();
        }

        #endregion
    }
}
