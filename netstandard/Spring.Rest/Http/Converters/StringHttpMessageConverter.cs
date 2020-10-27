﻿#region License

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

namespace Spring.Http.Converters
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can read and write strings.
    /// </summary>
    /// <remarks>
    /// By default, this converter supports all media types '*/*', and writes with a 'Content-Type' 
    /// of 'text/plain'. 
    /// This can be overridden by setting the <see cref="P:SupportedMediaTypes"/> property.
    /// </remarks>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia (.NET)</author>
    public class StringHttpMessageConverter : AbstractHttpMessageConverter
    {
        /// <summary>
        /// Default encoding for strings.
        /// </summary>

        protected static readonly Encoding DEFAULT_CHARSET = Encoding.GetEncoding("ISO-8859-1");



        /// <summary>
        /// Creates a new instance of the <see cref="StringHttpMessageConverter"/> 
        /// with 'text/plain; charset=ISO-8859-1', and '*/*' media types.
        /// </summary>
        public StringHttpMessageConverter() :
            base(new MediaType("text", "plain", "ISO-8859-1"), MediaType.ALL)
        {
        }


        /// <summary>
        /// Indicates whether the given class is supported by this converter.
        /// </summary>
        /// <param name="type">The type to test for support.</param>
        /// <returns><see langword="true"/> if supported; otherwise <see langword="false"/></returns>
        protected override bool Supports(Type type)
        {
            return type.Equals(typeof(string));
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
                return reader.ReadToEnd() as T;
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

            // Create a byte array of the data we want to send  
            byte[] byteData = encoding.GetBytes(content as string);

            // Write to the message stream
            message.Body = delegate(Stream stream) 
            {
                stream.Write(byteData, 0, byteData.Length);
            };
        }
    }
}
