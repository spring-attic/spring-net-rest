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
using System.ComponentModel;

namespace Spring.Http.Client
{
    /// <summary>
    /// Provides data when an asynchronous HTTP request execution completes.
    /// </summary>
    /// <see cref="IClientHttpRequest"/>
    public class ClientHttpRequestCompletedEventArgs : AsyncCompletedEventArgs
    {
        private IClientHttpResponse response;

        /// <summary>
        /// Gets the <see cref="IClientHttpResponse">response</see> result of the execution.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">If the execution was canceled.</exception>
        /// <exception cref="System.Reflection.TargetInvocationException">If the execution failed.</exception>
        public IClientHttpResponse Response
        {
            get
            {
                // Raise an exception if the operation failed or was canceled.
                base.RaiseExceptionIfNecessary();

                // If the operation was successful, return the value.
                return response;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClientHttpRequestCompletedEventArgs"/>.
        /// </summary>
        /// <param name="response">The response of the execution.</param>
        /// <param name="exception">Any error that occurred during the asynchronous execution.</param>
        /// <param name="cancelled">A value indicating whether the asynchronous execution was canceled.</param>
        /// <param name="userState">The optional user-supplied state object.</param>
        public ClientHttpRequestCompletedEventArgs(IClientHttpResponse response, Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            this.response = response;
        }
    }
}
