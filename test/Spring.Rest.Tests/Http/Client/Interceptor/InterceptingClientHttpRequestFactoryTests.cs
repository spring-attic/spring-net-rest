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
        public void CreationAndSyncExecution()
        {
            NoOpInterceptor interceptor1 = new NoOpInterceptor();
            NoOpInterceptor interceptor2 = new NoOpInterceptor();
            NoOpInterceptor interceptor3 = new NoOpInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock, 
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2, interceptor3 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);

            Assert.IsTrue(interceptor1.createInvoked);
            Assert.IsTrue(interceptor2.createInvoked);
            Assert.IsTrue(interceptor3.createInvoked);
            Assert.IsTrue(this.requestFactoryMock.created);
            Assert.IsFalse(interceptor1.executeInvoked);
            Assert.IsFalse(interceptor2.executeInvoked);
            Assert.IsFalse(interceptor3.executeInvoked);
            Assert.IsFalse(this.requestMock.executed);

            IClientHttpResponse response = request.Execute();

            Assert.IsTrue(interceptor1.executeInvoked);
            Assert.IsFalse(interceptor1.isAsync);
            Assert.IsTrue(interceptor2.executeInvoked);
            Assert.IsFalse(interceptor2.isAsync);
            Assert.IsTrue(interceptor3.executeInvoked);
            Assert.IsFalse(interceptor3.isAsync);
            Assert.IsTrue(this.requestMock.executed);

            Assert.AreSame(this.responseMock, response);
        }

        [Test]
        public void AsyncExecution()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Exception exception = null;

            NoOpInterceptor interceptor1 = new NoOpInterceptor();
            NoOpInterceptor interceptor2 = new NoOpInterceptor();
            ChangeHeaderInterceptor interceptor3 = new ChangeHeaderInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock,
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2, interceptor3 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            request.ExecuteAsync(null, delegate(ClientHttpRequestCompletedEventArgs args)
            {
                try
                {
                    Assert.IsNull(args.Error, "Invalid response");
                    Assert.IsFalse(args.Cancelled, "Invalid response");
                    Assert.AreSame(this.responseMock, args.Response, "Invalid response");
                    Assert.IsNotNull(args.Response.Headers.Get("AfterExecution"));
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

            Assert.IsTrue(interceptor1.executeInvoked);
            Assert.IsTrue(interceptor1.isAsync);
            Assert.IsTrue(interceptor2.executeInvoked);
            Assert.IsTrue(interceptor2.isAsync);
            Assert.IsTrue(this.requestMock.executed);
            Assert.IsNotNull(responseMock.Headers.Get("AfterExecution"));
        }

        [Test]
        public void NoExecution()
        {
            NoExecutionInterceptor interceptor1 = new NoExecutionInterceptor();
            NoOpInterceptor interceptor2 = new NoOpInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock, 
                new IClientHttpRequestInterceptor[] { interceptor1, interceptor2 });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            IClientHttpResponse response = request.Execute();

            Assert.IsFalse(interceptor2.executeInvoked);
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
            Assert.IsNull(requestMock.Headers.Get("BeforeExecution"));

            request.Execute();

            Assert.IsNotNull(requestMock.Headers.Get("AfterCreation"));
            Assert.IsNotNull(requestMock.Headers.Get("BeforeExecution"));
            Assert.IsNotNull(responseMock.Headers.Get("AfterExecution"));
        }

        [Test]
        public void ChangeUri()
        {
            ChangeUriInterceptor interceptor = new ChangeUriInterceptor();
            requestFactory = new InterceptingClientHttpRequestFactory(requestFactoryMock, 
                new IClientHttpRequestInterceptor[] { interceptor });

            IClientHttpRequest request = requestFactory.CreateRequest(new Uri("http://example.com"), HttpMethod.GET);
            Assert.AreEqual(new Uri("http://example.com/2"), requestMock.Uri);
        }

        [Test]
        public void ChangeMethod()
        {
            ChangeMethodInterceptor interceptor = new ChangeMethodInterceptor();
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

        #region Inner classes

        private sealed class NoOpInterceptor : IClientHttpRequestInterceptor
        {
            public bool createInvoked;
            public bool executeInvoked;
            public bool isAsync;

            public IClientHttpRequest Create(IClientHttpRequestCreation creation)
            {
                this.createInvoked = true;
                return creation.Create();
            }

            public void Execute(IClientHttpRequestExecution execution)
            {
                this.isAsync = execution.IsAsync;
                this.executeInvoked = true;
                execution.Execute();
            }
        }

        private sealed class NoExecutionInterceptor : IClientHttpRequestInterceptor
        {
            public IClientHttpRequest Create(IClientHttpRequestCreation creation)
            {
                return creation.Create();
            }

            public void Execute(IClientHttpRequestExecution execution)
            {
                //execution.Execute();
            }
        }

        private sealed class ChangeHeaderInterceptor : IClientHttpRequestInterceptor
        {
            public IClientHttpRequest Create(IClientHttpRequestCreation creation)
            {
                IClientHttpRequest request = creation.Create();
                request.Headers.Add("AfterCreation", "MyValue");
                return request;
            }

            public void Execute(IClientHttpRequestExecution execution)
            {
                execution.Headers.Add("BeforeExecution", "MyValue");
                execution.Execute(delegate(IClientHttpResponse response)
                {
                    response.Headers.Add("AfterExecution", "MyValue");
                    return response;
                });
            }
        }

        private sealed class ChangeUriInterceptor : IClientHttpRequestInterceptor
        {
            public IClientHttpRequest Create(IClientHttpRequestCreation creation)
            {
                creation.Uri = new Uri("http://example.com/2");
                return creation.Create();
            }

            public void Execute(IClientHttpRequestExecution execution)
            {
                execution.Execute();
            }
        }

        private sealed class ChangeMethodInterceptor : IClientHttpRequestInterceptor
        {
            public IClientHttpRequest Create(IClientHttpRequestCreation creation)
            {
                creation.Method = HttpMethod.POST;
                return creation.Create();
            }

            public void Execute(IClientHttpRequestExecution execution)
            {
                execution.Execute();
            }
        }

        private sealed class ChangeBodyInterceptor : IClientHttpRequestInterceptor
        {
            public IClientHttpRequest Create(IClientHttpRequestCreation creation)
            {
                return creation.Create();
            }

            public void Execute(IClientHttpRequestExecution execution)
            {
                byte[] bytes = Encoding.UTF8.GetBytes("New body");
                execution.Body = delegate(Stream stream)
                {
                    stream.Write(bytes, 0, bytes.Length);
                };
                execution.Execute();
            }
        }

        private sealed class ChangeResponseInterceptor : IClientHttpRequestInterceptor
        {
            public IClientHttpRequest Create(IClientHttpRequestCreation creation)
            {
                return creation.Create();
            }

            public void Execute(IClientHttpRequestExecution execution)
            {
                execution.Execute(delegate(IClientHttpResponse response)
                {
                    return new ResponseMock();
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
