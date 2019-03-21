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
    /// Unit tests for the JsonValue class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class JsonValueTests
    {
        [Test]
        public void ParseAndGenerate()
        {
            string json = "{\"ID\":1,\"Name\":\"Bruno Baïa\",\"IsFrench\":true,\"SpeaksEnglish\":false,\"Description\":null,\"Childrens\":[\"Lisa\"]}";
            JsonValue value = JsonValue.Parse(json);
            Assert.AreEqual(json, value.ToString());
        }

        [Test]
        public void Generate()
        {
            JsonArray jsonArray = new JsonArray();
            jsonArray.AddValue(new JsonValue());
            jsonArray.AddValue(new JsonValue(false));
            jsonArray.AddValue(new JsonValue(true));
            jsonArray.AddValue(new JsonValue((byte)1));
            jsonArray.AddValue(new JsonValue((decimal)2.5));
            jsonArray.AddValue(new JsonValue((double)0.7));
            jsonArray.AddValue(new JsonValue((float)1.4));
            jsonArray.AddValue(new JsonValue((int)6));
            jsonArray.AddValue(new JsonValue((long)16));
            jsonArray.AddValue(new JsonValue((short)2));
            jsonArray.AddValue(new JsonValue("\tbla"));
            jsonArray.AddValue(new JsonObject());
            jsonArray.AddValue(new JsonArray());

            Assert.AreEqual("[null,false,true,1,2.5,0.7,1.4,6,16,2,\"\\tbla\",{},[]]", jsonArray.ToString());
        }

        [Test]
        [ExpectedException(typeof(JsonException),
            ExpectedMessage = "Could not parse JSON string '{\"ID\":}'.")]
        public void ParseError()
        {
            JsonValue value = JsonValue.Parse("{\"ID\":}");
        }

        [Test]
        public void TryParseFailed()
        {
            JsonValue value;
            Assert.IsFalse(JsonValue.TryParse("[4}", out value));
            Assert.IsNull(value);
        }

        [Test]
        public void TryParseNull()
        {
            JsonValue value;
            Assert.IsTrue(JsonValue.TryParse(null, out value));
            Assert.IsNull(value);
        }

        [Test]
        public void GetStringValue()
        {
            JsonValue value1 = JsonValue.Parse("\"Hello world!\"");
            Assert.IsTrue(value1.IsString);
            Assert.AreEqual("Hello world!", value1.GetValue<string>());

            JsonValue value2 = JsonValue.Parse("\"\\u0061\"");
            Assert.IsTrue(value2.IsString);
            Assert.AreEqual("a", value2.GetValue<string>());

            JsonValue value3 = JsonValue.Parse("\"\\uD845\\uDE1A\"");
            Assert.IsTrue(value3.IsString);
            Assert.AreEqual("𡘚", value3.GetValue<string>());
        }

        [Test]
        public void GetNumberValue()
        {
            JsonValue value1 = JsonValue.Parse("5.1878483143195925e+49");
            Assert.IsTrue(value1.IsNumber);
            Assert.AreEqual(Math.Pow(Math.PI, 100), value1.GetValue<double>());

            JsonValue value2 = JsonValue.Parse("007");
            Assert.IsTrue(value2.IsNumber);
            Assert.AreEqual(7, value2.GetValue<int>());
        }

        [Test]
        public void GetBooleanValue()
        {
            JsonValue value1 = JsonValue.Parse("true");
            Assert.IsTrue(value1.IsBoolean);
            Assert.AreEqual(true, value1.GetValue<bool>());

            JsonValue value2 = JsonValue.Parse("false");
            Assert.IsTrue(value2.IsBoolean);
            Assert.AreEqual(false, value2.GetValue<bool>());
        }

        [Test]
        public void GetNullValue()
        {
            JsonValue value = JsonValue.Parse("null");
            Assert.IsTrue(value.IsNull);
        }

        [Test]
        public void GetObjectValue()
        {
            JsonValue value = JsonValue.Parse(
                "{\"ID\":1,\"Name\":\"Bruno Baïa\",\"IsFrench\":true,\"SpeaksEnglish\":false,\"Description\":null,\"Childrens\":[\"Lisa\"]}"
                );

            Assert.IsTrue(value.IsObject);
            Assert.AreEqual(6, value.GetNames().Count);
            Assert.AreEqual(6, value.GetValues().Count);
            Assert.IsNotNull(value.GetValue("ID"));
            Assert.IsTrue(value.GetValue("ID").IsNumber);
            Assert.AreEqual(1, value.GetValue("ID").GetValue<int>());
            Assert.AreEqual(1, value.GetValue<int>("ID"));
            Assert.IsNotNull(value.GetValue("Name"));
            Assert.IsTrue(value.GetValue("Name").IsString);
            Assert.AreEqual("Bruno Baïa", value.GetValue("Name").GetValue<string>());
            Assert.AreEqual("Bruno Baïa", value.GetValue<string>("Name"));
            Assert.IsNotNull(value.GetValue("IsFrench"));
            Assert.IsTrue(value.GetValue("IsFrench").IsBoolean);
            Assert.AreEqual(true, value.GetValue("IsFrench").GetValue<bool>());
            Assert.AreEqual(true, value.GetValue<bool>("IsFrench"));
            Assert.IsNotNull(value.GetValue("SpeaksEnglish"));
            Assert.IsTrue(value.GetValue("SpeaksEnglish").IsBoolean);
            Assert.AreEqual(false, value.GetValue("SpeaksEnglish").GetValue<bool>());
            Assert.AreEqual(false, value.GetValue<bool>("SpeaksEnglish"));
            Assert.IsNotNull(value.GetValue("Description"));
            Assert.IsTrue(value.GetValue("Description").IsNull);
            Assert.IsNotNull(value.GetValue("Childrens"));
            Assert.IsTrue(value.GetValue("Childrens").IsArray);
        }

        [Test]
        public void GetArrayValue() 
        {
            JsonValue value = JsonValue.Parse(
                "[1,\"Bruno Baïa\",true,false,null,{},[\"Lisa\"]]"
                );

            Assert.IsTrue(value.IsArray);
            Assert.AreEqual(7, value.GetValues().Count);
            Assert.IsNotNull(value.GetValue(0));
            Assert.IsTrue(value.GetValue(0).IsNumber);
            Assert.AreEqual(1, value.GetValue<int>(0));
            Assert.IsNotNull(value.GetValue(1));
            Assert.IsTrue(value.GetValue(1).IsString);
            Assert.AreEqual("Bruno Baïa", value.GetValue(1).GetValue<string>());
            Assert.AreEqual("Bruno Baïa", value.GetValue<string>(1));
            Assert.IsNotNull(value.GetValue(2));
            Assert.IsTrue(value.GetValue(2).IsBoolean);
            Assert.AreEqual(true, value.GetValue(2).GetValue<bool>());
            Assert.AreEqual(true, value.GetValue<bool>(2));
            Assert.IsNotNull(value.GetValue(3));
            Assert.IsTrue(value.GetValue(3).IsBoolean);
            Assert.AreEqual(false, value.GetValue(3).GetValue<bool>());
            Assert.AreEqual(false, value.GetValue<bool>(3));
            Assert.IsNotNull(value.GetValue(4));
            Assert.IsTrue(value.GetValue(4).IsNull);
            Assert.IsNotNull(value.GetValue(5));
            Assert.IsTrue(value.GetValue(5).IsObject);
            Assert.IsNotNull(value.GetValue(6));
            Assert.IsTrue(value.GetValue(6).IsArray);
        }

        [Test]
        public void GetNullableValue()
        {
            JsonValue value1 = JsonValue.Parse("123");
            Assert.IsTrue(value1.IsNumber);
            Assert.AreEqual(123, value1.GetValue<int?>());

            JsonValue value2 = JsonValue.Parse("null");
            Assert.IsTrue(value2.IsNull);
            Assert.AreEqual(null, value2.GetValue<int?>());
        }

        [Test]
        [ExpectedException(typeof(JsonException),
            ExpectedMessage = "Could not cast JSON string value to type 'System.Double'.")]
        public void GetValueWithInvalidCastError()
        {
            JsonValue value = JsonValue.Parse("\"abc\"");

            Assert.IsTrue(value.IsString);
            value.GetValue<double>();
        }

        [Test]
        public void GetObjectValueOnJsonObject()
        {
            JsonValue value = JsonValue.Parse("{\"Name\":\"Value\"}");

            Assert.IsTrue(value.IsObject);
            Assert.IsNull(value.GetValue("Bla"));
            JsonException ex = Assert.Throws<JsonException>(delegate() { value.GetValue<string>("Bla"); });
            Assert.AreEqual("The JSON object structure does not have an entry named 'Bla'.", ex.Message);
        }

        [Test]
        public void GetObjectValueOrDefaultValueOnJsonObject()
        {
            JsonValue value = JsonValue.Parse("{\"Name\":\"Value\"}");

            Assert.IsTrue(value.IsObject);
            Assert.IsNull(value.GetValue("Bla"));
            Assert.AreEqual("Value", value.GetValueOrDefault<string>("Name"));
            Assert.AreEqual(null, value.GetValueOrDefault<string>("Bla"));
            Assert.AreEqual(String.Empty, value.GetValueOrDefault<string>("Bla", String.Empty));
            Assert.AreEqual(0, value.GetValueOrDefault<int>("Gla"));
            Assert.AreEqual(-1, value.GetValueOrDefault<int>("Gla", -1));
        }

        [Test]
        [ExpectedException(
            typeof(JsonException),
            ExpectedMessage = "The value held by this instance is not a JSON object structure.")]
        public void GetObjectValueOnNonJsonObject()
        {
            JsonValue value = JsonValue.Parse("[]");

            Assert.IsFalse(value.IsObject);
            value.GetValue("Bla");
        }

        [Test]
        public void GetArrayValueOnJsonArray()
        {
            JsonValue value = JsonValue.Parse("[1, 2]");

            Assert.IsTrue(value.IsArray);
            Assert.IsNull(value.GetValue(7));
            JsonException ex = Assert.Throws<JsonException>(delegate() { value.GetValue<int>(7); });
            Assert.AreEqual("The JSON array structure does not have an entry at index '7'.", ex.Message);
        }

        [Test]
        public void GetArrayValueOrDefaultValueOnJsonArray()
        {
            JsonValue value = JsonValue.Parse("[1, 2]");

            Assert.IsTrue(value.IsArray);
            Assert.IsNull(value.GetValue(7));
            Assert.AreEqual(2, value.GetValueOrDefault<int>(1));
            Assert.AreEqual(null, value.GetValueOrDefault<string>(3));
            Assert.AreEqual(String.Empty, value.GetValueOrDefault<string>(3, String.Empty));
            Assert.AreEqual(0, value.GetValueOrDefault<int>(4));
            Assert.AreEqual(-1, value.GetValueOrDefault<int>(4, -1));            
        }

        [Test]
        [ExpectedException(
            typeof(JsonException),
            ExpectedMessage = "The value held by this instance is not a JSON array structure.")]
        public void GetArrayValueOnNonJsonArray()
        {
            JsonValue value = JsonValue.Parse("{}");

            Assert.IsFalse(value.IsArray);
            value.GetValue(7);
        }

        [Test]
        public void GetValuesOnJsonObject()
        {
            JsonValue value = JsonValue.Parse("{\"Name\":{}, \"Age\":31}");

            Assert.IsTrue(value.IsObject);

            Assert.AreEqual(2, value.GetValues().Count);
            // TODO

            Assert.AreEqual(0, value.GetValues("Name").Count);

            JsonException ex1 = Assert.Throws<JsonException>(delegate() { value.GetValues("Bla"); });
            Assert.AreEqual("The JSON object structure does not have an entry named 'Bla'.", ex1.Message);

            JsonException ex2 = Assert.Throws<JsonException>(delegate() { value.GetValues("Age"); });
            Assert.AreEqual("The value held by this instance is not a JSON object or array structure.", ex2.Message);
        }

        [Test]
        public void GetValuesOnJsonArray()
        {
            JsonValue value = JsonValue.Parse("[7, {\"Age\":31}]");

            Assert.IsTrue(value.IsArray);

            Assert.AreEqual(2, value.GetValues().Count);
            // TODO

            Assert.AreEqual(1, value.GetValues(1).Count);

            JsonException ex1 = Assert.Throws<JsonException>(delegate() { value.GetValues(4); });
            Assert.AreEqual("The JSON array structure does not have an entry at index '4'.", ex1.Message);

            JsonException ex2 = Assert.Throws<JsonException>(delegate() { value.GetValues(0); });
            Assert.AreEqual("The value held by this instance is not a JSON object or array structure.", ex2.Message);
        }

        [Test]
        [ExpectedException(
            typeof(JsonException),
            ExpectedMessage = "The value held by this instance is not a JSON object or array structure.")]
        public void GetValuesOnNonJsonObjectOrArray()
        {
            JsonValue value = JsonValue.Parse("123");

            Assert.IsTrue(value.IsNumber);
            value.GetValues();
        }

        [Test]
        public void GetNamesOnJsonObject()
        {
            JsonValue value = JsonValue.Parse("{\"Name\":{}, \"Age\":31}");

            Assert.IsTrue(value.IsObject);
            Assert.AreEqual(2, value.GetNames().Count);
        }

        [Test]
        [ExpectedException(
            typeof(JsonException),
            ExpectedMessage = "The value held by this instance is not a JSON object structure.")]
        public void GetNamesOnNonJsonObject()
        {
            JsonValue value = JsonValue.Parse("[]");

            Assert.IsFalse(value.IsObject);
            value.GetNames();
        }

        [Test]
        public void ContainsNameOnJsonObject()
        {
            JsonValue value = JsonValue.Parse("{\"Name\":{}, \"Age\":31}");

            Assert.IsTrue(value.IsObject);
            Assert.IsTrue(value.ContainsName("Name"));
            Assert.IsFalse(value.ContainsName("Location"));
        }

        [Test]
        [ExpectedException(
            typeof(JsonException),
            ExpectedMessage = "The value held by this instance is not a JSON object structure.")]
        public void ContainsNameOnNonJsonObject()
        {
            JsonValue value = JsonValue.Parse("[]");

            Assert.IsFalse(value.IsObject);
            value.ContainsName("Name");
        }
    }
}