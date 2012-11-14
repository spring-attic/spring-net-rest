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
using System.IO;
using System.Collections.Generic;

using Spring.Util;

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// Wrapper for an <see cref="IClientHttpRequest"/> that has support 
    /// for <see cref="IClientHttpRequestInterceptor"/>s.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia</author>
    public class InterceptingClientHttpRequest : IClientHttpRequest
    {
        private IClientHttpRequest targetRequest;
        private IEnumerable<IClientHttpRequestInterceptor> interceptors;

        private Action<Stream> body;

        /// <summary>
        /// Gets the intercepted request.
        /// </summary>
        public IClientHttpRequest TargetRequest
        {
            get { return this.targetRequest; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="InterceptingClientHttpRequest"/> with the given parameters.
        /// </summary>
        /// <param name="request">The request to wrap.</param>
        /// <param name="interceptors">The interceptors that are to be applied. Can be <c>null</c>.</param>
        public InterceptingClientHttpRequest(
            IClientHttpRequest request,
            IEnumerable<IClientHttpRequestInterceptor> interceptors) 
        {
            ArgumentUtils.AssertNotNull(request, "request");

            this.targetRequest = request;
            this.interceptors = interceptors != null ? interceptors : new IClientHttpRequestInterceptor[0];
        }

        #region IClientHttpRequest Members

        /// <summary>
        /// Gets the HTTP method of the request.
        /// </summary>
        public HttpMethod Method
        {
            get { return this.targetRequest.Method; }
        }

        /// <summary>
        /// Gets the URI of the request.
        /// </summary>
        public Uri Uri
        {
            get { return this.targetRequest.Uri; }
        }

        /// <summary>
        /// Gets the message headers.
        /// </summary>
        public HttpHeaders Headers
        {
            get { return this.targetRequest.Headers; }
        }

        /// <summary>
        /// Gets or sets the delegate that writes the body message as a stream.
        /// </summary>
        public Action<Stream> Body
        {
            get { return this.body; }
            set
            {
                this.body = value;
                this.targetRequest.Body = value;
            }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Execute this request, resulting in a <see cref="IClientHttpResponse" /> that can be read.
        /// </summary>
        /// <returns>The response result of the execution</returns>
        public IClientHttpResponse Execute()
        {
            RequestSyncExecution requestSyncExecution = new RequestSyncExecution(this.targetRequest, this.body, this.interceptors);
            return requestSyncExecution.Execute();
        }
#endif

#if !CF_3_5
        /// <summary>
        /// Execute this request asynchronously.
        /// </summary>
        /// <param name="state">
        /// An optional user-defined object that is passed to the method invoked 
        /// when the asynchronous operation completes.
        /// </param>
        /// <param name="executeCompleted">
        /// The <see cref="Action{ClientHttpRequestCompletedEventArgs}"/> to perform when the asynchronous execution completes.
        /// </param>
        public void ExecuteAsync(object state, Action<ClientHttpRequestCompletedEventArgs> executeCompleted)
        {
            RequestAsyncExecution requestAsyncExecution = new RequestAsyncExecution(this.targetRequest, this.body, this.interceptors, state, executeCompleted);
            requestAsyncExecution.ExecuteAsync();
        }

        /// <summary>
        /// Cancels a pending asynchronous operation.
        /// </summary>
        public void CancelAsync()
        {
            this.targetRequest.CancelAsync();
        }
#endif

        #endregion

        #region Inner class definitions

        private abstract class AbstractRequestContext  : IClientHttpRequestContext
        {
            protected IClientHttpRequest delegateRequest;
            protected Action<Stream> body;
            protected IEnumerator<IClientHttpRequestInterceptor> enumerator;

            protected AbstractRequestContext(
                IClientHttpRequest delegateRequest,
                Action<Stream> body,
                IEnumerable<IClientHttpRequestInterceptor> interceptors)
            {
                this.delegateRequest = delegateRequest;
                this.body = body;
                this.enumerator = interceptors.GetEnumerator();
            }

            public HttpMethod Method
            {
                get { return this.delegateRequest.Method; }
            }

            public Uri Uri
            {
                get { return this.delegateRequest.Uri; }
            }

            public HttpHeaders Headers
            {
                get { return this.delegateRequest.Headers; }
            }

            public Action<Stream> Body
            {
                get { return this.body; }
                set { this.delegateRequest.Body = value; }
            }
        }

#if !SILVERLIGHT
        private sealed class RequestSyncExecution : AbstractRequestContext, IClientHttpRequestSyncExecution
        {
            public RequestSyncExecution(
                IClientHttpRequest delegateRequest,
                Action<Stream> body,
                IEnumerable<IClientHttpRequestInterceptor> interceptors)
                : base(delegateRequest, body, interceptors)
            {
            }

            public IClientHttpResponse Execute()
            {
                if (enumerator.MoveNext())
                {
                    if (enumerator.Current is IClientHttpRequestSyncInterceptor)
                    {
                        return ((IClientHttpRequestSyncInterceptor)enumerator.Current).Execute(this);
                    }
                    if (enumerator.Current is IClientHttpRequestBeforeInterceptor)
                    {
                        ((IClientHttpRequestBeforeInterceptor)enumerator.Current).BeforeExecute(this);
                    }                    
                    return this.Execute();
                }
                else
                {
                    return this.delegateRequest.Execute();
                }
            }
        }
#endif

#if !CF_3_5
        private sealed class RequestAsyncExecution : AbstractRequestContext, IClientHttpRequestAsyncExecution
        {
            private object asyncState;
            private Action<ClientHttpRequestCompletedEventArgs> interceptedExecuteCompleted;

            private IList<Action<IClientHttpResponseAsyncContext>> executeCompletedDelegates;

            public RequestAsyncExecution(
                IClientHttpRequest delegateRequest,
                Action<Stream> body,
                IEnumerable<IClientHttpRequestInterceptor> interceptors, 
                object asyncState,
                Action<ClientHttpRequestCompletedEventArgs> executeCompleted)
                : base(delegateRequest, body, interceptors)
            {
                this.asyncState = asyncState;
                this.interceptedExecuteCompleted = executeCompleted;
                this.executeCompletedDelegates = new List<Action<IClientHttpResponseAsyncContext>>();
            }

            public object AsyncState
            {
                get { return this.asyncState; }
                set { this.asyncState = value; }
            }

            public void ExecuteAsync()
            {
                this.ExecuteAsync(null);
            }

            public void ExecuteAsync(Action<IClientHttpResponseAsyncContext> executeCompleted)
            {
                if (executeCompleted != null)
                {
                    this.executeCompletedDelegates.Insert(0, executeCompleted);
                }
                if (enumerator.MoveNext())
                {
                    if (enumerator.Current is IClientHttpRequestAsyncInterceptor)
                    {
                        ((IClientHttpRequestAsyncInterceptor)enumerator.Current).ExecuteAsync(this);
                    }
                    else
                    {
                        if (enumerator.Current is IClientHttpRequestBeforeInterceptor)
                        {
                            ((IClientHttpRequestBeforeInterceptor)enumerator.Current).BeforeExecute(this);
                        }
                        this.ExecuteAsync(null);
                    }
                }
                else
                {
                    this.delegateRequest.ExecuteAsync(this.asyncState,
                        delegate(ClientHttpRequestCompletedEventArgs args)
                        {
                            ResponseAsyncContext responseAsyncContext = new ResponseAsyncContext(args);
                            foreach (Action<IClientHttpResponseAsyncContext> action in this.executeCompletedDelegates)
                            {
                                action(responseAsyncContext);
                            }

                            if (this.interceptedExecuteCompleted != null)
                            {
                                interceptedExecuteCompleted(responseAsyncContext.GetCompletedEventArgs());
                            }
                        });
                }
            }
        }

        private sealed class ResponseAsyncContext : IClientHttpResponseAsyncContext
        {
            private bool hasChanged;
            private bool cancelled;
            private Exception error;
            private IClientHttpResponse response;
            private object userState;

            private ClientHttpRequestCompletedEventArgs completedEventArgs;

            public bool Cancelled
            {
                get { return this.cancelled; }
                set
                {
                    this.hasChanged = true;
                    this.cancelled = value;
                }
            }

            public Exception Error
            {
                get { return this.error; }
                set
                {
                    this.hasChanged = true;
                    this.error = value;
                }
            }

            public IClientHttpResponse Response
            {
                get { return this.response; }
                set
                {
                    this.hasChanged = true;
                    this.response = value;
                }
            }

            public object UserState
            {
                get { return this.userState; }
                set
                {
                    this.hasChanged = true;
                    this.userState = value;
                }
            }

            public ResponseAsyncContext(ClientHttpRequestCompletedEventArgs completedEventArgs)
            {
                this.cancelled = completedEventArgs.Cancelled;
                this.error = completedEventArgs.Error;
                if (this.error == null)
                {
                    this.response = completedEventArgs.Response;
                }
                this.userState = completedEventArgs.UserState;

                this.completedEventArgs = completedEventArgs;
            }

            public ClientHttpRequestCompletedEventArgs GetCompletedEventArgs()
            {
                if (!this.hasChanged)
                {
                    return this.completedEventArgs;
                }
                else
                {
                    return new ClientHttpRequestCompletedEventArgs(this.response, this.error, this.cancelled, this.userState);
                }
            }
        }
#endif

        #endregion
    }
}
