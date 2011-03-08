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
        private IClientHttpRequest delegateRequest;
        private IEnumerable<IClientHttpRequestInterceptor> interceptors;

        private Action<Stream> body;

        /// <summary>
        /// Creates a new instance of the <see cref="InterceptingClientHttpRequest"/> with the given parameters.
        /// </summary>
        /// <param name="request">The request to wrap.</param>
        /// <param name="interceptors">The interceptors that are to be applied. Can be <c>null</c>.</param>
        public InterceptingClientHttpRequest(
            IClientHttpRequest request,
            IEnumerable<IClientHttpRequestInterceptor> interceptors) 
        {
            AssertUtils.ArgumentNotNull(request, "'request' must not be null");

            this.delegateRequest = request;
            this.interceptors = interceptors != null ? interceptors : new IClientHttpRequestInterceptor[0];
        }

        #region IClientHttpRequest Membres

        /// <summary>
        /// Gets the HTTP method of the request.
        /// </summary>
        public HttpMethod Method
        {
            get { return this.delegateRequest.Method; }
        }

        /// <summary>
        /// Gets the URI of the request.
        /// </summary>
        public Uri Uri
        {
            get { return this.delegateRequest.Uri; }
        }

        /// <summary>
        /// Gets the message headers.
        /// </summary>
        public HttpHeaders Headers
        {
            get { return this.delegateRequest.Headers; }
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
                this.delegateRequest.Body = value;
            }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Execute this request, resulting in a <see cref="IClientHttpResponse" /> that can be read.
        /// </summary>
        /// <returns>The response result of the execution</returns>
        public IClientHttpResponse Execute()
        {
            RequestSyncExecution requestSyncExecution = new RequestSyncExecution(this.delegateRequest, this.body, this.interceptors);
            return requestSyncExecution.Execute();
        }
#endif

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
            RequestAsyncExecution requestAsyncExecution = new RequestAsyncExecution(this.delegateRequest, this.body, this.interceptors, state, executeCompleted);
            requestAsyncExecution.Execute();
        }

        /// <summary>
        /// Cancels a pending asynchronous operation.
        /// </summary>
        public void CancelAsync()
        {
            this.delegateRequest.CancelAsync();
        }

        #endregion

        #region IClientHttpRequestExecution implementations

        private abstract class AbstractRequestExecution : IClientHttpRequestExecution
        {
            protected IClientHttpRequest delegateRequest;
            protected Action<Stream> body;
            protected IEnumerator<IClientHttpRequestInterceptor> enumerator;
            protected bool isAsync;

            protected AbstractRequestExecution(
                IClientHttpRequest delegateRequest,
                Action<Stream> body,
                IEnumerable<IClientHttpRequestInterceptor> interceptors, 
                bool isAsync)
            {
                this.delegateRequest = delegateRequest;
                this.body = body;
                this.enumerator = interceptors.GetEnumerator();
                this.isAsync = isAsync;
            }

            bool IClientHttpRequestExecution.IsAsync
            {
                get { return this.isAsync; }
            }

            HttpMethod IClientHttpRequestExecution.Method
            {
                get { return this.delegateRequest.Method; }
            }

            Uri IClientHttpRequestExecution.Uri
            {
                get { return this.delegateRequest.Uri; }
            }

            HttpHeaders IClientHttpRequestExecution.Headers
            {
                get { return this.delegateRequest.Headers; }
            }

            Action<Stream> IClientHttpRequestExecution.Body
            {
                get { return this.body; }
                set { this.delegateRequest.Body = value; }
            }

            void IClientHttpRequestExecution.Execute()
            {
                this.DoExecute(null);
            }

            void IClientHttpRequestExecution.Execute(Action<IClientHttpResponse> requestExecuted)
            {
                this.DoExecute(requestExecuted);
            }

            protected abstract void DoExecute(Action<IClientHttpResponse> requestExecuted);
        }

#if !SILVERLIGHT
        private sealed class RequestSyncExecution : AbstractRequestExecution
        {
            private IClientHttpResponse response;

            public RequestSyncExecution(
                IClientHttpRequest delegateRequest,
                Action<Stream> body,
                IEnumerable<IClientHttpRequestInterceptor> interceptors)
                : base(delegateRequest, body, interceptors, false)
            {
            }

            public IClientHttpResponse Execute()
            {
                this.DoExecute(null);
                return response;
            }

            protected override void DoExecute(Action<IClientHttpResponse> requestExecuted)
            {
                if (enumerator.MoveNext())
                {
                    enumerator.Current.Execute(this);
                }
                else
                {
                    response = this.delegateRequest.Execute();
                }
                if (requestExecuted != null)
                {
                    requestExecuted(response);
                }
            }
        }
#endif

        private sealed class RequestAsyncExecution : AbstractRequestExecution
        {
            private object asyncState;
            private Action<ClientHttpRequestCompletedEventArgs> executeCompleted;

            private IList<Action<IClientHttpResponse>> responseActions;

            public RequestAsyncExecution(
                IClientHttpRequest delegateRequest,
                Action<Stream> body,
                IEnumerable<IClientHttpRequestInterceptor> interceptors,
                object ayncState,
                Action<ClientHttpRequestCompletedEventArgs> executeCompleted)
                : base(delegateRequest, body, interceptors, true)
            {
                this.asyncState = ayncState;
                this.executeCompleted = executeCompleted;
                responseActions = new List<Action<IClientHttpResponse>>();
            }

            public void Execute()
            {
                this.DoExecute(null);
            }

            protected override void DoExecute(Action<IClientHttpResponse> requestExecuted)
            {
                if (requestExecuted != null)
                {
                    this.responseActions.Insert(0, requestExecuted);
                }
                if (enumerator.MoveNext())
                {
                    enumerator.Current.Execute(this);
                }
                else
                {
                    this.delegateRequest.ExecuteAsync(this.asyncState, 
                        delegate(ClientHttpRequestCompletedEventArgs args)
                        {
                            foreach (Action<IClientHttpResponse> action in this.responseActions)
                            {
                                action(args.Response);
                            }
                            this.executeCompleted(args);
                        });
                }
            }
        }

        #endregion
    }
}
