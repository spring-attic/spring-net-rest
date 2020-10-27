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
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.Globalization;

using Spring.Util;

namespace Spring.Http.Client
{
    /// <summary>
    /// <see cref="IClientHttpRequest"/> implementation that uses 
    /// .NET <see cref="HttpWebRequest"/>'s class to execute requests.
    /// </summary>
    /// <seealso cref="WebClientHttpRequestFactory"/>
    /// <author>Bruno Baia</author>
    public class WebClientHttpRequest : IClientHttpRequest
    {
        private HttpHeaders headers;
        private Action<Stream> body;
        private HttpWebRequest httpWebRequest;

        private bool isExecuted;
        private bool isCancelled;

        /// <summary>
        /// Gets the <see cref="HttpWebRequest"/> instance used by the request.
        /// </summary>
        public HttpWebRequest HttpWebRequest
        {
            get { return this.httpWebRequest; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="WebClientHttpRequest"/> 
        /// with the given <see cref="HttpWebRequest"/> instance.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest"/> instance to use.</param>
        public WebClientHttpRequest(HttpWebRequest request)
        {
            ArgumentUtils.AssertNotNull(request, "request");

            this.httpWebRequest = request;
            this.headers = new HttpHeaders();
        }

        #region IClientHttpRequest Members

        /// <summary>
        /// Gets the HTTP method of the request.
        /// </summary>
        public HttpMethod Method
        {
            get 
            {
                return new HttpMethod(this.httpWebRequest.Method);
            }
        }

        /// <summary>
        /// Gets the URI of the request.
        /// </summary>
        public Uri Uri
        {
            get 
            { 
                return this.httpWebRequest.RequestUri; 
            }
        }

        /// <summary>
        /// Gets the message headers.
        /// </summary>
        public HttpHeaders Headers
        {
            get { return headers; }
        }

        /// <summary>
        /// Sets the delegate that writes the body message as a stream.
        /// </summary>
        public Action<Stream> Body
        {
            set { this.body = value; }
        }

        /// <summary>
        /// Execute this request, resulting in a <see cref="IClientHttpResponse" /> that can be read.
        /// </summary>
        /// <returns>The response result of the execution</returns>
        /// <see cref="InvalidOperationException">If the request is already executed or is currently executing.</see>
        public IClientHttpResponse Execute()
        {
            this.EnsureNotExecuted();

            try
            {
                // Prepare
                this.PrepareForExecution();

                // Write
                if (this.body != null)
                {
                    using (Stream stream = this.httpWebRequest.GetRequestStream())
                    {
                        this.body(stream);
                    }
                }

                // Read
                HttpWebResponse httpWebResponse = this.httpWebRequest.GetResponse() as HttpWebResponse;
                if (this.httpWebRequest.HaveResponse && httpWebResponse != null)
                {
                    return this.CreateClientHttpResponse(httpWebResponse);
                }
            }
            catch (WebException ex)
            {
                // This exception can be raised with some status code 
                // Try to retrieve the response from the error
                HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    return this.CreateClientHttpResponse(httpWebResponse);
                }
                throw;
            }
            finally
            {
                this.isExecuted = true;
            }

            return null;
        }
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
        /// <see cref="InvalidOperationException">If the request is already executed or is currently executing.</see>
        public void ExecuteAsync(object state, Action<ClientHttpRequestCompletedEventArgs> executeCompleted)
        {
            this.EnsureNotExecuted();

            AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(state);
            ExecuteState executeState = new ExecuteState(executeCompleted, asyncOperation);

            try
            {
                // Prepare
                this.PrepareForExecution();

                // Post request
                if (this.body != null)
                {
                    this.httpWebRequest.BeginGetRequestStream(ExecuteRequestCallback, executeState);
                }
                else
                {
                    // Get request
                    this.HttpWebRequest.BeginGetResponse(ExecuteResponseCallback, executeState);
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
                {
                    throw;
                }
                ExecuteAsyncCallback(executeState, null, ex);
            }
            finally
            {
                this.isExecuted = true;
            }
        }

        /// <summary>
        /// Cancels a pending asynchronous operation.
        /// </summary>
        public void CancelAsync()
        {
            this.isCancelled = true;
            try
            {
                if (this.httpWebRequest != null)
                {
                    this.httpWebRequest.Abort();
                }
            }
            catch (Exception exception)
            {
                if (((exception is OutOfMemoryException) || (exception is StackOverflowException)) || (exception is ThreadAbortException))
                {
                    throw;
                }
            }
        }

        #endregion


        #region Async methods/classes


        private void ExecuteRequestCallback(IAsyncResult result)
        {
            ExecuteState state = (ExecuteState)result.AsyncState;

            try
            {
                // Write
                using (Stream stream = this.httpWebRequest.EndGetRequestStream(result))
                {
                    this.body(stream);
                }

                // Read
                this.httpWebRequest.BeginGetResponse(ExecuteResponseCallback, state);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
                {
                    throw;
                }
                ExecuteAsyncCallback(state, null, ex);
            }
        }

        private void ExecuteResponseCallback(IAsyncResult result)
        {
            ExecuteState state = (ExecuteState)result.AsyncState;

            IClientHttpResponse response = null;
            Exception exception = null;
            try
            {
                HttpWebResponse httpWebResponse = this.httpWebRequest.EndGetResponse(result) as HttpWebResponse;
                if (this.httpWebRequest.HaveResponse == true && httpWebResponse != null)
                {
                    response = this.CreateClientHttpResponse(httpWebResponse);
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
                {
                    throw;
                }
                exception = ex;
                // This exception can be raised with some status code 
                // Try to retrieve the response from the error
                if (ex is WebException)
                {
                    HttpWebResponse httpWebResponse = ((WebException)ex).Response as HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        exception = null;
                        response = this.CreateClientHttpResponse(httpWebResponse);
                    }
                }
            }

            ExecuteAsyncCallback(state, response, exception);
        }

        // This is the method that the underlying, free-threaded asynchronous behavior will invoke.  
        // This will happen on an arbitrary thread.
        private void ExecuteAsyncCallback(ExecuteState state, IClientHttpResponse response, Exception exception)
        {
            // Package the results of the operation
            ClientHttpRequestCompletedEventArgs eventArgs = new ClientHttpRequestCompletedEventArgs(response, exception, this.isCancelled, state.AsyncOperation.UserSuppliedState);
            ExecuteCallbackArgs<ClientHttpRequestCompletedEventArgs> callbackArgs = new ExecuteCallbackArgs<ClientHttpRequestCompletedEventArgs>(eventArgs, state.ExecuteCompleted);

            // End the task. The asyncOp object is responsible for marshaling the call.
            state.AsyncOperation.PostOperationCompleted(ExecuteResponseReceived, callbackArgs);
        }

        private static void ExecuteResponseReceived(object arg)
        {
            ExecuteCallbackArgs<ClientHttpRequestCompletedEventArgs> callbackArgs = (ExecuteCallbackArgs<ClientHttpRequestCompletedEventArgs>)arg;
            if (callbackArgs.Callback != null)
            {
                callbackArgs.Callback(callbackArgs.EventArgs);
            }
        }

        private class ExecuteCallbackArgs<T> where T : class
        {
            public T EventArgs;
            public Action<T> Callback;

            public ExecuteCallbackArgs(T eventArgs,
                Action<T> callback)
            {
                this.EventArgs = eventArgs;
                this.Callback = callback;
            }
        }

        private class ExecuteState
        {
            public Action<ClientHttpRequestCompletedEventArgs> ExecuteCompleted;
            public AsyncOperation AsyncOperation;

            public ExecuteState(
                Action<ClientHttpRequestCompletedEventArgs> executeCompleted,
                AsyncOperation asyncOperation)
            {
                this.ExecuteCompleted = executeCompleted;
                this.AsyncOperation = asyncOperation;
            }
        }

        #endregion

        /// <summary>
        /// Ensures that the request can be executed.
        /// </summary>
        /// <see cref="InvalidOperationException">If the request is already executed or is currently executing.</see>
        protected void EnsureNotExecuted()
        {
            if (this.isExecuted)
            {
                throw new InvalidOperationException("Client HTTP request already executed or is currently executing.");
            }
        }

        /// <summary>
        /// Creates and returns an <see cref="IClientHttpResponse"/> implementation associated 
        /// with the request. 
        /// </summary>
        /// <param name="response">The <see cref="HttpWebResponse"/> instance to use.</param>
        /// <returns>
        /// An <see cref="IClientHttpResponse"/> implementation associated with the request.
        /// </returns>
        protected virtual IClientHttpResponse CreateClientHttpResponse(HttpWebResponse response)
        {
            return new WebClientHttpResponse(response);
        }

        /// <summary>
        /// Prepare the request for execution.
        /// </summary>
        /// <remarks>
        /// Default implementation copies headers to the .NET request. Can be overridden in subclasses.
        /// </remarks>
        protected virtual void PrepareForExecution()
        {
            // Copy headers
            foreach (string header in this.headers)
            {
                // Special headers
                switch (header.ToUpper(CultureInfo.InvariantCulture))
                {
                    case "ACCEPT":
                        {
                            this.httpWebRequest.Accept = this.headers[header];
                            break;
                        }

                    case "CONTENT-LENGTH":
                        {
                            this.httpWebRequest.ContentLength = this.headers.ContentLength;
                            break;
                        }
                    case "CONTENT-TYPE":
                        {
                            this.httpWebRequest.ContentType = this.headers[header];
                            break;
                        }
                    case "DATE" :
                        {
                            DateTime? date = this.headers.Date;
                            if (date.HasValue)
                            {
                                this.httpWebRequest.Date = date.Value;
                            }
                            else
                            {
                                this.httpWebRequest.Date = DateTime.MinValue;
                            }
                            break;
                        }
                    case "HOST" :
                        {
                            this.httpWebRequest.Host = this.headers[header];
                            break;
                        }
                    case "CONNECTION":
                        {
                            string headerValue = this.headers[header];
                            if (headerValue.Equals("Keep-Alive", StringComparison.OrdinalIgnoreCase))
                            {
                                this.httpWebRequest.KeepAlive = true;
                            }
                            else if (!headerValue.Equals("Close", StringComparison.OrdinalIgnoreCase))
                            {
                                this.httpWebRequest.Connection = headerValue;
                            }
                            break;
                        }
                    case "EXPECT":
                        {
                            this.httpWebRequest.Expect = this.headers[header];
                            break;
                        }
                    case "IF-MODIFIED-SINCE":
                        {
                            DateTime? date = this.headers.IfModifiedSince;
                            if (date.HasValue)
                            {
                                this.httpWebRequest.IfModifiedSince = date.Value;
                            }
                            else
                            {
                                this.httpWebRequest.IfModifiedSince = DateTime.MinValue;
                            }
                            break;
                        }
                    case "RANGE":
                        {
                            string headerValue = this.headers[header];
                            try
                            {
                                // Supports '<range specifier>=<from>-<to>' format
                                string[] rangesSpecifiers = headerValue.Split('=');
                                string rangesSpecifier = rangesSpecifiers[0];
                                string byteRangesSpecifier = rangesSpecifiers[1];
                                int byteRangesSpecifierSeparator = byteRangesSpecifier.IndexOf('-');

                                long from = long.Parse(byteRangesSpecifier.Substring(0, byteRangesSpecifierSeparator));
                                long to = long.Parse(byteRangesSpecifier.Substring(byteRangesSpecifierSeparator + 1));

                                this.httpWebRequest.AddRange(rangesSpecifier, from, to);
                            }
                            catch
                            {
                                this.httpWebRequest.Headers[header] = this.headers[header];
                            }
                            break;
                        }
                    case "REFERER":
                        {
                            this.httpWebRequest.Referer = this.headers[header];
                            break;
                        }
                    case "TRANSFER-ENCODING":
                        {
                            this.httpWebRequest.SendChunked = true;
                            string headerValue = this.headers[header];
                            if (!headerValue.Equals("Chunked", StringComparison.OrdinalIgnoreCase))
                            {
                                this.httpWebRequest.TransferEncoding = headerValue;
                            }
                            break;
                        }
                    case "USER-AGENT":
                        {
                            this.httpWebRequest.UserAgent = this.headers[header];
                            break;
                        }
                    default:
                        {
                            // Other headers
                            this.httpWebRequest.Headers[header] = this.headers[header];
                            break;
                        }
                }
            }
        }
    }
}
