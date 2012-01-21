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
using System.Text;

using Spring.Json;

using NUnit.Framework;

namespace Spring.Http.Converters.Json
{
    /// <summary>
    /// Unit tests for the SpringJsonHttpMessageConverter class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class SpringJsonHttpMessageConverterTests
    {
        private SpringJsonHttpMessageConverter converter;

	    [SetUp]
	    public void SetUp() 
        {
            JsonMapper mapper = new JsonMapper();
            mapper.RegisterDeserializer(typeof(CustomClass1), new CustomClass1Deserializer());
            mapper.RegisterSerializer(typeof(CustomClass2), new CustomClass2Serializer());

            converter = new SpringJsonHttpMessageConverter(mapper);
	    }

        [Test]
        public void CanRead() 
        {
            Assert.IsTrue(converter.CanRead(typeof(CustomClass1), new MediaType("application", "json")));
            Assert.IsFalse(converter.CanRead(typeof(CustomClass2), new MediaType("application", "json")));
            Assert.IsTrue(converter.CanRead(typeof(JsonValue), new MediaType("application", "json")));
            Assert.IsTrue(converter.CanRead(typeof(JsonObject), new MediaType("application", "json")));
            Assert.IsFalse(converter.CanRead(typeof(JsonValue), new MediaType("text", "xml")));
        }

        [Test]
        public void CanWrite() 
        {
            Assert.IsFalse(converter.CanWrite(typeof(CustomClass1), new MediaType("application", "json")));
            Assert.IsTrue(converter.CanWrite(typeof(CustomClass2), new MediaType("application", "json")));
            Assert.IsTrue(converter.CanRead(typeof(JsonValue), new MediaType("application", "json")));
            Assert.IsTrue(converter.CanRead(typeof(JsonArray), new MediaType("application", "json")));
            Assert.IsFalse(converter.CanWrite(typeof(CustomClass1), new MediaType("text", "xml")));
        }

        [Test]
        public void ReadClass()
        {
            string body = "{\"ID\":1,\"Name\":\"Bruno Baïa\"}";

            MockHttpInputMessage message = new MockHttpInputMessage(body, Encoding.UTF8);

            CustomClass1 result = converter.Read<CustomClass1>(message);
            Assert.IsNotNull(result, "Invalid result");
            Assert.AreEqual(1, result.ID, "Invalid result");
            Assert.AreEqual("Bruno Baïa", result.Name, "Invalid result");
        }

        [Test]
        public void ReadJsonValue()
        {
            string body = "{\"ID\":1,\"Name\":\"Bruno Baïa\"}";

            MockHttpInputMessage message = new MockHttpInputMessage(body, Encoding.UTF8);

            JsonValue result = converter.Read<JsonValue>(message);
            Assert.IsNotNull(result, "Invalid result");
            Assert.AreEqual(1, result.GetValue<int>("ID"), "Invalid result");
            Assert.AreEqual("Bruno Baïa", result.GetValue<string>("Name"), "Invalid result");
        }

        [Test]
        public void WriteClass()
        {
            string expectedBody = "{\"ID\":1,\"Age\":31}";
            CustomClass2 body = new CustomClass2(1, 31);

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.AreEqual(expectedBody, message.GetBodyAsString(Encoding.UTF8), "Invalid result");
            Assert.AreEqual(new MediaType("application", "json"), message.Headers.ContentType, "Invalid content-type");
            //Assert.IsTrue(message.Headers.ContentLength > -1, "Invalid content-length");
        }

        [Test]
        public void WriteJsonValue()
        {
            string expectedBody = "{\"ID\":1,\"Age\":31}";
            JsonObject body = new JsonObject();
            body.AddValue("ID", new JsonValue(1));
            body.AddValue("Age", new JsonValue(31));

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.AreEqual(expectedBody, message.GetBodyAsString(Encoding.UTF8), "Invalid result");
            Assert.AreEqual(new MediaType("application", "json"), message.Headers.ContentType, "Invalid content-type");
            //Assert.IsTrue(message.Headers.ContentLength > -1, "Invalid content-length");
        }

        #region Test classes
        
        public class CustomClass1
        {
            private long _id;
            public long ID 
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

            public CustomClass1()
            {
            }

            public CustomClass1(long id, string name)
            {
                this.ID = id;
                this.Name = name;
            }
        }

        public class CustomClass2
        {
            private long _id;
            public long ID
            {
                get { return _id; }
                set { _id = value; }
            }

            private int _age;
            public int Age
            {
                get { return _age; }
                set { _age = value; }
            }

            public CustomClass2()
            {
            }

            public CustomClass2(long id, int age)
            {
                this.ID = id;
                this.Age = age;
            }
        }

        public class CustomClass1Deserializer : IJsonDeserializer
        {
            public object Deserialize(JsonValue value, JsonMapper mapper)
            {
                CustomClass1 result = new CustomClass1();
                result.ID = value.GetValue<long>("ID");
                result.Name = value.GetValue<string>("Name");
                return result;
            }
        }

        public class CustomClass2Serializer : IJsonSerializer
        {
            public JsonValue Serialize(object obj, JsonMapper mapper)
            {
                CustomClass2 data = obj as CustomClass2;

                JsonObject result = new JsonObject();
                result.AddValue("ID", new JsonValue(data.ID));
                result.AddValue("Age", new JsonValue(data.Age));
                return result;
            }
        }

        #endregion
    }
}