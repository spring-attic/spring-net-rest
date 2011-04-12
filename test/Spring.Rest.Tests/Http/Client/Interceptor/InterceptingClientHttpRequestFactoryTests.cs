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
using System.Threading;

using NUnit.Framework;

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// Integration tests for InterceptingClientHttpRequestFactory implementations.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia</author>    
    [TestFixture]
    public class InterceptingClientHttpRequestFactoryTests
    {
        private InterceptingClientHttpRequestFactory requestFactory;

        private RequestFactoryMock requestFactoryMock;
        private RequestMock requestMock;
        private ResponseMock responseMock;

        [SetUp]
        public void SetUp()
        {
            responseMock = new ResponseMock();
            requestMock = new RequestMock(responseMock);
            requestFactoryMock = new RequestFactoryMock(requestMock);
        }

        [Test]
        public void Creation()
        {
            NoOpRequestFactoryInterceptor interceptor1 = new NoOpRequestFactoryInterceptor();
            NoOpRequestFactoryInterceptor interceptor2 = new NoOpRequestFactoryInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);

            Assert.IsTrue(interceptor1.invoked);
            Assert.IsTrue(interceptor2.invoked);
            Assert.IsTrue(this.requestFactoryMock.created);
            Assert.IsFalse(this.requestMock.executed);

            IClientHttpResponse response = request.Execute();
            Assert.IsTrue(this.requestMock.executed);
            Assert.AreSame(this.responseMock, response);
        }

        [Test]
        public void BeforeExecution()
        {
            NoOpRequestBeforeInterceptor interceptor1 = new NoOpRequestBeforeInterceptor();
            NoOpRequestBeforeInterceptor interceptor2 = new NoOpRequestBeforeInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);

            Assert.IsFalse(interceptor1.invoked);
            Assert.IsFalse(interceptor2.invoked);
            Assert.IsTrue(this.requestFactoryMock.created);
            Assert.IsFalse(this.requestMock.executed);

            IClientHttpResponse response = request.Execute();

            Assert.IsTrue(interceptor1.invoked);
            Assert.IsTrue(interceptor2.invoked);
            Assert.IsTrue(this.requestMock.executed);
            Assert.AreSame(this.responseMock, response);
        }

        [Test]
        public void SyncExecution()
        {
            NoOpRequestSyncInterceptor interceptor1 = new NoOpRequestSyncInterceptor();
            NoOpRequestSyncInterceptor interceptor2 = new NoOpRequestSyncInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock, 
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);

            Assert.IsFalse(interceptor1.invoked);
            Assert.IsFalse(interceptor2.invoked);
            Assert.IsTrue(this.requestFactoryMock.created);
            Assert.IsFalse(this.requestMock.executed);

            IClientHttpResponse response = request.Execute();

            Assert.IsTrue(interceptor1.invoked);
            Assert.IsTrue(interceptor2.invoked);
            Assert.IsTrue(this.requestMock.executed);
            Assert.AreSame(this.responseMock, response);
        }

        [Test]
        public void AsyncExecution()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            NoOpRequestAsyncInterceptor interceptor1 = new NoOpRequestAsyncInterceptor();
            ChangeHeaderInterceptor interceptor2 = new ChangeHeaderInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            request.ExecuteAsync(null, delegate(ClientHttpRequestCompletedEventArgs args)
            {
                try
                {
                    Assert.IsNull(args.Error, "Invalid response");
                    Assert.IsFalse(args.Cancelled, "Invalid response");
                    Assert.AreSame(this.responseMock, args.Response, "Invalid response");
                    Assert.IsNotNull(args.Response.Headers.Get("AfterAsyncExecution"));
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

            Assert.IsTrue(interceptor1.invoked);
            Assert.IsTrue(this.requestMock.executed);
            Assert.IsNotNull(responseMock.Headers.Get("AfterAsyncExecution"));
        }

        [Test]
        public void NoSyncExecution()
        {
            NoOpExecutionInterceptor interceptor1 = new NoOpExecutionInterceptor();
            NoOpRequestSyncInterceptor interceptor2 = new NoOpRequestSyncInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            IClientHttpResponse response = request.Execute();

            Assert.IsFalse(interceptor2.invoked);
            Assert.IsFalse(requestMock.executed);
        }

        [Test]
        public void NoAsyncExecution()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            NoOpExecutionInterceptor interceptor1 = new NoOpExecutionInterceptor();
            NoOpRequestAsyncInterceptor interceptor2 = new NoOpRequestAsyncInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            request.ExecuteAsync(null, delegate(ClientHttpRequestCompletedEventArgs args)
            {
                try
                {
                    Assert.Fail("No Execution");
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

            //manualEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }

            Assert.IsFalse(interceptor2.invoked);
            Assert.IsFalse(requestMock.executed);
        }

        [Test]
        public void ChangeHeaders()
        {
            ChangeHeaderInterceptor interceptor = new ChangeHeaderInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);

            Assert.IsNotNull(requestMock.Headers.Get("AfterCreation"));
            Assert.IsNull(requestMock.Headers.Get("BeforeSyncExecution"));
            Assert.IsNull(responseMock.Headers.Get("AfterSyncExecution"));

            request.Execute();

            Assert.IsNotNull(requestMock.Headers.Get("BeforeSyncExecution"));
            Assert.IsNotNull(responseMock.Headers.Get("AfterSyncExecution"));
        }

        [Test]
        public void ChangeUri()
        {
            ChangeUriRequestFactoryInterceptor interceptor = new ChangeUriRequestFactoryInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            Assert.AreEqual(new Uri("http://example.com/2"), requestMock.Uri);
        }

        [Test]
        public void ChangeMethod()
        {
            ChangeMethodRequestFactoryInterceptor interceptor = new ChangeMethodRequestFactoryInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            Assert.AreEqual(HttpMethod.POST, requestMock.Method);
        }

        [Test]
        public void ChangeBody()
        {
            ChangeBodyInterceptor interceptor = new ChangeBodyInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            request.Execute();

            MemoryStream stream = new MemoryStream();
            requestMock.Body(stream);
            stream.Position = 0;
            string result = null;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            Assert.AreEqual("New body", result);
        }

        [Test]
        public void ChangeResponse()
        {
            ChangeResponseInterceptor interceptor = new ChangeResponseInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            IClientHttpResponse response = request.Execute();

            Assert.AreNotSame(this.responseMock, response);
        }

        [Test]
        public void ChangeResponseAsync()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            ChangeResponseInterceptor interceptor = new ChangeResponseInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            request.ExecuteAsync(null, delegate(ClientHttpRequestCompletedEventArgs args)
            {
                try
                {
                    Assert.IsNull(args.Error, "Invalid response");
                    Assert.IsFalse(args.Cancelled, "Invalid response");
                    Assert.AreNotSame(this.responseMock, args.Response, "Invalid response");
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

        #region Inner classes

        private sealed class NoOpRequestFactoryInterceptor : IClientHttpRequestFactoryInterceptor
        {
            public bool invoked;

            public IClientHttpRequest Create(IClientHttpRequestFactoryCreation creation)
            {
                this.invoked = true;
                return creation.Create();
            }
        }
        private sealed class NoOpRequestBeforeInterceptor : IClientHttpRequestBeforeInterceptor
        {
            public bool invoked;

            public void BeforeExecute(IClientHttpRequestContext request)
            {
                this.invoked = true;
            }
        }

        private sealed class NoOpRequestSyncInterceptor : IClientHttpRequestSyncInterceptor
        {
            public bool invoked;

            public IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution)
            {
                this.invoked = true;
                return execution.Execute();
            }
        }

        private sealed class NoOpRequestAsyncInterceptor : IClientHttpRequestAsyncInterceptor
        {
            public bool invoked;

            public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
            {
                this.invoked = true;
                execution.ExecuteAsync();
            }
        }

        private sealed class NoOpExecutionInterceptor : 
            IClientHttpRequestSyncInterceptor,
            IClientHttpRequestAsyncInterceptor
        {
            public IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution)
            {
                return null; //execution.Execute();
            }

            public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
            {
                //execution.ExecuteAsync();
            }
        }

        private sealed class ChangeHeaderInterceptor : 
            IClientHttpRequestFactoryInterceptor,
            IClientHttpRequestSyncInterceptor, 
            IClientHttpRequestAsyncInterceptor
        {
            // IClientHttpRequestFactoryInterceptor
            public IClientHttpRequest Create(IClientHttpRequestFactoryCreation creation)
            {
                IClientHttpRequest request = creation.Create();
                request.Headers.Add("AfterCreation", "MyValue");
                return request;
            }

            // IClientHttpRequestSyncInterceptor
            public IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution)
            {
                execution.Headers.Add("BeforeSyncExecution", "MyValue");
                IClientHttpResponse response = execution.Execute();
                response.Headers.Add("AfterSyncExecution", "MyValue");
                return response;
            }

            // IClientHttpRequestAsyncInterceptor
            public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
            {
                execution.Headers.Add("BeforeAsyncExecution", "MyValue");
                execution.ExecuteAsync(
                    delegate(IClientHttpResponseAsyncContext ctx)
                    {
                        ctx.Response.Headers.Add("AfterAsyncExecution", "MyValue");
                    });
            }
        }

        private sealed class ChangeUriRequestFactoryInterceptor : IClientHttpRequestFactoryInterceptor
        {
            public IClientHttpRequest Create(IClientHttpRequestFactoryCreation creation)
            {
                creation.Uri = new Uri("http://example.com/2");
                return creation.Create();
            }
        }

        private sealed class ChangeMethodRequestFactoryInterceptor : IClientHttpRequestFactoryInterceptor
        {
            public IClientHttpRequest Create(IClientHttpRequestFactoryCreation creation)
            {
                creation.Method = HttpMethod.POST;
                return creation.Create();
            }
        }

        private sealed class ChangeBodyInterceptor : 
            IClientHttpRequestSyncInterceptor, 
            IClientHttpRequestAsyncInterceptor
        {
            public IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution)
            {
                byte[] bytes = Encoding.UTF8.GetBytes("New body");
                execution.Body = delegate(Stream stream)
                {
                    stream.Write(bytes, 0, bytes.Length);
                };
                return execution.Execute();
            }

            public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
            {
                byte[] bytes = Encoding.UTF8.GetBytes("New body");
                execution.Body = delegate(Stream stream)
                {
                    stream.Write(bytes, 0, bytes.Length);
                };
                execution.ExecuteAsync();
            }
        }

        private sealed class ChangeResponseInterceptor : 
            IClientHttpRequestSyncInterceptor, 
            IClientHttpRequestAsyncInterceptor 
        {
            public IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution)
            {
                execution.Execute();
                return new ResponseMock();
            }

            public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
            {
                execution.ExecuteAsync(
                    delegate(IClientHttpResponseAsyncContext ctx)
                    {
                        ctx.Response = new ResponseMock();
                    });
            }
        }

        private sealed class RequestFactoryMock : IClientHttpRequestFactory
        {
            private RequestMock requestMock;
            
            public bool created = false;

            public RequestFactoryMock(RequestMock requestMock)
            {
                this.requestMock = requestMock;
            }

            public IClientHttpRequest CreateRequest(Uri uri, HttpMethod method)
            {
                this.created = true;
                this.requestMock.Uri = uri;
                this.requestMock.Method = method;
                return requestMock;
            }
        }

        private sealed class RequestMock : IClientHttpRequest
        {
            private Uri uri;
            private HttpMethod method;
            private HttpHeaders headers = new HttpHeaders();
            private Action<Stream> body;

            private ResponseMock responseMock;

            public bool executed = false;

            public RequestMock(ResponseMock responseMock)
            {
                this.responseMock = responseMock;
            }

            public HttpMethod Method
            {
                get { return this.method; }
                set { this.method = value; }
            }

            public Uri Uri
            {
                get { return this.uri; }
                set { this.uri = value; }
            }

            public HttpHeaders Headers
            {
                get { return this.headers; }
            }

            public Action<Stream> Body
            {
                get { return this.body; }
                set { this.body = value; }
            }

            public IClientHttpResponse Execute()
            {
                this.executed = true;
                return this.responseMock;
            }

            public void ExecuteAsync(object state, Action<ClientHttpRequestCompletedEventArgs> executeCompleted)
            {
                this.executed = true;
                executeCompleted(new ClientHttpRequestCompletedEventArgs(this.responseMock, null, false, null));
            }

            public void CancelAsync()
            {
            }
        }

        private sealed class ResponseMock : IClientHttpResponse
        {
            private HttpHeaders headers = new HttpHeaders();

            public HttpStatusCode StatusCode
            {
                get { return HttpStatusCode.OK; }
            }

            public string StatusDescription
            {
                get { return String.Empty; }
            }

            public HttpHeaders Headers
            {
                get { return this.headers; }
            }

            public Stream Body
            {
                get { return null; }
            }

            public void Close()
            {
            }

            public void Dispose()
            {
            }
        }

        #endregion
    }
}
