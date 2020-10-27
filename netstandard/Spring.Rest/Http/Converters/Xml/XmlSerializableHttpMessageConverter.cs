﻿
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
using System.Xml;
using System.Xml.Serialization;

namespace Spring.Http.Converters.Xml
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can read and write XML 
    /// using <see cref="XmlSerializer"/>.
    /// </summary>
    /// <remarks>
    /// By default, this converter supports 'text/xml', 'application/xml', and 'application/*-xml' media types. 
    /// This can be overridden by setting the <see cref="P:SupportedMediaTypes"/> property.
    /// </remarks>
    /// <author>Bruno Baia</author>
    public class XmlSerializableHttpMessageConverter : AbstractXmlHttpMessageConverter
    {
        private Type[] _knownTypes;

        /// <summary>
        /// Gets or sets types that may be present in the object graph.
        /// </summary>
        public Type[] KnownTypes
        {
            get { return _knownTypes; }
            set { _knownTypes = value; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="XmlSerializableHttpMessageConverter"/> 
        /// with 'text/xml', 'application/xml', and 'application/*-xml' media types.
        /// </summary>
        public XmlSerializableHttpMessageConverter() :
            base()
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
            //return (
            //    AttributeUtils.FindAttribute(type, typeof(XmlRootAttribute)) != null ||
            //    AttributeUtils.FindAttribute(type, typeof(XmlTypeAttribute)) != null);
        }

        /// <summary>
        /// Abstract template method that reads the actualy object using a <see cref="XmlReader"/>. Invoked from <see cref="M:ReadInternal"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="xmlReader">The XmlReader to use.</param>
        /// <returns>The converted object.</returns>
        protected override T ReadXml<T>(XmlReader xmlReader)
        {
            XmlSerializer serializer = this.GetSerializer(typeof(T));
            return serializer.Deserialize(xmlReader) as T;
        }

        /// <summary>
        /// Abstract template method that writes the actual body using a <see cref="XmlWriter"/>. Invoked from <see cref="M:WriteInternal"/>.
        /// </summary>
        /// <param name="xmlWriter">The XmlWriter to use.</param>
        /// <param name="content">The object to write to the HTTP message.</param>
        protected override void WriteXml(XmlWriter xmlWriter, object content)
        {
            XmlSerializer serializer = this.GetSerializer(content.GetType());
            serializer.Serialize(xmlWriter, content);
        }

        /// <summary>
        /// Creates an instance of <see cref="XmlSerializer"/> to 
        /// serialize or deserialize an object of the specified type.
        /// </summary>
        /// <param name="type">The type of instances to serialize or deserialize.</param>
        /// <returns>The serializer to use.</returns>
        protected virtual XmlSerializer GetSerializer(Type type)
        {
            if (this._knownTypes == null)
            {
                return new XmlSerializer(type);
            }
            else
            {
                return new XmlSerializer(type, this._knownTypes);
            }
        }
    }
}