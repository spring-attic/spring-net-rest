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
using System.Collections.Generic;

namespace Spring.Json
{
    /// <summary>
    /// Data binder that provides functionality for converting between 
    /// objects of arbitrary types and <see cref="JsonValue"/> instances.
    /// </summary>
    /// <author>Bruno Baia</author>
    public class JsonMapper
    {
        private IDictionary<Type, IJsonDeserializer> deserializers;
        private IDictionary<Type, IJsonSerializer> serializers;

        /// <summary>
        /// Creates a new empty instance of <see cref="JsonMapper"/>.
        /// </summary>
        public JsonMapper()
        {
            this.deserializers = new Dictionary<Type, IJsonDeserializer>();
            this.serializers = new Dictionary<Type, IJsonSerializer>();
        }

        /// <summary>
        /// Registers a deserializer for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to deserialize to.</param>
        /// <param name="deserializer">The deserializer to use.</param>
        public void RegisterDeserializer(Type type, IJsonDeserializer deserializer)
        {
            this.deserializers[type] = deserializer;
        }

        /// <summary>
        /// Registers a serializer for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to serialize from.</param>
        /// <param name="serializer">The serializer to use.</param>
        public void RegisterSerializer(Type type, IJsonSerializer serializer)
        {
            this.serializers[type] = serializer;
        }

        /// <summary>
        /// Indicates whether a <see cref="JsonValue"/> can be deserialized to a given type.
        /// </summary>
        /// <param name="type">The type to use.</param>
        /// <returns>
        /// <see langword="true"/> if a <see cref="JsonValue"/> can be deserialized to the given type; otherwise <see langword="false"/>.
        /// </returns>
        public bool CanDeserialize(Type type)
        {
            if (type == typeof(JsonValue) ||
                this.deserializers.ContainsKey(type))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Indicates whether a given type can be serialized to a <see cref="JsonValue"/>.
        /// </summary>
        /// <param name="type">The type to use.</param>
        /// <returns>
        /// <see langword="true"/> if the given type can be serialized to a <see cref="JsonValue"/>; otherwise <see langword="false"/>.
        /// </returns>
        public bool CanSerialize(Type type)
        {
            if (type == typeof(JsonValue) ||
                this.serializers.ContainsKey(type))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deserialize the specified <paramref name="value"/> to the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="value">The <see cref="JsonValue"/> to deserialize.</param>
        /// <returns>An object created from the passed <see cref="JsonValue"/>.</returns>
        public T Deserialize<T>(JsonValue value)
        {
            IJsonDeserializer deserializer;
            if (this.deserializers.TryGetValue(typeof(T), out deserializer))
            {
                return (T)deserializer.Deserialize(value, this);
            }
            throw new JsonException(String.Format("Could not find deserializer for type '{0}'.", typeof(T)));
        }

        /// <summary>
        /// Serialize the specified <paramref name="obj"/> to a <see cref="JsonValue"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A <see cref="JsonValue"/> object created from the passed objects.</returns>
        public JsonValue Serialize(object obj)
        {
            Type objType = obj.GetType();
            IJsonSerializer serializer;
            if (this.serializers.TryGetValue(objType, out serializer))
            {
                return serializer.Serialize(obj, this);
            }
            throw new JsonException(String.Format("Could not find serializer for type '{0}'.", objType));
        }
    }
}

