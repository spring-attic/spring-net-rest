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
using System.Collections;
using System.Collections.Generic;

using Spring.Util;

namespace Spring.Json
{
    /// <summary>
    /// Represents a JSON object structure. 
    /// A JSON object is a collection of name/value pairs.
    /// </summary>
    /// <remarks>
    /// Specification details, see http://www.json.org/
    /// </remarks>
    /// <seealso cref="JsonValue"/>
    /// <author>Bruno Baia</author>
    public class JsonObject : JsonValue
    {
        /// <summary>
        /// Creates a new instance of <see cref="JsonObject"/>.
        /// </summary>
        public JsonObject() :
            base(JsonValueType.Object, new Dictionary<string, JsonValue>())
        {
        }

        #region Public methods

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
        public override JsonValue GetValue(string name)
        {
            ArgumentUtils.AssertNotNull(name, "name");

            IDictionary<string, JsonValue> jsonObject = (IDictionary<string, JsonValue>)this.value;
            JsonValue result;
            if (jsonObject.TryGetValue(name, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Returns all <see cref="JsonValue"/>s 
        /// if the value held by this instance is a JSON object or array structure.
        /// </summary>
        /// <returns>The collection of <see cref="JsonValue"/>s.</returns>
        /// <exception cref="JsonException">If the value held by this instance is not a JSON object or array structure.</exception>
        public override ICollection<JsonValue> GetValues()
        {
            return ((IDictionary<string, JsonValue>)this.value).Values;
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
        public override bool ContainsName(string name)
        {
            return ((IDictionary<string, JsonValue>)this.value).ContainsKey(name);
        }

        /// <summary>
        /// Returns all entry names 
        /// if the value held by this instance is a JSON object structure.
        /// </summary>
        /// <returns>The collection of entry names.</returns>
        /// <exception cref="JsonException">If the value held by this instance is not a JSON object structure.</exception>
        public override ICollection<string> GetNames()
        {
            return ((IDictionary<string, JsonValue>)this.value).Keys;
        }

        /// <summary>
        /// Adds the specified name/<see cref="JsonValue"/> pair 
        /// to the JSON object structure held by this instance.
        /// </summary>
        /// <param name="name">The name of the entry to add.</param>
        /// <param name="value">The <see cref="JsonValue"/> of the entry to add.</param>
        /// <exception cref="JsonException">
        /// If the value held by this instance is not a JSON object structure or 
        /// if an entry with the same name already exists.
        /// </exception>
        public void AddValue(string name, JsonValue value)
        {
            ArgumentUtils.AssertNotNull(name, "name");
            ArgumentUtils.AssertNotNull(value, "value");

            IDictionary<string, JsonValue> jsonObject = (IDictionary<string, JsonValue>)this.value;
            if (jsonObject.ContainsKey(name))
            {
                throw new JsonException(String.Format(
                    "An entry with the name '{0}' already exists in the JSON object structure.",
                    name));
            }
            jsonObject.Add(name, value);
        }

        #endregion
    }
}