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

using System.Text;

using NUnit.Framework;

namespace Spring.Http.Converters
{
    /// <summary>
    /// Unit tests for the StringHttpMessageConverter class.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia (.NET)</author>
    [TestFixture]
    public class StringHttpMessageConverterTests
    {
        private StringHttpMessageConverter converter;

	    [SetUp]
	    public void SetUp() 
        {
            converter = new StringHttpMessageConverter();
	    }

        [Test]
        public void CanRead() 
        {
            Assert.IsTrue(converter.CanRead(typeof(string), new MediaType("text", "plain")));
            Assert.IsTrue(converter.CanRead(typeof(string), MediaType.ALL));
            Assert.IsTrue(converter.CanRead(typeof(string), new MediaType("application", "xml")));
            Assert.IsFalse(converter.CanRead(typeof(int[]), new MediaType("text", "plain")));
        }

        [Test]
        public void CanWrite() 
        {
            Assert.IsTrue(converter.CanWrite(typeof(string), new MediaType("text", "plain")));
            Assert.IsTrue(converter.CanWrite(typeof(string), MediaType.ALL));
            Assert.IsTrue(converter.CanWrite(typeof(string), new MediaType("application", "xml")));
            Assert.IsFalse(converter.CanWrite(typeof(int[]), new MediaType("text", "plain")));
        }

        [Test]
	    public void Read() 
        {
            string body = "Hello Bruno Baïa";
            Encoding charSet = Encoding.UTF8;
            MediaType mediaType = new MediaType("text", "plain", charSet);

            MockHttpInputMessage message = new MockHttpInputMessage(body, charSet);
            message.Headers.ContentType = mediaType;
            
            string result = converter.Read<string>(message);
            Assert.AreEqual(body, result, "Invalid result");
	    }

        [Test]
        public void WriteDefaultCharset()
        {
            string body = "H\u00e9llo W\u00f6rld";
            Encoding charSet = Encoding.GetEncoding("ISO-8859-1");
            MediaType mediaType = new MediaType("text", "plain", charSet);

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.AreEqual(body, message.GetBodyAsString(charSet), "Invalid result");
            Assert.AreEqual(mediaType, message.Headers.ContentType, "Invalid content-type");
            //Assert.AreEqual(charSet.GetBytes(body).Length, message.Headers.ContentLength, "Invalid content-length");
        }

        [Test]
        public void WriteUTF8()
        {
            string body = "H\u00e9llo W\u00f6rld";
            Encoding charSet = Encoding.UTF8;
            MediaType mediaType = new MediaType("text", "plain", charSet);

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, mediaType, message);

            Assert.AreEqual(body, message.GetBodyAsString(charSet), "Invalid result");
            Assert.AreEqual(mediaType, message.Headers.ContentType, "Invalid content-type");
            //Assert.AreEqual(charSet.GetBytes(body).Length, message.Headers.ContentLength, "Invalid content-length");
        }
    }
}
