#region License

/*
 * Copyright 2002-2011 the original author or authors.
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

using System.Net;
using System.Text;

using Spring.Http;
using Spring.Util;

using NUnit.Framework;

namespace Spring.Rest.Client
{
    /// <summary>
    /// Unit tests for the HttpResponseException class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class HttpResponseExceptionTests
    {
        [Test]
        public void BinarySerialization()
        {
            byte[] body = new byte[2] { 0, 1 };
            HttpHeaders headers = new HttpHeaders();
            headers.ContentType = new MediaType("text", "plain");
            HttpStatusCode statusCode = HttpStatusCode.Accepted;
            string statusDescription = "Accepted description";

            HttpResponseException exBefore = new HttpResponseException(
                new HttpResponseMessage<byte[]>(body, headers, statusCode, statusDescription));

            HttpResponseException exAfter = SerializationTestUtils.BinarySerializeAndDeserialize(exBefore) as HttpResponseException;

            Assert.IsNotNull(exAfter);
            Assert.AreEqual(body, exAfter.Response.Body, "Invalid response body");
            Assert.AreEqual(new MediaType("text", "plain"), exAfter.Response.Headers.ContentType, "Invalid response headers");
            Assert.AreEqual(statusCode, exAfter.Response.StatusCode, "Invalid status code");
            Assert.AreEqual(statusDescription, exAfter.Response.StatusDescription, "Invalid status description");
        }
    }
}
