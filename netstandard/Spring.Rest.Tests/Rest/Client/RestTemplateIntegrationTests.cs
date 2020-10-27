﻿#if NET_3_5
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
using System.IO;
using System.Text;
using System.Threading;
#if NET_4_0 || SILVERLIGHT_5
using System.Threading.Tasks;
#endif
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using NUnit.Framework;

using Spring.Http;
using Spring.Http.Client;
using Spring.Http.Client.Interceptor;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Integration tests for the RestTemplate class.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia (.NET)</author>
    [TestFixture]
    public class RestTemplateIntegrationTests
    {
        #region Logging

        private static readonly Common.Logging.ILog LOG = Common.Logging.LogManager.GetLogger(typeof(RestTemplateIntegrationTests));

        #endregion

        private WebServiceHost webServiceHost;
        private string uri = "http://localhost:1337";
        private RestTemplate template;
        private MediaType contentType;

        [SetUp]
        public void SetUp()
        {
            template = new RestTemplate(uri);
            contentType = new MediaType("text", "plain");

            webServiceHost = new WebServiceHost(typeof(TestService), new Uri(uri));
            webServiceHost.Open();
        }

        [TearDown]
        public void TearDown()
        {
            webServiceHost.Close();
        }

        #region Sync

        [Test]
        public void GetString()
        {
            string result = template.GetForObject<string>("users");
            Assert.AreEqual("2", result, "Invalid content");
        }

        [Test(Description = "SPRNETREST-4")]
        public void GetStringWithHeaders()
        {
            HttpEntity entity = new HttpEntity();
            entity.Headers["MyHeader"] = "MyValue";
            HttpResponseMessage<string> result = template.Exchange<string>("users", HttpMethod.GET, entity);
            Assert.AreEqual("2", result.Body, "Invalid content");
        }

        [Test]
        public void GetStringVarArgsTemplateVariables()
        {
            string result = template.GetForObject<string>("user/{id}", 1);
            Assert.AreEqual("Bruno Baïa", result, "Invalid content");
        }

        [Test]
        public void GetStringDictionaryTemplateVariables()
        {
            IDictionary<string, object> uriVariables = new Dictionary<string, object>(1);
            uriVariables.Add("id", 2);
            string result = template.GetForObject<string>("user/{id}", uriVariables);
            Assert.AreEqual("Marie Baia", result, "Invalid content");
        }

        [Test]
        [ExpectedException(typeof(HttpClientErrorException),
            ExpectedMessage = "GET request for 'http://localhost:1337/user/5' resulted in 404 - NotFound (User with id '5' not found).")]
        public void GetStringError()
        {
            string result = template.GetForObject<string>("user/{id}", 5);
        }

        [Test]
        public void GetStringForMessage()
        {
            HttpResponseMessage<string> result = template.GetForMessage<string>("user/{id}", 1);
            Assert.AreEqual("Bruno Baïa", result.Body, "Invalid content");
            Assert.AreEqual(new MediaType("text", "plain", "utf-8"), result.Headers.ContentType, "Invalid content-type");
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid status code");
            Assert.AreEqual("OK", result.StatusDescription, "Invalid status description");
        }

        [Test]
        public void GetStringNoResponse()
        {
            string result = template.GetForObject<string>("nothing");
            Assert.IsNull(result);
        }
 	 	 		
        [Test]
        public void GetNoContent() 
        {
            string result = template.GetForObject<string>("status/nocontent");
            Assert.IsNull(result, "Invalid content");
 	 	 		                
            HttpResponseMessage<string> response = template.GetForMessage<string>("status/nocontent");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "Invalid response code");
            Assert.IsNull(response.Body, "Invalid content");
        }

        [Test]
        public void GetNoModified()
        {
            string result = template.GetForObject<string>("status/notmodified");
            Assert.IsNull(result, "Invalid content");

            HttpResponseMessage<string> response = template.GetForMessage<string>("status/notmodified");
            Assert.AreEqual(HttpStatusCode.NotModified, response.StatusCode, "Invalid response code");
            Assert.IsNull(response.Body, "Invalid content");
        }

        [Test]
        public void HeadForHeaders()
        {
            HttpHeaders result = template.HeadForHeaders("head");
            Assert.AreEqual("MyValue", result["MyHeader"], "Invalid header");
        }

        [Test]
        public void PostStringForLocation()
        {
            Uri result = template.PostForLocation("user", "Lisa Baia");
            Assert.AreEqual(new Uri(new Uri(uri), "/user/3"), result, "Invalid location");
        }

        [Test]
        public void PostStringForMessage()
        {
            HttpResponseMessage<string> result = template.PostForMessage<string>("user", "Lisa Baia");
            Assert.AreEqual(new Uri(new Uri(uri), "/user/3"), result.Headers.Location, "Invalid location");
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, "Invalid status code");
            Assert.AreEqual("User id '3' created with 'Lisa Baia'", result.StatusDescription, "Invalid status description");
            Assert.AreEqual("3", result.Body, "Invalid content");
        }

        [Test]
        public void PostStringForObject()
        {
            string result = template.PostForObject<string>("user", "Lisa Baia");
            Assert.AreEqual("3", result, "Invalid content");
        }

        [Test]
        [ExpectedException(typeof(HttpClientErrorException),
            ExpectedMessage = "POST request for 'http://localhost:1337/user' resulted in 400 - BadRequest (Content cannot be null or empty).")]
        public void PostStringForObjectWithError()
        {
            string result = template.PostForObject<string>("user", "");
        }

        [Test]
        [ExpectedException(typeof(HttpClientErrorException),
            ExpectedMessage = "POST request for 'http://localhost:1337/user' resulted in 400 - BadRequest (Content cannot be null or empty).")]
        public void PostStringNull()
        {
            template.PostForObject<string>("user", null);
        }

        [Test]
        public void Put()
        {
            string result = template.GetForObject<string>("user/1");
            Assert.AreEqual("Bruno Baïa", result, "Invalid content");

            template.Put("user/1", "Bruno Baia");

            result = template.GetForObject<string>("user/1");
            Assert.AreEqual("Bruno Baia", result, "Invalid content");
        }

        [Test]
        [ExpectedException(typeof(HttpClientErrorException),
            ExpectedMessage = "PUT request for 'http://localhost:1337/user/4' resulted in 400 - BadRequest (User id '4' does not exist).")]
        public void PutWithError()
        {
            template.Put("user/4", "Dinora Baia");
        }

        [Test]
        public void Delete()
        {
            string result = template.GetForObject<string>("users");
            Assert.AreEqual("2", result, "Invalid content");

            template.Delete("user/2");

            result = template.GetForObject<string>("users");
            Assert.AreEqual("1", result, "Invalid content");
        }

        [Test]
        [ExpectedException(typeof(HttpClientErrorException),
            ExpectedMessage = "DELETE request for 'http://localhost:1337/user/10' resulted in 400 - BadRequest (User id '10' does not exist).")]
        public void DeleteWithError()
        {
            template.Delete("user/10");
        }

        [Test]
        public void OptionsForAllow()
        {
            IList<HttpMethod> result = template.OptionsForAllow("allow");
            Assert.AreEqual(3, result.Count, "Invalid response");
            Assert.IsTrue(result.Contains(HttpMethod.GET), "Invalid response");
            Assert.IsTrue(result.Contains(HttpMethod.HEAD), "Invalid response");
            Assert.IsTrue(result.Contains(HttpMethod.PUT), "Invalid response");
        }

        [Test]
        public void ExchangeForResponse()
        {
            HttpResponseMessage<string> result = template.Exchange<string>(
                "user", HttpMethod.POST, new HttpEntity("Maryse Baia"));

            Assert.AreEqual("3", result.Body, "Invalid content");
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, "Invalid status code");
            Assert.AreEqual("User id '3' created with 'Maryse Baia'", result.StatusDescription, "Invalid status description");
        }

        [Test]
        public void ExchangeForMessage()
        {
            HttpResponseMessage result = template.Exchange(
                "user/1", HttpMethod.PUT, new HttpEntity("Bruno Baia"));

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid status code");
            Assert.AreEqual("User id '1' updated with 'Bruno Baia'", result.StatusDescription, "Invalid status description");
        }

        [Test]
        public void ExchangeWithHeaders()
        {
            HttpHeaders headers = new HttpHeaders();
            headers.ContentLength = 11;
            HttpEntity entity = new HttpEntity("Maryse Baia", headers);

            HttpResponseMessage<string> result = template.Exchange<string>(
                "user", HttpMethod.POST, entity);

            Assert.AreEqual("3", result.Body, "Invalid content");
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, "Invalid status code");
            Assert.AreEqual("User id '3' created with 'Maryse Baia'", result.StatusDescription, "Invalid status description");
        }

        [Test]
        public void ClientError()
        {
            try
            {
                template.Execute<object>("status/notfound", HttpMethod.GET, null, null);
                Assert.Fail("RestTemplate should throw an exception");
            }
            catch (Exception ex)
            {
                HttpClientErrorException clientErrorException = ex as HttpClientErrorException;
                Assert.IsNotNull(clientErrorException, "Exception HttpClientErrorException expected");
                Assert.AreEqual("GET request for 'http://localhost:1337/status/notfound' resulted in 404 - NotFound (Not Found).", clientErrorException.Message);
                Assert.IsTrue(clientErrorException.Response.Body.Length == 0);
                Assert.IsTrue(clientErrorException.Response.Headers.ContentLength == 0);
                Assert.AreEqual(String.Empty, clientErrorException.GetResponseBodyAsString());
                Assert.AreEqual(HttpStatusCode.NotFound, clientErrorException.Response.StatusCode);
                Assert.AreEqual("Not Found", clientErrorException.Response.StatusDescription);
            }
        }

        [Test]
        public void ServerError()
        {
            try
            {
                template.Execute<object>("status/server", HttpMethod.GET, null, null);
                Assert.Fail("RestTemplate should throw an exception");
            }
            catch (Exception ex)
            {
                HttpServerErrorException serverErrorException = ex as HttpServerErrorException;
                Assert.IsNotNull(serverErrorException, "Exception HttpServerErrorException expected");
                Assert.AreEqual("GET request for 'http://localhost:1337/status/server' resulted in 500 - InternalServerError (Internal Server Error).", serverErrorException.Message);
                Assert.IsTrue(serverErrorException.Response.Body.Length == 0);
                Assert.IsTrue(serverErrorException.Response.Headers.ContentLength == 0);
                Assert.AreEqual(String.Empty, serverErrorException.GetResponseBodyAsString());
                Assert.AreEqual(HttpStatusCode.InternalServerError, serverErrorException.Response.StatusCode);
                Assert.AreEqual("Internal Server Error", serverErrorException.Response.StatusDescription);
            }
        }

        [Test]
        public void UsingInterceptors()
        {
            NoOpRequestSyncInterceptor.counter = 0;
            NoOpRequestBeforeInterceptor.counter = 0;
            NoOpRequestSyncInterceptor interceptor1 = new NoOpRequestSyncInterceptor();
            NoOpRequestBeforeInterceptor interceptor2 = new NoOpRequestBeforeInterceptor();
            NoOpRequestSyncInterceptor interceptor3 = new NoOpRequestSyncInterceptor();
            template.RequestInterceptors.Add(interceptor1);
            template.RequestInterceptors.Add(interceptor2);
            template.RequestInterceptors.Add(interceptor3);

            string result = template.PostForObject<string>("user", "Lisa Baia");
            Assert.AreEqual("3", result, "Invalid content");

            Assert.AreEqual(1, interceptor1.HandleRequestCounter);
            Assert.AreEqual(2, interceptor3.HandleRequestCounter);
            Assert.AreEqual(3, interceptor3.HandleResponseCounter);
            Assert.AreEqual(4, interceptor1.HandleResponseCounter);
            Assert.AreEqual(1, interceptor2.HandleCounter);
        }

        #endregion

        #region Async

        [Test]
        public void GetStringAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            template.GetForObjectAsync<string>("users",
                delegate(RestOperationCompletedEventArgs<string> args)
                {
                    try
                    {
                        Assert.IsNull(args.Error, "Invalid response");
                        Assert.IsFalse(args.Cancelled, "Invalid response");
                        Assert.AreEqual("2", args.Response, "Invalid content");
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                });

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        [Test]
        public void GetStringForMessageAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            template.GetForMessageAsync<string>("user/{id}",
                delegate(RestOperationCompletedEventArgs<HttpResponseMessage<string>> args)
                {
                    try
                    {
                        Assert.IsNull(args.Error, "Invalid response");
                        Assert.IsFalse(args.Cancelled, "Invalid response");
                        Assert.AreEqual("Bruno Baïa", args.Response.Body, "Invalid content");
                        Assert.AreEqual(new MediaType("text", "plain", "utf-8"), args.Response.Headers.ContentType, "Invalid content-type");
                        Assert.AreEqual(HttpStatusCode.OK, args.Response.StatusCode, "Invalid status code");
                        Assert.AreEqual("OK", args.Response.StatusDescription, "Invalid status description");
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                }, 1);

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        [Test]
        public void PostStringForMessageAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            template.PostForMessageAsync<string>("user", "Lisa Baia", 
                delegate(RestOperationCompletedEventArgs<HttpResponseMessage<string>> args)
                {
                    try
                    {
                        Assert.IsNull(args.Error, "Invalid response");
                        Assert.IsFalse(args.Cancelled, "Invalid response");
                        Assert.AreEqual(new Uri(new Uri(uri), "/user/3"), args.Response.Headers.Location, "Invalid location");
                        Assert.AreEqual(HttpStatusCode.Created, args.Response.StatusCode, "Invalid status code");
                        Assert.AreEqual("User id '3' created with 'Lisa Baia'", args.Response.StatusDescription, "Invalid status description");
                        Assert.AreEqual("3", args.Response.Body, "Invalid content");
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                });

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        [Test]
        public void DeleteAsyncWithNoAction()
        {
            string result = template.GetForObject<string>("users");
            Assert.AreEqual("2", result, "Invalid content");

            template.DeleteAsync("user/2", (Action<RestOperationCompletedEventArgs<object>>)null);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            result = template.GetForObject<string>("users");
            Assert.AreEqual("1", result, "Invalid content");
        }

        [Test]
        [ExpectedException(typeof(RestClientException),
            ExpectedMessage = "Could not write request: no suitable IHttpMessageConverter found for request type [System.Int32]")]
        public void ExceptionBeforeAsync()
        {
            // An exception occurs during request preparation, before async calls.
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            template.ExchangeAsync("user/1", HttpMethod.PUT, new HttpEntity(1),
                delegate(RestOperationCompletedEventArgs<HttpResponseMessage> args)
                {
                    try
                    {
                        Assert.IsFalse(args.Cancelled, "Invalid response");

                        Assert.IsNotNull(args.Error, "Invalid response");
                        exception = args.Error;
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                });

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        [Test]
        public void ExchangeForMessageAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            template.ExchangeAsync("user/1", HttpMethod.PUT, new HttpEntity("Bruno Baia"), 
                delegate(RestOperationCompletedEventArgs<HttpResponseMessage> args)
                {
                    try
                    {
                        Assert.IsNull(args.Error, "Invalid response");
                        Assert.IsFalse(args.Cancelled, "Invalid response");
                        Assert.AreEqual(HttpStatusCode.OK, args.Response.StatusCode, "Invalid status code");
                        Assert.AreEqual("User id '1' updated with 'Bruno Baia'", args.Response.StatusDescription, "Invalid status description");
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                });

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        [Test]
        [ExpectedException(typeof(HttpClientErrorException),
            ExpectedMessage = "GET request for 'http://localhost:1337/status/notfound' resulted in 404 - NotFound (Not Found).")]
        public void ClientErrorAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            template.ExecuteAsync("status/notfound", HttpMethod.GET, null, null, 
                delegate(RestOperationCompletedEventArgs<object> args)
                {
                    try
                    {
                        Assert.IsFalse(args.Cancelled, "Invalid response");

                        Assert.IsNotNull(args.Error, "Invalid response");
                        exception = args.Error;
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                });

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        [Test]
        public void ServerErrorAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            template.ExecuteAsync("status/server", HttpMethod.GET, null, null,
                delegate(RestOperationCompletedEventArgs<object> args)
                {
                    try
                    {
                        Assert.IsFalse(args.Cancelled, "Invalid response");
                        Assert.IsNotNull(args.Error, "Invalid response");
                        HttpServerErrorException serverErrorException = args.Error as HttpServerErrorException;
                        Assert.IsNotNull(serverErrorException, "Exception HttpServerErrorException expected");
                        Assert.AreEqual("GET request for 'http://localhost:1337/status/server' resulted in 500 - InternalServerError (Internal Server Error).", serverErrorException.Message);
                        Assert.IsTrue(serverErrorException.Response.Body.Length == 0);
                        Assert.IsTrue(serverErrorException.Response.Headers.ContentLength == 0);
                        Assert.AreEqual(String.Empty, serverErrorException.GetResponseBodyAsString());
                        Assert.AreEqual(HttpStatusCode.InternalServerError, serverErrorException.Response.StatusCode);
                        Assert.AreEqual("Internal Server Error", serverErrorException.Response.StatusDescription);

                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                });

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        [Test]
        public void CancelAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            RestOperationCanceler canceler = template.ExchangeAsync("sleep/2", HttpMethod.GET, null, 
                delegate(RestOperationCompletedEventArgs<HttpResponseMessage> args)
                {
                    try
                    {
                        Assert.IsTrue(args.Cancelled, "Invalid response");

                        WebException webEx = args.Error as WebException;
                        Assert.IsNotNull(webEx, "Invalid response exception");
                        Assert.AreEqual(WebExceptionStatus.RequestCanceled, webEx.Status, "Invalid response exception status");

                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                });

            canceler.Cancel();

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        [Test]
        public void UsingInterceptorsAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            NoOpRequestAsyncInterceptor.counter = 0;
            NoOpRequestSyncInterceptor.counter = 0;
            NoOpRequestBeforeInterceptor.counter = 0;
            NoOpRequestAsyncInterceptor interceptor1 = new NoOpRequestAsyncInterceptor();
            NoOpRequestSyncInterceptor interceptor2 = new NoOpRequestSyncInterceptor();
            NoOpRequestBeforeInterceptor interceptor3 = new NoOpRequestBeforeInterceptor();
            NoOpRequestAsyncInterceptor interceptor4 = new NoOpRequestAsyncInterceptor();
            template.RequestInterceptors.Add(interceptor1);
            template.RequestInterceptors.Add(interceptor2);
            template.RequestInterceptors.Add(interceptor3);
            template.RequestInterceptors.Add(interceptor4);

            template.PostForObjectAsync<string>("user", "Lisa Baia",
                delegate(RestOperationCompletedEventArgs<string> args)
                {
                    try
                    {
                        Assert.IsNull(args.Error, "Invalid response");
                        Assert.IsFalse(args.Cancelled, "Invalid response");
                        Assert.AreEqual("3", args.Response, "Invalid content");
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        manualEvent.Set();
                    }
                });

            manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }

            Assert.AreEqual(1, interceptor1.HandleRequestCounter);
            Assert.AreEqual(2, interceptor4.HandleRequestCounter);
            Assert.AreEqual(3, interceptor4.HandleResponseCounter);
            Assert.AreEqual(4, interceptor1.HandleResponseCounter);
            Assert.AreEqual(1, interceptor3.HandleCounter);
            Assert.AreEqual(0, interceptor2.HandleRequestCounter);
            Assert.AreEqual(0, interceptor2.HandleResponseCounter);
        }

        #endregion

        #region Async (TPL)

#if NET_4_0 || SILVERLIGHT_5
        [Test]
        public void GetStringTaskAsync()
        {
            Assert.AreEqual("2", template.GetForObjectAsync<string>("users").Result);

            template.GetForObjectAsync<string>("users")
                .ContinueWith(task => 
                {
                    Assert.IsFalse(task.IsCanceled, "Invalid response");
                    Assert.IsFalse(task.IsFaulted, "Invalid response");
                    Assert.AreEqual("2", task.Result, "Invalid content");
                })
                .Wait();
        }

        [Test]
        public void GetStringForMessageTaskAsync()
        {
            Assert.AreEqual("Bruno Baïa", template.GetForMessageAsync<string>("user/{id}", 1).Result.Body);

            template.GetForMessageAsync<string>("user/{id}", 1)
                .ContinueWith(task =>
                {
                    Assert.IsFalse(task.IsCanceled, "Invalid response");
                    Assert.IsFalse(task.IsFaulted, "Invalid response");
                    Assert.AreEqual("Bruno Baïa", task.Result.Body, "Invalid content");
                    Assert.AreEqual(new MediaType("text", "plain", "utf-8"), task.Result.Headers.ContentType, "Invalid content-type");
                    Assert.AreEqual(HttpStatusCode.OK, task.Result.StatusCode, "Invalid status code");
                    Assert.AreEqual("OK", task.Result.StatusDescription, "Invalid status description");
                })
                .Wait();
        }

        [Test]
        public void PostStringForMessageTaskAsync()
        {
            template.PostForMessageAsync<string>("user", "Lisa Baia")
                .ContinueWith(task =>
                {
                    Assert.IsFalse(task.IsCanceled, "Invalid response");
                    Assert.IsFalse(task.IsFaulted, "Invalid response");
                    Assert.AreEqual(new Uri(new Uri(uri), "/user/3"), task.Result.Headers.Location, "Invalid location");
                    Assert.AreEqual(HttpStatusCode.Created, task.Result.StatusCode, "Invalid status code");
                    Assert.AreEqual("User id '3' created with 'Lisa Baia'", task.Result.StatusDescription, "Invalid status description");
                    Assert.AreEqual("3", task.Result.Body, "Invalid content");
                })
                .Wait();
        }

        [Test]
        public void DeleteAsyncWithNoContinuation()
        {
            string result = template.GetForObject<string>("users");
            Assert.AreEqual("2", result, "Invalid content");

            template.DeleteAsync("user/2").Wait();

            result = template.GetForObject<string>("users");
            Assert.AreEqual("1", result, "Invalid content");
        }

        [Test]
        public void ExceptionBeforeTaskAsync()
        {
            // An exception occurs during request preparation, before async calls.
            template.ExchangeAsync<object>("user/1", HttpMethod.PUT, new HttpEntity(1), CancellationToken.None)
                .ContinueWith(task =>
                {
                    Assert.IsFalse(task.IsCanceled, "Invalid response");
                    Assert.IsTrue(task.IsFaulted, "Invalid response");
                    Assert.IsNotNull(task.Exception, "Invalid response");
                    AssertAggregateException(task.Exception, typeof(RestClientException), "Could not write request: no suitable IHttpMessageConverter found for request type [System.Int32]");
                })
                .Wait();
        }

        [Test]
        public void ExchangeForMessageTaskAsync()
        {
            template.ExchangeAsync("user/1", HttpMethod.PUT, new HttpEntity("Bruno Baia"), CancellationToken.None)
                .ContinueWith(task =>
                {
                    Assert.IsFalse(task.IsCanceled, "Invalid response");
                    Assert.IsFalse(task.IsFaulted, "Invalid response");
                    Assert.AreEqual(HttpStatusCode.OK, task.Result.StatusCode, "Invalid status code");
                    Assert.AreEqual("User id '1' updated with 'Bruno Baia'", task.Result.StatusDescription, "Invalid status description");
                })
                .Wait();
        }

        [Test]
        public void ClientErrorTaskAsync()
        {
            template.ExecuteAsync<object>("status/notfound", HttpMethod.GET, null, null, CancellationToken.None)
                .ContinueWith(task =>
                {
                    Assert.IsFalse(task.IsCanceled, "Invalid response");
                    Assert.IsTrue(task.IsFaulted, "Invalid response");
                    Assert.IsNotNull(task.Exception, "Invalid response");
                    AssertAggregateException(task.Exception, typeof(HttpClientErrorException), "GET request for 'http://localhost:1337/status/notfound' resulted in 404 - NotFound (Not Found).");
                })
                .Wait();
        }

        [Test]
        public void ServerErrorTaskAsync()
        {
            template.ExecuteAsync<object>("status/server", HttpMethod.GET, null, null, CancellationToken.None)
                .ContinueWith(task =>
                {
                    Assert.IsFalse(task.IsCanceled, "Invalid response");
                    Assert.IsTrue(task.IsFaulted, "Invalid response");
                    Assert.IsNotNull(task.Exception, "Invalid response");
                    AssertAggregateException(task.Exception, typeof(HttpServerErrorException), "GET request for 'http://localhost:1337/status/server' resulted in 500 - InternalServerError (Internal Server Error).");
                })
                .Wait();
        }

        [Test]
        public void CancelTaskAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task exchangeTask = template.ExchangeAsync("sleep/2", HttpMethod.GET, null, cancellationTokenSource.Token)
                .ContinueWith(task =>
                {
                    Assert.IsTrue(task.IsCanceled, "Invalid response");
                });

            cancellationTokenSource.Cancel();

            exchangeTask.Wait();
        }

        [Test]
        public void UsingInterceptorsTaskAsync()
        {
            NoOpRequestAsyncInterceptor.counter = 0;
            NoOpRequestSyncInterceptor.counter = 0;
            NoOpRequestBeforeInterceptor.counter = 0;
            NoOpRequestAsyncInterceptor interceptor1 = new NoOpRequestAsyncInterceptor();
            NoOpRequestSyncInterceptor interceptor2 = new NoOpRequestSyncInterceptor();
            NoOpRequestBeforeInterceptor interceptor3 = new NoOpRequestBeforeInterceptor();
            NoOpRequestAsyncInterceptor interceptor4 = new NoOpRequestAsyncInterceptor();
            template.RequestInterceptors.Add(interceptor1);
            template.RequestInterceptors.Add(interceptor2);
            template.RequestInterceptors.Add(interceptor3);
            template.RequestInterceptors.Add(interceptor4);

            template.ExchangeAsync<string>("user", HttpMethod.POST, new HttpEntity("Lisa Baia"), CancellationToken.None)
                .ContinueWith(task =>
                {
                    Assert.IsFalse(task.IsCanceled, "Invalid response");
                    Assert.IsFalse(task.IsFaulted, "Invalid response");
                    Assert.AreEqual("3", task.Result.Body, "Invalid content");
                })
                .Wait();

            Assert.AreEqual(1, interceptor1.HandleRequestCounter);
            Assert.AreEqual(2, interceptor4.HandleRequestCounter);
            Assert.AreEqual(3, interceptor4.HandleResponseCounter);
            Assert.AreEqual(4, interceptor1.HandleResponseCounter);
            Assert.AreEqual(1, interceptor3.HandleCounter);
            Assert.AreEqual(0, interceptor2.HandleRequestCounter);
            Assert.AreEqual(0, interceptor2.HandleResponseCounter);
        }

        private void AssertAggregateException(AggregateException ae, Type expectedException, string expectedMessage)
        {
            ae.Handle(ex =>
            {
                if (expectedException.Equals(ex.GetType()))
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                    return true;
                }
                return false;
            });
        }
#endif

        #endregion

        #region Test classes

        private class NoOpRequestBeforeInterceptor : IClientHttpRequestBeforeInterceptor
        {
            public static int counter = 0;
            public int HandleCounter { get; set; }

            public void BeforeExecute(IClientHttpRequestContext request)
            {
                HandleCounter = ++counter;
            }
        }

        private class NoOpRequestSyncInterceptor : IClientHttpRequestSyncInterceptor
        {
            public static int counter = 0;
            public int HandleRequestCounter { get; set; }
            public int HandleResponseCounter { get; set; }

            public IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution)
            {
                HandleRequestCounter = ++counter;
                IClientHttpResponse response = execution.Execute();
                HandleResponseCounter = ++counter;
                return response;
            }
        }

        private class NoOpRequestAsyncInterceptor : IClientHttpRequestAsyncInterceptor
        {
            public static int counter = 0;
            public int HandleRequestCounter { get; set; }
            public int HandleResponseCounter { get; set; }

            public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
            {
                HandleRequestCounter = ++counter;
                execution.ExecuteAsync(
                    delegate(IClientHttpResponseAsyncContext ctx)
                    {
                        HandleResponseCounter = ++counter;
                    });
            }
        }

        #endregion

        #region REST test service

        [ServiceContract]
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class TestService
        {
            private IDictionary<string, string> users;

            public TestService()
            {
                users = new Dictionary<string, string>();
                users.Add("1", "Bruno Baïa");
                users.Add("2", "Marie Baia");
            }

            [OperationContract]
            [WebGet(UriTemplate = "status/notfound")]
            public void StatusNotFound()
            {
                WebOperationContext.Current.OutgoingResponse.SetStatusAsNotFound();
            }

            [OperationContract]
            [WebGet(UriTemplate = "status/server")]
            public void StatusServer()
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            [OperationContract]
            [WebGet(UriTemplate = "status/nocontent")]
            public void StatusNoContent()
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
            }

            [OperationContract]
            [WebGet(UriTemplate = "status/notmodified")]
            public void StatusNotModified()
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotModified;
            }

            [OperationContract]
            [WebGet(UriTemplate = "nothing")]
            public Stream GetNothing()
            {
                WebOperationContext context = WebOperationContext.Current;
                
                return CreateTextResponse(context, string.Empty);
            }

            [OperationContract]
            [WebInvoke(UriTemplate = "allow", Method = "OPTIONS")]
            public void Allow()
            {
                WebOperationContext.Current.OutgoingResponse.Headers[HttpResponseHeader.Allow] = "GET, HEAD, PUT";
            }

            [OperationContract]
            [WebInvoke(UriTemplate = "head", Method = "HEAD")]
            public void Head()
            {
                WebOperationContext.Current.OutgoingResponse.Headers["MyHeader"] = "MyValue";
            }

            [OperationContract]
            [WebGet(UriTemplate = "user/{id}")]
            public Stream GetUser(string id)
            {
                WebOperationContext context = WebOperationContext.Current;

                if (!users.ContainsKey(id))
                {
                    context.OutgoingResponse.SetStatusAsNotFound(String.Format("User with id '{0}' not found", id));
                    return null;
                }

                return CreateTextResponse(context, users[id]);
            }

            [OperationContract]
            [WebGet(UriTemplate = "users")]
            public Stream GetUsersCount()
            {
                WebOperationContext context = WebOperationContext.Current;

                return CreateTextResponse(context, users.Count.ToString());
            }

            [OperationContract]
            [WebInvoke(UriTemplate = "user", Method = "POST")]
            public Stream Post(Stream stream)
            {
                WebOperationContext context = WebOperationContext.Current;

                UriTemplateMatch match = context.IncomingRequest.UriTemplateMatch;
                UriTemplate template = new UriTemplate("/user/{id}");

                MediaType mediaType = MediaType.Parse(context.IncomingRequest.ContentType);
                Encoding encoding = (mediaType == null) ? Encoding.UTF8 : mediaType.CharSet;

                string id = (users.Count + 1).ToString(); // generate new ID
                string name;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    name = reader.ReadToEnd();
                }

                if (String.IsNullOrEmpty(name))
                {
                    context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    context.OutgoingResponse.StatusDescription = "Content cannot be null or empty";
                    return CreateTextResponse(context, "");
                }

                users.Add(id, name);

                Uri uri = template.BindByPosition(match.BaseUri, id);
                context.OutgoingResponse.SetStatusAsCreated(uri);
                context.OutgoingResponse.StatusDescription = String.Format("User id '{0}' created with '{1}'", id, name);

                return CreateTextResponse(context, id);
            }

            [OperationContract]
            [WebInvoke(UriTemplate = "user/{id}", Method = "PUT")]
            public void Update(string id, Stream stream)
            {
                WebOperationContext context = WebOperationContext.Current;

                if (!users.ContainsKey(id))
                {
                    context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    context.OutgoingResponse.StatusDescription = String.Format("User id '{0}' does not exist", id);
                    return;
                }

                MediaType mediaType = MediaType.Parse(context.IncomingRequest.ContentType);

                string name;
                using (StreamReader reader = new StreamReader(stream, mediaType.CharSet))
                {
                    name = reader.ReadToEnd();
                }
                users[id] = name;

                context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                context.OutgoingResponse.StatusDescription = String.Format("User id '{0}' updated with '{1}'", id, name);
            }

            [OperationContract]
            [WebInvoke(UriTemplate = "user/{id}", Method = "DELETE")]
            public void Delete(string id)
            {
                WebOperationContext context = WebOperationContext.Current;

                if (!users.ContainsKey(id))
                {
                    context.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    context.OutgoingResponse.StatusDescription = String.Format("User id '{0}' does not exist", id);
                    return;
                }

                users.Remove(id);

                context.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                context.OutgoingResponse.StatusDescription = String.Format("User id '{0}' have been removed", id);
            }

            [OperationContract]
            [WebGet(UriTemplate = "sleep/{seconds}")]
            public void Sleep(string seconds)
            {
                Thread.Sleep(TimeSpan.FromSeconds(Int32.Parse(seconds)));

                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = "Status OK";
            }

            private Stream CreateTextResponse(WebOperationContext context, string text)
            {
                context.OutgoingResponse.ContentType = "text/plain; charset=utf-8";
                return new MemoryStream(Encoding.UTF8.GetBytes(text));
            }
        }

        #endregion
    }
}
#endif