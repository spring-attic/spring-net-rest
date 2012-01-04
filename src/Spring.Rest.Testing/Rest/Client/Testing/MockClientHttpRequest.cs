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
using System.Text;
using System.Collections.Generic;

using Spring.Http;
using Spring.Http.Client;

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Mock implementation of <see cref="IClientHttpRequest"/>. 
    /// Implements <see cref="IRequestActions"/> to form a fluent API.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Lukas Krecan</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public class MockClientHttpRequest : IClientHttpRequest, IRequestActions
    {
        private IList<RequestMatcher> requestMatchers = new List<RequestMatcher>();
        private ResponseCreator responseCreator;

	    private Uri uri;
        private HttpMethod method;
        private HttpHeaders headers = new HttpHeaders();
        private Action<Stream> body;

        #region IClientHttpRequest Members

        /// <summary>
        /// Gets or sets the HTTP method of the request.
        /// </summary>
        public HttpMethod Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        /// <summary>
        /// Gets or sets the URI of the request.
        /// </summary>
        public Uri Uri
        {
            get { return this.uri; }
            set { this.uri = value; }
        }

        /// <summary>
        /// Gets the message headers.
        /// </summary>
        public HttpHeaders Headers
        {
            get { return this.headers; }
        }

        /// <summary>
        /// Sets the delegate that writes the body message as a stream.
        /// </summary>
        public Action<Stream> Body
        {
            set { this.body = value; }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Execute this request, resulting in a <see cref="IClientHttpResponse" /> that can be read.
        /// </summary>
        /// <returns>The response result of the execution</returns>
        public IClientHttpResponse Execute()
        {
            foreach (RequestMatcher requestMatcher in this.requestMatchers)
            {
                requestMatcher(this);
            }

            if (this.responseCreator != null)
            {
                return responseCreator(this);
            }
            else
            {
                return null;
            }
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
            foreach (RequestMatcher requestMatcher in this.requestMatchers)
            {
                requestMatcher(this);
            }

            IClientHttpResponse response = null;
            if (this.responseCreator != null)
            {
                response = responseCreator(this);
            }
            executeCompleted(new ClientHttpRequestCompletedEventArgs(response, null, false, state));
        }

        /// <summary>
        /// Cancels a pending asynchronous operation.
        /// </summary>
        public void CancelAsync()
        {
        }
#endif

        #endregion

        #region IRequestActions Members

        /// <summary>
        /// Allows for further expectations to be set on the request.
        /// </summary>
        /// <param name="requestMatcher">The request expectations.</param>
        /// <returns>The request expectations.</returns>
        public IRequestActions AndExpect(RequestMatcher requestMatcher)
        {
            // TODO: AssertUtils
            //AssertUtils.ArgumentNotNull(requestMatcher, "'requestMatcher' must not be null");
            this.requestMatchers.Add(requestMatcher);
            return this;
        }

        /// <summary>
        /// Allows for reponse creation on the request.
        /// </summary>
        /// <param name="responseCreator">The response creator.</param>
        public void AndRespond(ResponseCreator responseCreator)
        {
            // TODO: AssertUtils
            //AssertUtils.ArgumentNotNull(responseCreator, "'responseCreator' must not be null");
            this.responseCreator = responseCreator;
        }

        #endregion

        /// <summary>
        /// Returns the body content as a string.
        /// </summary>
        /// <returns>The body content.</returns>
        public string GetBodyContent()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.body(stream);
                byte[] bodyAsBytes = stream.ToArray();

                return Encoding.UTF8.GetString(bodyAsBytes, 0, bodyAsBytes.Length);
            }
	    }
    }
}
