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
using System.Collections.Generic;

using Spring.Util;

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// Wrapper for an <see cref="IClientHttpRequestFactory"/> that has support 
    /// for <see cref="IClientHttpRequestInterceptor"/>s.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia</author>
    public class InterceptingClientHttpRequestFactory : IClientHttpRequestFactory
    {
        private IClientHttpRequestFactory targetRequestFactory;
        private IEnumerable<IClientHttpRequestInterceptor> interceptors;

        /// <summary>
        /// Gets the intercepted request factory.
        /// </summary>
        public IClientHttpRequestFactory TargetRequestFactory
        {
            get { return this.targetRequestFactory; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="InterceptingClientHttpRequestFactory"/> with the given parameters.
        /// </summary>
        /// <param name="requestFactory">The request factory to wrap.</param>
        /// <param name="interceptors">The interceptors that are to be applied. Can be <c>null</c>.</param>
        public InterceptingClientHttpRequestFactory(
            IClientHttpRequestFactory requestFactory,
            IEnumerable<IClientHttpRequestInterceptor> interceptors)
        {
            ArgumentUtils.AssertNotNull(requestFactory, "requestFactory");

            this.targetRequestFactory = requestFactory;
            this.interceptors = interceptors != null ? interceptors : new IClientHttpRequestInterceptor[0];
        }

        #region IClientHttpRequestFactory Members

        /// <summary>
        /// Create a new <see cref="IClientHttpRequest"/> for the specified URI and HTTP method.
        /// </summary>
        /// <param name="uri">The URI to create a request for.</param>
        /// <param name="method">The HTTP method to execute.</param>
        /// <returns>The created request</returns>
        public IClientHttpRequest CreateRequest(Uri uri, HttpMethod method)
        {
            RequestCreation requestCreation = new RequestCreation(uri, method, this.targetRequestFactory, this.interceptors);
            IClientHttpRequest request = requestCreation.Create();
            return new InterceptingClientHttpRequest(request, this.interceptors);
        }

        #endregion

        #region IClientHttpRequestFactoryCreation implementation

        private sealed class RequestCreation : IClientHttpRequestFactoryCreation
        {
            private Uri uri;
            private HttpMethod method;
            private IClientHttpRequestFactory delegateRequestFactory;
            private IEnumerator<IClientHttpRequestInterceptor> enumerator;

            public RequestCreation(Uri uri, HttpMethod method,
                IClientHttpRequestFactory delegateRequestFactory,
                IEnumerable<IClientHttpRequestInterceptor> interceptors)
            {
                this.uri = uri;
                this.method = method;
                this.delegateRequestFactory = delegateRequestFactory;
                this.enumerator = interceptors.GetEnumerator();
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

            public IClientHttpRequest Create()
            {
                if (enumerator.MoveNext())
                {
                    if (enumerator.Current is IClientHttpRequestFactoryInterceptor)
                    {
                        return ((IClientHttpRequestFactoryInterceptor)enumerator.Current).Create(this);
                    }
                    return this.Create();
                }
                else
                {
                    return this.delegateRequestFactory.CreateRequest(this.uri, this.method);
                }
            }
        }

        #endregion
    }
}
