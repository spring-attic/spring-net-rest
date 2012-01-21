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
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Spring.Util;

namespace Spring.Http.Client
{
    /// <summary>
    /// <see cref="IClientHttpRequestFactory"/> implementation that uses 
    /// .NET <see cref="HttpWebRequest"/>'s class to create requests.
    /// </summary>
    /// <see cref="WebClientHttpRequest"/>
    /// <author>Bruno Baia</author>
    public class WebClientHttpRequestFactory : IClientHttpRequestFactory
    {
        #region Properties

#if !SILVERLIGHT_3
        private bool? _allowAutoRedirect;
        /// <summary>
        /// Gets or sets a boolean value that controls whether the request should follow redirection responses.
        /// </summary>
        public bool? AllowAutoRedirect
        {
            get { return this._allowAutoRedirect; }
            set { this._allowAutoRedirect = value; }
        }

#if !CF_3_5
        private bool? _useDefaultCredentials;
        /// <summary>
        /// Gets or sets a boolean value that controls whether default credentials are sent with this request.
        /// </summary>
        public bool? UseDefaultCredentials
        {
            get { return this._useDefaultCredentials; }
            set { this._useDefaultCredentials = value; }
        }
#endif
#endif

#if !CF_3_5
        private CookieContainer _cookieContainer;
        /// <summary>
        /// Gets or sets the cookies for the request.
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return this._cookieContainer; }
            set { this._cookieContainer = value; }
        }
#endif

        private ICredentials _credentials;
        /// <summary>
        /// Gets or sets authentication information for the request.
        /// </summary>
        public ICredentials Credentials
        {
            get { return this._credentials; }
            set { this._credentials = value; }
        }

#if !SILVERLIGHT
        private X509CertificateCollection _clientCertificates;
        /// <summary>
        /// Gets or sets the collection of security certificates that are associated with this request.
        /// </summary>
        public X509CertificateCollection ClientCertificates
        {
            get
            {
                if (this._clientCertificates == null)
                {
                    this._clientCertificates = new X509CertificateCollection();
                }
                return this._clientCertificates;
            }
        }

        private IWebProxy _proxy;
        /// <summary>
        /// Gets or sets proxy information for the request.
        /// </summary>
        /// <remarks>
        /// The default value is set by calling the <see cref="P:System.Net.GlobalProxySelection.Select"/> property.
        /// </remarks>
        public IWebProxy Proxy
        {
            get { return this._proxy; }
            set { this._proxy = value; }
        }

        private int? _timeout;
        /// <summary>
        /// Gets or sets the time-out value in milliseconds for synchronous request only.
        /// </summary>
        /// <remarks>
        /// The default is 100,000 milliseconds (100 seconds).
        /// </remarks>
        public int? Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        private bool? _expect100Continue;
        /// <summary>
        /// Gets or sets a boolean value that determines whether 100-Continue behavior is used.
        /// </summary>
        /// <remarks>
        /// The default value is <see langword="true"/>.
        /// </remarks>
        public bool? Expect100Continue
        {
            get { return this._expect100Continue; }
            set { this._expect100Continue = value; }
        }

        private DecompressionMethods? _automaticDecompression;
        /// <summary>
        /// Gets or sets the type of decompression that is automatically used for the response result of this request.
        /// </summary>
        public DecompressionMethods? AutomaticDecompression
        {
            get { return this._automaticDecompression; }
            set { this._automaticDecompression = value; }
        }
#endif

#if SILVERLIGHT && !WINDOWS_PHONE
        private WebRequestCreatorType _webRequestCreator;
        /// <summary>
        /// Gets or sets a value that indicates how HTTP requests and responses will be handled. 
        /// </summary>
        /// <remarks>
        /// By default, this factory will use the default Silverlight behavior for HTTP methods GET and POST, 
        /// and force the client HTTP stack for other HTTP methods.
        /// </remarks>
        public WebRequestCreatorType WebRequestCreator
        {
            get { return this._webRequestCreator; }
            set { this._webRequestCreator = value; }
        }
#endif

        #endregion

#if SILVERLIGHT && !WINDOWS_PHONE
        /// <summary>
        /// Creates a new instance of <see cref="WebClientHttpRequestFactory"/>.
        /// </summary>
        public WebClientHttpRequestFactory()
        {
            this._webRequestCreator = WebRequestCreatorType.Unknown;
        }
#endif

        #region IClientHttpRequestFactory Members

        /// <summary>
        /// Create a new <see cref="IClientHttpRequest"/> for the specified URI and HTTP method.
        /// </summary>
        /// <param name="uri">The URI to create a request for.</param>
        /// <param name="method">The HTTP method to execute.</param>
        /// <returns>The created request</returns>
        public virtual IClientHttpRequest CreateRequest(Uri uri, HttpMethod method)
        {
            ArgumentUtils.AssertNotNull(uri, "uri");
            ArgumentUtils.AssertNotNull(method, "method");

            HttpWebRequest httpWebRequest;
#if SILVERLIGHT && !WINDOWS_PHONE
            switch (this._webRequestCreator)
            {
                case WebRequestCreatorType.ClientHttp:
                    httpWebRequest = (HttpWebRequest)System.Net.Browser.WebRequestCreator.ClientHttp.Create(uri);
                    break;
                case WebRequestCreatorType.BrowserHttp:
                    httpWebRequest = (HttpWebRequest)System.Net.Browser.WebRequestCreator.BrowserHttp.Create(uri);
                    break;
                case WebRequestCreatorType.Unknown:
                default:
                    if (method == HttpMethod.GET || method == HttpMethod.POST)
                    {
                        httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
                    }
                    else
                    {
                        // Force Client HTTP stack
                        httpWebRequest = (HttpWebRequest)System.Net.Browser.WebRequestCreator.ClientHttp.Create(uri);
                    }
                    break;
            }
#else
            httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
#endif

            httpWebRequest.Method = method.ToString();

#if !SILVERLIGHT_3
            if (this._allowAutoRedirect != null)
            {
                httpWebRequest.AllowAutoRedirect = this._allowAutoRedirect.Value;
            }
#if !CF_3_5
            if (this._useDefaultCredentials.HasValue)
            {
                httpWebRequest.UseDefaultCredentials = this._useDefaultCredentials.Value;
            }
#endif
#endif
#if !CF_3_5
            if (this._cookieContainer != null)
            {
                httpWebRequest.CookieContainer = this._cookieContainer;
            }
#endif
            if (this._credentials != null)
            {
                httpWebRequest.Credentials = this._credentials;
            }
#if !SILVERLIGHT
            if (this._clientCertificates != null)
            {
                foreach (X509Certificate2 certificate in this._clientCertificates)
                {
                    httpWebRequest.ClientCertificates.Add(certificate);
                }
            }
            if (this._proxy != null)
            {
                httpWebRequest.Proxy = this._proxy;
            }
            if (this._timeout != null)
            {
                httpWebRequest.Timeout = this._timeout.Value;
            }
            if (this._expect100Continue != null)
            {
                httpWebRequest.ServicePoint.Expect100Continue = this._expect100Continue.Value;
            }
            if (this._automaticDecompression.HasValue)
            {
                httpWebRequest.AutomaticDecompression = this._automaticDecompression.Value;
            }
#endif

            return new WebClientHttpRequest(httpWebRequest);
        }

        #endregion
    }

#if SILVERLIGHT && !WINDOWS_PHONE
    /// <summary>
    /// Defines identifiers for supported Silverlight HTTP handling stacks.  
    /// </summary>
    public enum WebRequestCreatorType
    {
        /// <summary>
        /// Specifies an unknown HTTP handling stack.
        /// </summary>
        Unknown,
        /// <summary>
        /// Specifies browser HTTP handling stack.
        /// </summary>
        BrowserHttp,
        /// <summary>
        /// Specifies client HTTP handling stack.
        /// </summary>
        ClientHttp
    }
#endif
}
