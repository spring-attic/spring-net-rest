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

// Parsing code based on SimpleJson 0.8 http://simplejson.codeplex.com/
// http://bit.ly/simplejson
// License: Apache License 2.0 (Apache)
// original json parsing code from http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html

using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using Spring.Util;

namespace Spring.Json
{
    /// <summary>
    /// A generic container for JSON content. 
    /// Every JSON value types (object, array, string, number, true, false or null) can be accessed via the provided functions.
    /// </summary>
    /// <remarks>
    /// Specification details, see http://www.json.org/
    /// </remarks>
    /// <seealso cref="JsonObject"/>
    /// <seealso cref="JsonArray"/>
    /// <author>Bruno Baia</author>
    public class JsonValue
    {
        /// <summary>
        /// Specifies the JSON value types.
        /// </summary>
        protected enum JsonValueType
        {
            /// <summary>
            /// The JSON string.
            /// </summary>
            String,
            /// <summary>
            /// The JSON number.
            /// </summary>
            Number,
            /// <summary>
            /// The JSON object.
            /// </summary>
            Object,
            /// <summary>
            /// The JSON array.
            /// </summary>
            Array,
            /// <summary>
            /// The JSON boolean.
            /// </summary>
            Boolean,
            /// <summary>
            /// The JSON null.
            /// </summary>
            Null
        }

        /// <summary>
        /// The value held by this instance.
        /// </summary>
        protected object value;

        /// <summary>
        /// The JSON type of the value held by this instance.
        /// </summary>
        protected JsonValueType type;

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not this instance holds a JSON object structure.
        /// </summary>
        public bool IsObject
        {
            get { return this.type == JsonValueType.Object; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this instance holds a JSON array structure.
        /// </summary>
        public bool IsArray
        {
            get { return this.type == JsonValueType.Array; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this instance holds a JSON string.
        /// </summary>
        public bool IsString
        {
            get { return this.type == JsonValueType.String; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this instance holds a JSON number.
        /// </summary>
        public bool IsNumber
        {
            get { return this.type == JsonValueType.Number; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this instance holds a JSON boolean.
        /// </summary>
        public bool IsBoolean
        {
            get { return this.type == JsonValueType.Boolean; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this instance holds a JSON null value.
        /// </summary>
        public bool IsNull
        {
            get { return this.type == JsonValueType.Null; }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON null value.
        /// </summary>
        public JsonValue()
        {
            this.type = JsonValueType.Null;
            this.value = null;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public JsonValue(string value)
        {
            ArgumentUtils.AssertNotNull(value, "value");

            this.type = JsonValueType.String;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        public JsonValue(bool value)
        {
            this.type = JsonValueType.Boolean;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON number value.
        /// </summary>
        /// <param name="value">The number value.</param>
        public JsonValue(byte value)
        {
            this.type = JsonValueType.Number;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON number value.
        /// </summary>
        /// <param name="value">The number value.</param>
        public JsonValue(decimal value)
        {
            this.type = JsonValueType.Number;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON number value.
        /// </summary>
        /// <param name="value">The number value.</param>
        public JsonValue(double value)
        {
            this.type = JsonValueType.Number;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON number value.
        /// </summary>
        /// <param name="value">The number value.</param>
        public JsonValue(float value)
        {
            this.type = JsonValueType.Number;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON number value.
        /// </summary>
        /// <param name="value">The number value.</param>
        public JsonValue(int value)
        {
            this.type = JsonValueType.Number;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON number value.
        /// </summary>
        /// <param name="value">The number value.</param>
        public JsonValue(long value)
        {
            this.type = JsonValueType.Number;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> that holds a JSON number value.
        /// </summary>
        /// <param name="value">The number value.</param>
        public JsonValue(short value)
        {
            this.type = JsonValueType.Number;
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonValue"/> with the the given value and type.
        /// </summary>
        /// <param name="type">The JSON type.</param>
        /// <param name="value">The JSON value.</param>
        protected JsonValue(JsonValueType type, object value)
        {
            this.type = type;
            this.value = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parses the JSON string representation. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="json">A JSON string to parse.</param>
        /// <param name="result">
        /// If the conversion succeed, contains the <see cref="JsonValue"/> equivalent to the passed string, 
        /// or null if the conversion failed. 
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the passed string was parsed successfully; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string json, out JsonValue result)
        {
            result = null;
            if (json == null)
            {
                return true;
            }
            char[] charArray = json.ToCharArray();
            int index = 0;
            bool success = true;
            JsonValue simpleJsonObject = JsonParser.ParseValue(charArray, ref index, ref success);
            if (success)
            {
                result = simpleJsonObject;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Parses the JSON string representation.
        /// </summary>
        /// <param name="json">A JSON string to parse.</param>
        /// <returns>A <see cref="JsonValue"/> equivalent to the passed string.</returns>
        /// <exception cref="JsonException"><paramref name="json"/> is not in the correct format.</exception>
        public static JsonValue Parse(string json)
        {
            JsonValue result;
            if (TryParse(json, out result))
            {
                return result;
            }
            throw new JsonException(String.Format("Could not parse JSON string '{0}'.", json));
        }

        /// <summary>
        /// Converts this instance to its JSON string representation.
        /// </summary>
        /// <returns>The JSON string representation of this instance.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(2000);
            JsonGenerator.GenerateValue(this, builder);
            return builder.ToString();
        }

        /// <summary>
        /// Returns the value held by this instance converted to type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The value held by this instance.</returns>
        /// <exception cref="JsonException">If the value held by this instance could not be converted to type <typeparamref name="T"/>.</exception>
        public T GetValue<T>()
        {
            if (this.value == null)
            {
                return default(T);
            }
            if (this.value is T)
            {
                return (T)this.value;
            }
            Type conversionType = typeof(T);
            // Manage Nullable types
            if (conversionType.IsGenericType &&
               (conversionType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                conversionType = Nullable.GetUnderlyingType(conversionType);
            }
            try
            {
                return (T)Convert.ChangeType(this.value, conversionType, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new JsonException(String.Format("Could not cast JSON {0} value to type '{1}'.", this.type.ToString().ToLower(), conversionType), ex);
            }
        }


        /// <summary>
        /// Returns the <see cref="JsonValue"/> associated with the specified entry name 
        /// if the value held by this instance is a JSON object structure.
        /// </summary>
        /// <param name="name">The name of the entry that contains the value to get.</param>
        /// <returns>
        /// The <see cref="JsonValue"/> associated with the specified name 
        /// or <see langword="null"/> if the entry does not exists.
        /// </returns>
        /// <exception cref="JsonException">If the value held by this instance is not a JSON object structure.</exception>
        public virtual JsonValue GetValue(string name)
        {
            throw new JsonException("The value held by this instance is not a JSON object structure.");
        }

        /// <summary>
        /// Returns the <see cref="JsonValue"/> at the specified entry index 
        /// if the value held by this instance is a JSON array structure.
        /// </summary>
        /// <param name="index">The zero-based index of the entry that contains the value to get.</param>
        /// <returns>
        /// The <see cref="JsonValue"/> at the specified index 
        /// or <see langword="null"/> if the entry does not exists.
        /// </returns>
        /// <exception cref="JsonException">If the value held by this instance is not a JSON array structure.</exception>
        public virtual JsonValue GetValue(int index)
        {
            throw new JsonException("The value held by this instance is not a JSON array structure.");
        }

        /// <summary>
        /// Returns the value associated with the specified entry name 
        /// if the value held by this instance is a JSON object structure.
        /// </summary>
        /// <remarks>
        /// Equivalent to: <code>GetValue(name).Get&lt;T>()</code>
        /// </remarks>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="name">The name of the entry that contains the value to get.</param>
        /// <returns>The value associated with the specified name.</returns>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON object structure or 
        /// if the entry with the specified name does not exists.
        /// </exception>
        public T GetValue<T>(string name)
        {
            JsonValue jsonValue = this.GetValue(name);
            if (jsonValue != null)
            {
                return jsonValue.GetValue<T>();
            }
            throw new JsonException(String.Format(
                "The JSON object structure does not have an entry named '{0}'.",
                name));
        }

        /// <summary>
        /// Returns the value associated with the specified entry name, or the object's default value, 
        /// if the value held by this instance is a JSON object structure.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="name">The name of the entry that contains the value to get.</param>
        /// <returns>The value associated with the specified name.</returns>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON object structure.
        /// </exception>
        public T GetValueOrDefault<T>(string name)
        {
            return this.GetValueOrDefault<T>(name, default(T));
        }

        /// <summary>
        /// Returns the value associated with the specified entry name, or the specified default value, 
        /// if the value held by this instance is a JSON object structure.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="name">The name of the entry that contains the value to get.</param>
        /// <param name="defaultValue">The default value to return if the entry does not exists.</param>
        /// <returns>The value associated with the specified name if the entry does not exists.</returns>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON object structure.
        /// </exception>
        public T GetValueOrDefault<T>(string name, T defaultValue)
        {
            JsonValue jsonValue = this.GetValue(name);
            if (jsonValue != null)
            {
                return jsonValue.GetValue<T>();
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the value at the specified entry index 
        /// if the value held by this instance is a JSON array structure.
        /// </summary>
        /// <remarks>
        /// Equivalent to: <code>GetValue(index).Get&lt;T>()</code>
        /// </remarks>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="index">The zero-based index of the entry that contains the value to get.</param>
        /// <returns>The value at the specified index.</returns>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON array structure or
        /// if the entry at the specified index does not exists.
        /// </exception>
        public T GetValue<T>(int index)
        {
            JsonValue jsonValue = this.GetValue(index);
            if (jsonValue != null)
            {
                return jsonValue.GetValue<T>();
            }
            throw new JsonException(String.Format(
                "The JSON array structure does not have an entry at index '{0}'.",
                index));
        }

        /// <summary>
        /// Returns the value at the specified entry index, or the object's default value, 
        /// if the value held by this instance is a JSON array structure.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="index">The zero-based index of the entry that contains the value to get.</param>
        /// <returns>The value at the specified index.</returns>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON array structure.
        /// </exception>
        public T GetValueOrDefault<T>(int index)
        {
            return this.GetValueOrDefault<T>(index, default(T));
        }

        /// <summary>
        /// Returns the value at the specified entry index, or the specified default value, 
        /// if the value held by this instance is a JSON array structure.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="index">The zero-based index of the entry that contains the value to get.</param>
        /// <param name="defaultValue">The default value to return if the entry does not exists.</param>
        /// <returns>The value at the specified index.</returns>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON array structure.
        /// </exception>
        public T GetValueOrDefault<T>(int index, T defaultValue)
        {
            JsonValue jsonValue = this.GetValue(index);
            if (jsonValue != null)
            {
                return jsonValue.GetValue<T>();
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns all <see cref="JsonValue"/>s 
        /// if the value held by this instance is a JSON object or array structure.
        /// </summary>
        /// <returns>The collection of <see cref="JsonValue"/>s.</returns>
        /// <exception cref="JsonException">If the value held by this instance is not a JSON object or array structure.</exception>
        public virtual ICollection<JsonValue> GetValues()
        {
            throw new JsonException("The value held by this instance is not a JSON object or array structure.");
        }

        /// <summary>
        /// Returns all <see cref="JsonValue"/>s associated with the specified entry name 
        /// if the value held by this instance is a JSON object structure.
        /// </summary>
        /// <remarks>
        /// Equivalent to: <code>GetValue(name).GetValues()</code>
        /// </remarks>
        /// <param name="name">The name of the entry that contains the values to get.</param>
        /// <returns>The collection of <see cref="JsonValue"/>s.</returns>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON object structure or 
        /// if the entry with the specified name does not exists.
        /// </exception>
        public ICollection<JsonValue> GetValues(string name)
        {
            JsonValue jsonValue = this.GetValue(name);
            if (jsonValue != null)
            {
                return jsonValue.GetValues();
            }
            throw new JsonException(String.Format(
                "The JSON object structure does not have an entry named '{0}'.",
                name));
        }

        /// <summary>
        /// Returns all <see cref="JsonValue"/>s at the specified entry index 
        /// if the value held by this instance is a JSON array structure.
        /// </summary>
        /// <remarks>
        /// Equivalent to: <code>GetValue(index).GetValues()</code>
        /// </remarks>
        /// <param name="index">The zero-based index of the entry that contains the values to get.</param>
        /// <returns>The collection of <see cref="JsonValue"/>s.</returns>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON array structure or
        /// if the entry at the specified index does not exists.
        /// </exception>
        public ICollection<JsonValue> GetValues(int index)
        {
            JsonValue jsonValue = this.GetValue(index);
            if (jsonValue != null)
            {
                return jsonValue.GetValues();
            }
            throw new JsonException(String.Format(
                "The JSON array structure does not have an entry at index '{0}'.",
                index));
        }

        /// <summary>
        /// If the value held by this instance is a JSON object structure, 
        /// indicates whether or not it contains the specified entry name.
        /// </summary>
        /// <param name="name">The name of the entry to search for.</param>
        /// <returns>
        /// <see langword="true"/> if this JSON object contains the specified entry name; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="JsonException">If the value held by this instance is not a JSON object structure.</exception>
        public virtual bool ContainsName(string name)
        {
            throw new JsonException("The value held by this instance is not a JSON object structure.");
        }

        /// <summary>
        /// Returns all entry names 
        /// if the value held by this instance is a JSON object structure.
        /// </summary>
        /// <returns>The collection of entry names.</returns>
        /// <exception cref="JsonException">If the value held by this instance is not a JSON object structure.</exception>
        public virtual ICollection<string> GetNames()
        {
            throw new JsonException("The value held by this instance is not a JSON object structure.");
        }

        #endregion

        #region JSON parsing & generation

        static class JsonParser
        {
            private const int TOKEN_NONE = 0;
            private const int TOKEN_CURLY_OPEN = 1;
            private const int TOKEN_CURLY_CLOSE = 2;
            private const int TOKEN_SQUARED_OPEN = 3;
            private const int TOKEN_SQUARED_CLOSE = 4;
            private const int TOKEN_COLON = 5;
            private const int TOKEN_COMMA = 6;
            private const int TOKEN_STRING = 7;
            private const int TOKEN_NUMBER = 8;
            private const int TOKEN_TRUE = 9;
            private const int TOKEN_FALSE = 10;
            private const int TOKEN_NULL = 11;

            public static JsonValue ParseValue(char[] json, ref int index, ref bool success)
            {
                switch (LookAhead(json, index))
                {
                    case TOKEN_STRING:
                        return new JsonValue(ParseString(json, ref index, ref success));
                    case TOKEN_NUMBER:
                        return new JsonValue(JsonValueType.Number, ParseNumber(json, ref index, ref success));
                    case TOKEN_CURLY_OPEN:
                        return ParseObject(json, ref index, ref success);
                    case TOKEN_SQUARED_OPEN:
                        return ParseArray(json, ref index, ref success);
                    case TOKEN_TRUE:
                        NextToken(json, ref index);
                        return new JsonValue(true);
                    case TOKEN_FALSE:
                        NextToken(json, ref index);
                        return new JsonValue(false);
                    case TOKEN_NULL:
                        NextToken(json, ref index);
                        return new JsonValue();
                    case TOKEN_NONE:
                        break;
                }

                success = false;
                return null;
            }

            private static JsonObject ParseObject(char[] json, ref int index, ref bool success)
            {
                JsonObject jsonObject = new JsonObject();
                int token;

                // {
                NextToken(json, ref index);

                bool done = false;
                while (!done)
                {
                    token = LookAhead(json, index);
                    if (token == TOKEN_NONE)
                    {
                        success = false;
                        return null;
                    }
                    else if (token == TOKEN_COMMA)
                        NextToken(json, ref index);
                    else if (token == TOKEN_CURLY_CLOSE)
                    {
                        NextToken(json, ref index);
                        return jsonObject;
                    }
                    else
                    {
                        // name
                        string name = ParseString(json, ref index, ref success);
                        if (!success)
                        {
                            success = false;
                            return null;
                        }

                        // :
                        token = NextToken(json, ref index);
                        if (token != TOKEN_COLON)
                        {
                            success = false;
                            return null;
                        }

                        // value
                        JsonValue value = ParseValue(json, ref index, ref success);
                        if (!success)
                        {
                            success = false;
                            return null;
                        }

                        jsonObject.AddValue(name, value);
                    }
                }

                return jsonObject;
            }

            private static JsonArray ParseArray(char[] json, ref int index, ref bool success)
            {
                JsonArray jsonArray = new JsonArray();

                // [
                NextToken(json, ref index);

                bool done = false;
                while (!done)
                {
                    int token = LookAhead(json, index);
                    if (token == TOKEN_NONE)
                    {
                        success = false;
                        return null;
                    }
                    else if (token == TOKEN_COMMA)
                        NextToken(json, ref index);
                    else if (token == TOKEN_SQUARED_CLOSE)
                    {
                        NextToken(json, ref index);
                        break;
                    }
                    else
                    {
                        JsonValue value = ParseValue(json, ref index, ref success);
                        if (!success)
                            return null;
                        jsonArray.AddValue(value);
                    }
                }

                return jsonArray;
            }

            private static string ParseString(char[] json, ref int index, ref bool success)
            {
                StringBuilder s = new StringBuilder();
                char c;

                EatWhitespace(json, ref index);

                // "
                c = json[index++];

                bool complete = false;
                while (!complete)
                {
                    if (index == json.Length)
                    {
                        break;
                    }

                    c = json[index++];
                    if (c == '"')
                    {
                        complete = true;
                        break;
                    }
                    else if (c == '\\')
                    {
                        if (index == json.Length)
                            break;
                        c = json[index++];
                        if (c == '"')
                            s.Append('"');
                        else if (c == '\\')
                            s.Append('\\');
                        else if (c == '/')
                            s.Append('/');
                        else if (c == 'b')
                            s.Append('\b');
                        else if (c == 'f')
                            s.Append('\f');
                        else if (c == 'n')
                            s.Append('\n');
                        else if (c == 'r')
                            s.Append('\r');
                        else if (c == 't')
                            s.Append('\t');
                        else if (c == 'u')
                        {
                            int remainingLength = json.Length - index;
                            if (remainingLength >= 4)
                            {
                                // parse the 32 bit hex into an integer codepoint
                                uint codePoint;
                                if (
                                    !(success =
                                      TryParseUInt32(new string(json, index, 4), NumberStyles.HexNumber,
                                                      CultureInfo.InvariantCulture, out codePoint)))
                                    return "";

                                // convert the integer codepoint to a unicode char and add to string
                                if (0xD800 <= codePoint && codePoint <= 0xDBFF) // if high surrogate
                                {
                                    index += 4; // skip 4 chars
                                    remainingLength = json.Length - index;
                                    if (remainingLength >= 6)
                                    {
                                        uint lowCodePoint;
                                        if (new string(json, index, 2) == "\\u" &&
                                            TryParseUInt32(new string(json, index + 2, 4), NumberStyles.HexNumber,
                                                            CultureInfo.InvariantCulture, out lowCodePoint))
                                        {
                                            if (0xDC00 <= lowCodePoint && lowCodePoint <= 0xDFFF) // if low surrogate
                                            {
                                                s.Append((char)codePoint);
                                                s.Append((char)lowCodePoint);
                                                index += 6; // skip 6 chars
                                                continue;
                                            }
                                        }
                                    }
                                    success = false; // invalid surrogate pair
                                    return "";
                                }
#if SILVERLIGHT || CF_3_5
                                s.Append(ConvertFromUtf32((int)codePoint));
#else
                                s.Append(Char.ConvertFromUtf32((int)codePoint));
#endif
                                // skip 4 chars
                                index += 4;
                            }
                            else
                                break;
                        }
                    }
                    else
                        s.Append(c);
                }

                if (!complete)
                {
                    success = false;
                    return null;
                }

                return s.ToString();
            }

            private static bool TryParseUInt32(string s, NumberStyles style, IFormatProvider provider, out uint result)
            {
#if CF_3_5
                try
                {
                    result = UInt32.Parse(s, style, provider);
                    return true;
                }
                catch
                {
                    result = 0;
                    return false;
                }
#else
                return UInt32.TryParse(s, style, provider, out result);
#endif
            }

#if SILVERLIGHT || CF_3_5
            private static string ConvertFromUtf32(int utf32)
            {
                // http://www.java2s.com/Open-Source/CSharp/2.6.4-mono-.net-core/System/System/Char.cs.htm
                if (utf32 < 0 || utf32 > 0x10FFFF)
                    throw new ArgumentOutOfRangeException("utf32", "The argument must be from 0 to 0x10FFFF.");
                if (0xD800 <= utf32 && utf32 <= 0xDFFF)
                    throw new ArgumentOutOfRangeException("utf32", "The argument must not be in surrogate pair range.");
                if (utf32 < 0x10000)
                    return new string((char)utf32, 1);
                utf32 -= 0x10000;
                return new string(new char[] {(char) ((utf32 >> 10) + 0xD800),(char) (utf32 % 0x0400 + 0xDC00)});
            }
#endif

            private static object ParseNumber(char[] json, ref int index, ref bool success)
            {
                EatWhitespace(json, ref index);

                int lastIndex = GetLastIndexOfNumber(json, index);
                int charLength = (lastIndex - index) + 1;

                string returnNumber = new string(json, index, charLength);
                index = lastIndex + 1;
                return returnNumber;
            }

            private static int GetLastIndexOfNumber(char[] json, int index)
            {
                int lastIndex;
                for (lastIndex = index; lastIndex < json.Length; lastIndex++)
                {
                    if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1)
                    {
                        break;
                    }
                }
                return lastIndex - 1;
            }

            private static void EatWhitespace(char[] json, ref int index)
            {
                for (; index < json.Length; index++)
                {
                    if (" \t\n\r\b\f".IndexOf(json[index]) == -1)
                    {
                        break;
                    }
                }
            }

            private static int LookAhead(char[] json, int index)
            {
                int saveIndex = index;
                return NextToken(json, ref saveIndex);
            }

            private static int NextToken(char[] json, ref int index)
            {
                EatWhitespace(json, ref index);

                if (index == json.Length)
                {
                    return TOKEN_NONE;
                }

                char c = json[index];
                index++;
                switch (c)
                {
                    case '{':
                        return TOKEN_CURLY_OPEN;
                    case '}':
                        return TOKEN_CURLY_CLOSE;
                    case '[':
                        return TOKEN_SQUARED_OPEN;
                    case ']':
                        return TOKEN_SQUARED_CLOSE;
                    case ',':
                        return TOKEN_COMMA;
                    case '"':
                        return TOKEN_STRING;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return TOKEN_NUMBER;
                    case ':':
                        return TOKEN_COLON;
                }
                index--;

                int remainingLength = json.Length - index;

                // false
                if (remainingLength >= 5)
                {
                    if (json[index] == 'f' &&
                        json[index + 1] == 'a' &&
                        json[index + 2] == 'l' &&
                        json[index + 3] == 's' &&
                        json[index + 4] == 'e')
                    {
                        index += 5;
                        return TOKEN_FALSE;
                    }
                }

                // true
                if (remainingLength >= 4)
                {
                    if (json[index] == 't' &&
                        json[index + 1] == 'r' &&
                        json[index + 2] == 'u' &&
                        json[index + 3] == 'e')
                    {
                        index += 4;
                        return TOKEN_TRUE;
                    }
                }

                // null
                if (remainingLength >= 4)
                {
                    if (json[index] == 'n' &&
                        json[index + 1] == 'u' &&
                        json[index + 2] == 'l' &&
                        json[index + 3] == 'l')
                    {
                        index += 4;
                        return TOKEN_NULL;
                    }
                }

                return TOKEN_NONE;
            }
        }

        static class JsonGenerator
        {
            public static void GenerateValue(JsonValue jsonValue, StringBuilder builder)
            {
                switch (jsonValue.type)
                {
                    case JsonValueType.String:
                        {
                            GenerateString((string)jsonValue.value, builder);
                            break;
                        }
                    case JsonValueType.Object:
                        {
                            GenerateObject((IDictionary<string, JsonValue>)jsonValue.value, builder);
                            break;
                        }
                    case JsonValueType.Array:
                        {
                            GenerateArray((IList<JsonValue>)jsonValue.value, builder);
                            break;
                        }
                    case JsonValueType.Number:
                        {
                            GenerateNumber(jsonValue.value, builder);
                            break;
                        }
                    case JsonValueType.Boolean:
                        {
                            builder.Append((bool)jsonValue.value ? "true" : "false");
                            break;
                        }
                    case JsonValueType.Null:
                        {
                            builder.Append("null");
                            break;
                        }
                }
            }

            private static void GenerateObject(IDictionary<string, JsonValue> jsonObject, StringBuilder builder)
            {
                builder.Append("{");

                bool first = true;
                foreach (KeyValuePair<string, JsonValue> keyValuePair in jsonObject)
                {
                    if (!first)
                    {
                        builder.Append(",");
                    }
                    GenerateString(keyValuePair.Key, builder);
                    builder.Append(":");
                    GenerateValue(keyValuePair.Value, builder);
                    first = false;
                }

                builder.Append("}");
            }

            private static void GenerateArray(IList<JsonValue> jsonArray, StringBuilder builder)
            {
                builder.Append("[");

                bool first = true;
                foreach (JsonValue value in jsonArray)
                {
                    if (!first)
                    {
                        builder.Append(",");
                    }
                    GenerateValue(value, builder);
                    first = false;
                }

                builder.Append("]");
            }

            private static void GenerateString(string jsonString, StringBuilder builder)
            {
                builder.Append("\"");

                char[] charArray = jsonString.ToCharArray();
                for (int i = 0; i < charArray.Length; i++)
                {
                    char c = charArray[i];
                    if (c == '"')
                        builder.Append("\\\"");
                    else if (c == '\\')
                        builder.Append("\\\\");
                    else if (c == '\b')
                        builder.Append("\\b");
                    else if (c == '\f')
                        builder.Append("\\f");
                    else if (c == '\n')
                        builder.Append("\\n");
                    else if (c == '\r')
                        builder.Append("\\r");
                    else if (c == '\t')
                        builder.Append("\\t");
                    else
                        builder.Append(c);
                }

                builder.Append("\"");
            }

            private static void GenerateNumber(object jsonNumber, StringBuilder builder)
            {
                builder.Append(Convert.ToString(jsonNumber, CultureInfo.InvariantCulture));
            }
        }

        #endregion
    }
}