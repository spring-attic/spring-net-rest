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
using System.Text;

using Spring.Http;
using Spring.Http.Client;

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Mock implementation of <see cref="IClientHttpResponse"/>. 
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Lukas Krecan</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public class MockClientHttpResponse : IClientHttpResponse
    {
        private HttpStatusCode statusCode;
	    private string statusDescription;
        private HttpHeaders headers;
        private string body;

        /// <summary>
        /// Creates a new instance of <see cref="MockClientHttpResponse"/>.
        /// </summary>
        /// <param name="body">The body of the response as a string.</param>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="statusDescription">The response status description.</param>
        public MockClientHttpResponse(string body, HttpHeaders headers, HttpStatusCode statusCode, string statusDescription)
        {
            this.body = body;
            this.headers = headers;
            this.statusCode = statusCode;
            this.statusDescription = statusDescription;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MockClientHttpResponse"/>.
        /// </summary>
        /// <param name="body">The body of the response as a stream.</param>
        /// <param name="headers">The response headers.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="statusDescription">The response status description.</param>
        public MockClientHttpResponse(Stream body, HttpHeaders headers, HttpStatusCode statusCode, string statusDescription)
            : this(StreamToString(body), headers, statusCode, statusDescription)
        {
        }

        private static string StreamToString(Stream s)
        {
            using (StreamReader reader = new StreamReader(s, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        #region IClientHttpResponse Members

        /// <summary>
        /// Gets the HTTP status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return this.statusCode; }
        }

        /// <summary>
        /// Gets the HTTP status description of the response.
        /// </summary>
        public string StatusDescription
        {
            get { return this.statusDescription; }
        }

        /// <summary>
        /// Gets the message headers.
        /// </summary>
        public HttpHeaders Headers
        {
            get { return this.headers; }
        }

        /// <summary>
        /// Gets the body of the message as a stream.
        /// </summary>
        public Stream Body
        {
            get  { return new MemoryStream(Encoding.UTF8.GetBytes(this.body)); }
        }

        /// <summary>
        /// Closes this response, freeing any resources created.
        /// </summary>
        public void Close()
        {
        }

        void IDisposable.Dispose()
        {
        }

        #endregion
    }
}
