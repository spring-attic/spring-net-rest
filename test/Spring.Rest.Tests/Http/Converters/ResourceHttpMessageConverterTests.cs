#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
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

using NUnit.Framework;

using Spring.IO;

namespace Spring.Http.Converters
{
    /// <summary>
    /// Unit tests for the ResourceHttpMessageConverter class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class ResourceHttpMessageConverterTests
    {
        private ResourceHttpMessageConverter converter;

	    [SetUp]
	    public void SetUp() 
        {
            converter = new ResourceHttpMessageConverter();
	    }

        [Test]
        public void CanRead() 
        {
            Assert.IsTrue(converter.CanRead(typeof(IResource), MediaType.ALL));
            Assert.IsTrue(converter.CanRead(typeof(FileResource), MediaType.ALL));
            Assert.IsTrue(converter.CanRead(typeof(IResource), new MediaType("text", "plain")));
            Assert.IsFalse(converter.CanRead(typeof(string), new MediaType("text", "plain")));
        }

        [Test]
        public void CanWrite() 
        {
            Assert.IsTrue(converter.CanWrite(typeof(IResource), MediaType.ALL));
            Assert.IsTrue(converter.CanWrite(typeof(FileResource), MediaType.ALL));
            Assert.IsTrue(converter.CanWrite(typeof(IResource), new MediaType("text", "plain")));
            Assert.IsFalse(converter.CanWrite(typeof(string), new MediaType("text", "plain")));
        }

        [Test]
	    public void Read()
        {
            byte[] body = new byte[] { 0x1, 0x2 };

            MockHttpInputMessage message = new MockHttpInputMessage(body);

            IResource resource = converter.Read<IResource>(message);

            Assert.IsNotNull(resource, "Invalid result");
            Assert.IsTrue(resource is ByteArrayResource, "Invalid result");
            byte[] result = ((ByteArrayResource)resource).Bytes;
            Assert.AreEqual(body[0], result[0], "Invalid result");
            Assert.AreEqual(body[1], result[1], "Invalid result");
	    }

	    [Test]
	    public void Write() 
        {
            IResource body = new AssemblyResource("assembly://Spring.Rest.Tests/Spring.Http.Converters/Resource.txt");

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.IsTrue(message.GetBodyAsBytes().Length > 0, "Invalid result");
            Assert.AreEqual(new MediaType("text", "plain"), message.Headers.ContentType, "Invalid content-type");
	    }

        [Test]
        public void WriteWithUnknownExtension()
        {
            IResource body = new FileResource(@"C:\Dummy.unknown");

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.AreEqual(new MediaType("application", "octet-stream"), message.Headers.ContentType, "Invalid content-type");
        }

        [Test]
        public void WriteWithKnownExtension()
        {
            IResource body = new FileResource(@"C:\Dummy.txt");

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.AreEqual(new MediaType("text", "plain"), message.Headers.ContentType, "Invalid content-type");
        }


        [Test]
        public void WriteWithCustomExtension()
        {
            IResource body = new FileResource(@"C:\Dummy.myext");

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.MimeMapping.Add(".myext", "spring/custom");
            converter.Write(body, null, message);

            Assert.AreEqual(new MediaType("spring", "custom"), message.Headers.ContentType, "Invalid content-type");
        }
    }
}
