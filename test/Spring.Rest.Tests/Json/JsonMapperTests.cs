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

using System;

using NUnit.Framework;

namespace Spring.Json
{
    /// <summary>
    /// Unit tests for the JsonMapper class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class JsonMapperTests
    {
        [Test]
        public void Config()
        {
            JsonMapper mapper = new JsonMapper();
            mapper.RegisterDeserializer(typeof(Class1), new Class1Deserializer());
            mapper.RegisterSerializer(typeof(Class2), new Class2Serializer());

            Assert.IsTrue(mapper.CanDeserialize(typeof(Class1)));
            Assert.IsFalse(mapper.CanDeserialize(typeof(Class2)));

            Assert.IsFalse(mapper.CanSerialize(typeof(Class1)));
            Assert.IsTrue(mapper.CanSerialize(typeof(Class2)));
        }

        [Test]
        public void JsonValueSpecialCase()
        {
            JsonMapper mapper = new JsonMapper();
            Assert.IsTrue(mapper.CanDeserialize(typeof(JsonValue)));
            Assert.IsTrue(mapper.CanSerialize(typeof(JsonValue)));
        }

        [Test]
        public void DeserializeKnownType()
        {
            JsonMapper mapper = new JsonMapper();
            mapper.RegisterDeserializer(typeof(Class1), new Class1Deserializer());

            JsonObject value = new JsonObject();
            value.AddValue("ID", new JsonValue("007"));
            Class1 obj1 = mapper.Deserialize<Class1>(value);

            Assert.IsNotNull(obj1);
            Assert.AreEqual("007", obj1.ID);
        }

        [Test]
        [ExpectedException(
            typeof(JsonException),
            ExpectedMessage = "Could not find deserializer for type 'Spring.Json.JsonMapperTests+Class2'.")]
        public void DeserializeUnknownType()
        {
            JsonMapper mapper = new JsonMapper();
            mapper.RegisterDeserializer(typeof(Class1), new Class1Deserializer());

            mapper.Deserialize<Class2>(new JsonValue());
        }

        [Test]
        public void SerializeKnownType()
        {
            JsonMapper mapper = new JsonMapper();
            mapper.RegisterSerializer(typeof(Class2), new Class2Serializer());

            Class2 obj2 = new Class2();
            obj2.ID = 7;
            JsonValue value2 = mapper.Serialize(obj2);
            Assert.IsNotNull(value2);
            Assert.AreEqual(7, value2.GetValue<long>("ID"));
        }

        [Test]
        [ExpectedException(
            typeof(JsonException),
            ExpectedMessage = "Could not find serializer for type 'Spring.Json.JsonMapperTests+Class1'.")]
        public void SerializeUnknownType()
        {
            JsonMapper mapper = new JsonMapper();
            mapper.RegisterSerializer(typeof(Class2), new Class2Serializer());

            mapper.Serialize(new Class1());
        }

        #region Test classes

        public class Class1
        {
            private string _id;

            public string ID
            {
                get { return _id; }
                set { _id = value; }
            }
        }

        public class Class2
        {
            private long _id;

            public long ID
            {
                get { return _id; }
                set { _id = value; }
            }
        }

        public class Class1Deserializer : IJsonDeserializer
        {
            public object Deserialize(JsonValue value, JsonMapper mapper)
            {
                Class1 obj1 = new Class1();
                obj1.ID = value.GetValue<string>("ID");
                return obj1;
            }
        }

        public class Class2Serializer : IJsonSerializer
        {
            public JsonValue Serialize(object obj, JsonMapper mapper)
            {
                Class2 obj1 = obj as Class2;
                JsonObject value = new JsonObject();
                value.AddValue("ID", new JsonValue(obj1.ID));
                return value;
            }
        }

        #endregion
    }
}