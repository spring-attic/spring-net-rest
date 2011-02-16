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

using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace Spring.Http.Converters.Json
{
    /// <summary>
    /// Unit tests for the NJsonHttpMessageConverter class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class NJsonHttpMessageConverterTests
    {
        private NJsonHttpMessageConverter converter;

	    [SetUp]
	    public void SetUp() 
        {
            converter = new NJsonHttpMessageConverter();
	    }

        [Test]
        public void CanRead() 
        {
            Assert.IsTrue(converter.CanRead(typeof(CustomClass), new MediaType("application", "json")));
            Assert.IsTrue(converter.CanRead(typeof(JArray), new MediaType("application", "json")));
            Assert.IsFalse(converter.CanRead(typeof(CustomClass), new MediaType("text", "xml")));
        }

        [Test]
        public void CanWrite() 
        {
            Assert.IsTrue(converter.CanWrite(typeof(CustomClass), new MediaType("application", "json")));
            Assert.IsTrue(converter.CanRead(typeof(JArray), new MediaType("application", "json")));
            Assert.IsFalse(converter.CanWrite(typeof(CustomClass), new MediaType("text", "xml")));
        }

        [Test]
        public void ReadClass()
        {
            string body = "{\"ID\":1,\"Name\":\"Bruno Baïa\"}";

            MockHttpInputMessage message = new MockHttpInputMessage(body, Encoding.UTF8);

            CustomClass result = converter.Read<CustomClass>(message);
            Assert.IsNotNull(result, "Invalid result");
            Assert.AreEqual(1, result.ID, "Invalid result");
            Assert.AreEqual("Bruno Baïa", result.Name, "Invalid result");
        }

        [Test]
        public void ReadJToken()
        {
            string body = "{\"ID\":1,\"Name\":\"Bruno Baïa\"}";

            MockHttpInputMessage message = new MockHttpInputMessage(body, Encoding.UTF8);

            JToken result = converter.Read<JToken>(message);
            Assert.IsNotNull(result, "Invalid result");
            Assert.AreEqual(1, result.Value<int>("ID"), "Invalid result");
            Assert.AreEqual("Bruno Baïa", result.Value<string>("Name"), "Invalid result");
        }

        [Test]
        public void WriteClass()
        {
            string expectedBody = "{\"ID\":1,\"Name\":\"Bruno Baïa\"}";
            CustomClass body = new CustomClass(1, "Bruno Baïa");

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.AreEqual(expectedBody, message.GetBodyAsString(Encoding.UTF8), "Invalid result");
            Assert.AreEqual(new MediaType("application", "json"), message.Headers.ContentType, "Invalid content-type");
            //Assert.IsTrue(message.Headers.ContentLength > -1, "Invalid content-length");
        }

        [Test]
        public void WriteJToken()
        {
            string expectedBody = "{\"ID\":1,\"Name\":\"Bruno Baïa\"}";
            JObject body = new JObject(
                new JProperty("ID", new JValue(1)), 
                new JProperty("Name", new JValue("Bruno Baïa")));

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.AreEqual(expectedBody, message.GetBodyAsString(Encoding.UTF8), "Invalid result");
            Assert.AreEqual(new MediaType("application", "json"), message.Headers.ContentType, "Invalid content-type");
            //Assert.IsTrue(message.Headers.ContentLength > -1, "Invalid content-length");
        }

        #region Test classes
        
        public class CustomClass
        {
            private int _id;
            public int ID 
            { 
                get { return _id; }
                set { _id = value; }
            }

            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public CustomClass()
            {
            }

            public CustomClass(int id, string name)
            {
                this.ID = id;
                this.Name = name;
            }
        }

        #endregion
    }
}