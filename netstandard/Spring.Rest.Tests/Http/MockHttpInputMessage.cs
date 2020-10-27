﻿#region License

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

using System.IO;
using System.Text;

namespace Spring.Http
{
    /// <summary>
    /// Mocked IHttpInputMessage implementation.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia (.NET)</author>
    public class MockHttpInputMessage : IHttpInputMessage
    {
        private HttpHeaders headers;
        private Stream body;

        public MockHttpInputMessage(byte[] body)
        {
            this.headers = new HttpHeaders();
            this.body = new MemoryStream(body);
        }

        public MockHttpInputMessage(string body, Encoding charset)
            : this(charset.GetBytes(body))
        {
        }

        public HttpHeaders Headers
        {
            get { return this.headers; }
        }

        public Stream Body
        {
            get { return this.body;  }
        }
    }
}
