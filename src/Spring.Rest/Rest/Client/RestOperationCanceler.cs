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
using Spring.Http.Client;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Cancels a pending REST asynchronous operation.
    /// </summary>
    public class RestOperationCanceler
    {
        private Uri uri;
        private HttpMethod method;
        private IClientHttpRequest request;

        internal RestOperationCanceler(Uri uri, HttpMethod method)
        {
            this.uri = uri;
            this.method = method;
        }

        internal RestOperationCanceler(IClientHttpRequest request)
        {
            this.request = request;
        }

        /// <summary>
        /// Gets the HTTP method of the request.
        /// </summary>
        public HttpMethod Method
        {
            get
            {
                return this.request != null ? this.request.Method : this.method;
            }
        }

        /// <summary>
        /// Gets the URI of the request.
        /// </summary>
        public Uri Uri
        {
            get
            {
                return this.request != null ? this.request.Uri : this.uri;
            }
        }

        /// <summary>
        /// Cancels a pending asynchronous operation.
        /// </summary>
        public void Cancel()
        {
            if (this.request != null)
            {
                this.request.CancelAsync();
            }
        }
    }
}
#endif
