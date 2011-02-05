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
        private IClientHttpRequest request;

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
                return this.request.Method;
            }
        }

        /// <summary>
        /// Gets the URI of the request.
        /// </summary>
        public Uri Uri
        {
            get
            {
                return this.request.Uri;
            }
        }

        /// <summary>
        /// Cancels a pending asynchronous operation.
        /// </summary>
        public void Cancel()
        {
            this.request.CancelAsync();
        }
    }
}
