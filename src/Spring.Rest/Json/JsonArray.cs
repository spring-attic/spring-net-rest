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

using System.Collections;
using System.Collections.Generic;

using Spring.Util;

namespace Spring.Json
{
    /// <summary>
    /// Defines a JSON array structure. 
    /// A JSON array is a list of <see cref="JsonValue"/>s.
    /// </summary>
    /// <remarks>
    /// Specification details, see http://www.json.org/
    /// </remarks>
    /// <seealso cref="JsonValue"/>
    /// <author>Bruno Baia</author>
    public class JsonArray : JsonValue
    {
        /// <summary>
        /// Creates a new instance of <see cref="JsonArray"/>.
        /// </summary>
        public JsonArray(params JsonValue[] values) :
            base(JsonValueType.Array, new List<JsonValue>(values))
        {
        }

        #region Public methods

        /// <summary>
        /// Returns the <see cref="JsonValue"/> at the specified entry index.
        /// </summary>
        /// <param name="index">The zero-based index of the entry that contains the value to get.</param>
        /// <returns>
        /// The <see cref="JsonValue"/> at the specified index 
        /// or <see langword="null"/> if the entry does not exists.
        /// </returns>
        public override JsonValue GetValue(int index)
        {
            IList<JsonValue> jsonArray = (IList<JsonValue>)this.value;
            if (index < jsonArray.Count)
            {
                return jsonArray[index];
            }
            return null;
        }

        /// <summary>
        /// Returns all <see cref="JsonValue"/>s of the JSON array structure.
        /// </summary>
        /// <returns>The collection of <see cref="JsonValue"/>s.</returns>
        public override ICollection<JsonValue> GetValues()
        {
            return (IList<JsonValue>)this.value;
        }

        /// <summary>
        /// Adds a <see cref="JsonValue"/> to the end of the JSON array structure.
        /// </summary>
        /// <param name="value">The <see cref="JsonValue"/> to add.</param>
        public void AddValue(JsonValue value)
        {
            ArgumentUtils.AssertNotNull(value, "value");

            ((IList<JsonValue>)this.value).Add(value);
        }

        #endregion
    }
}