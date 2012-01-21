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

namespace Spring.Json
{
    /// <summary>
    /// An interface to deserialize object of arbitrary type from JSON.
    /// </summary>
    /// <author>Bruno Baia</author>
    public interface IJsonDeserializer
    {
        /// <summary>
        /// Deserialize the specified <paramref name="value"/> to a custom object.
        /// </summary>
        /// <param name="value">The <see cref="JsonValue"/> to deserialize.</param>
        /// <param name="mapper">The <see cref="JsonMapper"/> to use.</param>
        /// <returns>An object created from the passed <see cref="JsonValue"/>.</returns>
        object Deserialize(JsonValue value, JsonMapper mapper);
    }
}
