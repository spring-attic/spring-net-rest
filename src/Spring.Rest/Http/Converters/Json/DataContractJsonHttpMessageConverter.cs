#if NET_3_5 || SILVERLIGHT
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
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Spring.Http.Converters.Json
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can read and write JSON 
    /// using <see cref="DataContractJsonSerializer"/>.
    /// </summary>
    /// <remarks>
    /// By default, this converter supports 'application/json' media type. 
    /// This can be overridden by setting the <see cref="P:SupportedMediaTypes"/> property.
    /// </remarks>
    /// <author>Bruno Baia</author>
    public class DataContractJsonHttpMessageConverter : AbstractHttpMessageConverter
    {
        /// <summary>
        /// Default encoding for JSON.
        /// </summary>
        protected static readonly Encoding DEFAULT_CHARSET = new UTF8Encoding(false); // Remove byte Order Mask (BOM)

        private IEnumerable<Type> _knownTypes;
        private bool _requiresAttribute;

        /// <summary>
        /// Gets or sets types that may be present in the object graph.
        /// </summary>
        public IEnumerable<Type> KnownTypes
        {
            get { return _knownTypes; }
            set { _knownTypes = value; }
        }

        /// <summary>
        /// Indicates whether this converter supports only classes decorated with 
        /// <see cref="DataContractAttribute"/> and <see cref="CollectionDataContractAttribute"/>. 
        /// Default value is <c>false</c>.
        /// </summary>
        public bool RequiresAttribute
        {
            get { return _requiresAttribute; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DataContractJsonHttpMessageConverter"/> 
        /// with the media type 'application/json'. 
        /// </summary>
        public DataContractJsonHttpMessageConverter() :
            base(new MediaType("application", "json"))
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DataContractJsonHttpMessageConverter"/> 
        /// with the media type 'application/json'. 
        /// </summary>
        /// <param name="requiresAttribute">
        /// If <c>true</c>, supports only classes decorated with 
        /// <see cref="DataContractAttribute"/> and <see cref="CollectionDataContractAttribute"/>. 
        /// </param>
        public DataContractJsonHttpMessageConverter(bool requiresAttribute) :
            base(new MediaType("application", "json"))
        {
            this._requiresAttribute = requiresAttribute;
        }

        /// <summary>
        /// Indicates whether the given class is supported by this converter.
        /// </summary>
        /// <param name="type">The type to test for support.</param>
        /// <returns><see langword="true"/> if supported; otherwise <see langword="false"/></returns>
        protected override bool Supports(Type type)
        {
            if (this._requiresAttribute)
            {
                return (
                    Attribute.GetCustomAttributes(type, typeof(DataContractAttribute), true).Length > 0 ||
                    Attribute.GetCustomAttributes(type, typeof(CollectionDataContractAttribute), true).Length > 0
                    );
            }
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
            DataContractJsonSerializer serializer = this.GetSerializer(typeof(T));
            return (T)serializer.ReadObject(message.Body) as T;
        }

        /// <summary>
        /// Abstract template method that writes the actual body. Invoked from <see cref="M:Write"/>.
        /// </summary>
        /// <param name="content">The object to write to the HTTP message.</param>
        /// <param name="message">The HTTP message to write to.</param>
        /// <exception cref="HttpMessageNotWritableException">In case of conversion errors</exception>
        protected override void WriteInternal(object content, IHttpOutputMessage message)
        {
#if SILVERLIGHT
            // Write to the message stream
            message.Body = delegate(Stream stream)
            {
                DataContractJsonSerializer serializer = this.GetSerializer(content.GetType());
                serializer.WriteObject(stream, content);
            };
#else
            // Get the message encoding
            Encoding encoding = this.GetContentTypeCharset(message.Headers.ContentType, DEFAULT_CHARSET);

            DataContractJsonSerializer serializer = this.GetSerializer(content.GetType());
            
            // Write to the message stream
            message.Body = delegate(Stream stream)
            {
                // Using JsonReaderWriterFactory directly to set encoding
                using (XmlDictionaryWriter jsonWriter = JsonReaderWriterFactory.CreateJsonWriter(stream, encoding, false))
                {
                    serializer.WriteObject(jsonWriter, content);
                }
            };
#endif
        }

        /// <summary>
        /// Creates an instance of <see cref="DataContractJsonSerializer"/> to 
        /// serialize or deserialize an object of the specified type.
        /// </summary>
        /// <param name="type">The type of instances to serialize or deserialize.</param>
        /// <returns>The serializer to use.</returns>
        protected virtual DataContractJsonSerializer GetSerializer(Type type)
        {
            if (this._knownTypes == null)
            {
                return new DataContractJsonSerializer(type);
            }
            else
            {
                return new DataContractJsonSerializer(type, this._knownTypes);
            }
        }
    }
}
#endif