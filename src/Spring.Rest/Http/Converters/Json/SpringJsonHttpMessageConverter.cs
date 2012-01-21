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
using System.Text;
using System.Collections.Generic;

using Spring.Json;

namespace Spring.Http.Converters.Json
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can read and write JSON 
    /// using the Spring.Json library.
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
    public class SpringJsonHttpMessageConverter : AbstractHttpMessageConverter
    {
        /// <summary>
        /// Default encoding for JSON strings.
        /// </summary>
        protected static readonly Encoding DEFAULT_CHARSET = new UTF8Encoding(false); // Remove byte Order Mask (BOM)

        private JsonMapper mapper;

        /// <summary>
        /// Gets the underlying <see cref="JsonMapper"/> used for converting custom types.
        /// </summary>
        public JsonMapper JsonMapper
        {
            get { return mapper; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SpringJsonHttpMessageConverter"/> 
        /// with the media type 'application/json'.
        /// </summary>
        public SpringJsonHttpMessageConverter() :
            this(new JsonMapper())
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SpringJsonHttpMessageConverter"/> 
        /// with the media type 'application/json'.
        /// </summary>
        /// <param name="mapper">A <see cref="JsonMapper"/> to use for converting custom types.</param>
        public SpringJsonHttpMessageConverter(JsonMapper mapper) :
            base(new MediaType("application", "json"))
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Indicates whether the given class can be read by this converter.
        /// </summary>
        /// <param name="type">The class to test for readability</param>
        /// <param name="mediaType">
        /// The media type to read, can be null if not specified. Typically the value of a 'Content-Type' header.
        /// </param>
        /// <returns><see langword="true"/> if readable; otherwise <see langword="false"/></returns>
        public override bool CanRead(Type type, MediaType mediaType)
        {
            return base.CanRead(mediaType) &&
                (typeof(JsonValue).IsAssignableFrom(type) || this.mapper.CanDeserialize(type));
        }

        /// <summary>
        /// Indicates whether the given class can be written by this converter.
        /// </summary>
        /// <param name="type">The class to test for writability</param>
        /// <param name="mediaType">
        /// The media type to write, can be null if not specified. Typically the value of an 'Accept' header.
        /// </param>
        /// <returns><see langword="true"/> if writable; otherwise <see langword="false"/></returns>
        public override bool CanWrite(Type type, MediaType mediaType)
        {
            return base.CanWrite(mediaType) &&
                (typeof(JsonValue).IsAssignableFrom(type) || this.mapper.CanSerialize(type));
        }

        /// <summary>
        /// Indicates whether the given class is supported by this converter.
        /// </summary>
        /// <param name="type">The type to test for support.</param>
        /// <returns><see langword="true"/> if supported; otherwise <see langword="false"/></returns>
        protected override bool Supports(Type type)
        {
            // Should not be called
            throw new InvalidOperationException();
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
            // Get the message encoding
            Encoding encoding = GetContentTypeCharset(message.Headers.ContentType, DEFAULT_CHARSET);

            // Read from the message stream
            using (StreamReader reader = new StreamReader(message.Body, encoding))
            {
                // Parse JSON
                JsonValue jsonValue = JsonValue.Parse(reader.ReadToEnd());

                if (typeof(T) == typeof(JsonValue))
                {
                    return jsonValue as T;
                }
                // Map from JsonValue to object
                return this.mapper.Deserialize<T>(jsonValue);
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
            // Get the message encoding
            Encoding encoding = GetContentTypeCharset(message.Headers.ContentType, DEFAULT_CHARSET);

            // Map from object to JsonValue
            JsonValue jsonValue = (content is JsonValue) ? (JsonValue)content : this.mapper.Serialize(content);

            // Create a byte array of the data we want to send  
            byte[] byteData = encoding.GetBytes(jsonValue.ToString());

            // Write to the message stream
            message.Body = delegate(Stream stream)
            {
                stream.Write(byteData, 0, byteData.Length);
            };
        }
    }
}