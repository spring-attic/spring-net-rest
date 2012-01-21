#if !CF_3_5
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

using Spring.Http;

namespace Spring.Rest.Client.Support
{
    /// <summary>
    /// Represents the state of an asynchronous REST operation 
    /// that is passed to the request asynchronous execution method.
    /// </summary>
    /// <typeparam name="T">The type of the response value.</typeparam>
    /// <author>Bruno Baia</author>
    public class RestAsyncOperationState<T> where T : class
    {
        private Uri _uri;
        private HttpMethod _method;
        private IResponseExtractor<T> _responseExtractor;
        private IResponseErrorHandler _responseErrorHandler;
        private Action<RestOperationCompletedEventArgs<T>> _methodCompleted;

        /// <summary>
        /// Gets or sets the HTTP URI.
        /// </summary>
        public Uri Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }        

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        public HttpMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IResponseExtractor{T}"/> 
        /// that extracts the return value from the response.
        /// </summary>
        public IResponseExtractor<T> ResponseExtractor
        {
            get { return _responseExtractor; }
            set { _responseExtractor = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IResponseErrorHandler"/> 
        /// that handles error in the response.
        /// </summary>
        public IResponseErrorHandler ResponseErrorHandler
        {
            get { return _responseErrorHandler; }
            set { _responseErrorHandler = value; }
        }

        /// <summary>
        /// Gets or sets the <code>Action&lt;RestOperationCompletedEventArgs&lt;T&gt;&gt;</code> 
        /// to perform when the REST operation completes.
        /// </summary>
        public Action<RestOperationCompletedEventArgs<T>> MethodCompleted
        {
            get { return _methodCompleted; }
            set { _methodCompleted = value; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="RestAsyncOperationState{T}"/>.
        /// </summary>
        /// <param name="uri">The HTTP URI.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="responseExtractor">
        /// The object that extracts the return value from the response.
        /// </param>
        /// <param name="responseErrorHandler">
        /// The object that handles error in the response.
        /// </param>
        /// <param name="methodCompleted">
        /// The callback method when the REST operation completes.
        /// </param>
        public RestAsyncOperationState(Uri uri, HttpMethod method,
            IResponseExtractor<T> responseExtractor,
            IResponseErrorHandler responseErrorHandler,
            Action<RestOperationCompletedEventArgs<T>> methodCompleted)
        {
            this._uri = uri;
            this._method = method;
            this._responseExtractor = responseExtractor;
            this._responseErrorHandler = responseErrorHandler;
            this._methodCompleted = methodCompleted;
        }
    }
}
#endif