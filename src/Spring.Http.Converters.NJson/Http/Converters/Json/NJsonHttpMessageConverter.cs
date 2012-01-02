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
using System.IO;

using Newtonsoft.Json;

namespace Spring.Http.Converters.Json
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can read and write JSON 
    /// using the Json.NET (Newtonsoft.Json) library.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation supports getting/setting values from JSON directly, 
    /// without the need to deserialize/serialize to a .NET class.
    /// </para>
    /// <para>
    /// By default, this converter supports 'application/json' media type. 
    /// This can be overridden by setting the <see cref="P:SupportedMediaTypes"/> property.
    /// </para>
    /// </remarks>
    /// <author>Bruno Baia</author>
    public class NJsonHttpMessageConverter : AbstractHttpMessageConverter
    {
        /// <summary>
        /// Creates a new instance of the <see cref="NJsonHttpMessageConverter"/> 
        /// with the media type 'application/json'. 
        /// </summary>
        public NJsonHttpMessageConverter() :
            base(new MediaType("application", "json"))
        {
        }

        /// <summary>
        /// Indicates whether the given class is supported by this converter.
        /// </summary>
        /// <param name="type">The type to test for support.</param>
        /// <returns><see langword="true"/> if supported; otherwise <see langword="false"/></returns>
        protected override bool Supports(Type type)
        {
            return true;
        }

        /// <summary>
        /// Abstract template method that reads the actualy object. Invoked from <see cref="M:Read"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="message">The HTTP message to read from.</param>
        /// <returns>The converted object.</returns>
        /// <exception cref="HttpMessageNotReadableException">In case of conversion errors</exception>
        protected override T ReadInternal<T>(IHttpInputMessage message)
        {
            // Read from the message stream
            using (StreamReader reader = new StreamReader(message.Body))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                return jsonSerializer.Deserialize<T>(jsonReader);
            }
        }

        /// <summary>
        /// Abstract template method that writes the actual body. Invoked from <see cref="M:Write"/>.
        /// </summary>
        /// <param name="content">The object to write to the HTTP message.</param>
        /// <param name="message">The HTTP message to write to.</param>
        /// <exception cref="HttpMessageNotWritableException">In case of conversion errors</exception>
        protected override void WriteInternal(object content, IHttpOutputMessage message)
        {
            // Write to the message stream
            message.Body = delegate(Stream stream)
            {
                using (StreamWriter writer = new StreamWriter(stream))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(jsonWriter, content);
                }
            };
        }
    }
}