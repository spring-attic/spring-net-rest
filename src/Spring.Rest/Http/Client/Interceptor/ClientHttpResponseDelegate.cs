using System;

namespace Spring.Http.Client.Interceptor
{
    /// <summary>
    /// Callback delegate used by the <see cref="IClientHttpRequestExecution"/> interface for handling 
    /// the client-side HTTP response returned by a request execution.
    /// </summary>
    /// <param name="response">The response result of the execution.</param>
    /// <returns>The response result of the execution.</returns>
    public delegate IClientHttpResponse ClientHttpResponseDelegate(IClientHttpResponse response);
}
