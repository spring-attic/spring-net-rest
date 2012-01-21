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
using System.Collections.Generic;

using Spring.IO;
using Spring.Util;

namespace Spring.Http.Converters
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can read and write <see cref="IResource"/>s.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default, this converter supports all media types.
    /// </para>
    /// <para>
    /// A mapping between file extension and mime types is used to determine the Content-Type of written files. 
    /// If no Content-Type is available, 'application/octet-stream' is used.
    /// </para>
    /// <para>
    /// This can be overridden by setting the <see cref="P:SupportedMediaTypes"/> property.
    /// </para>
    /// </remarks>
    /// <author>Arjen Poutsma</author>
    /// <author>Bruno Baia (.NET)</author>
    public class ResourceHttpMessageConverter : AbstractHttpMessageConverter
    {
        // Pre-defined mapping between file extension and mime types
        private static IDictionary<string, string> defaultMimeMapping;

        private IDictionary<string, string> _mimeMapping;

        /// <summary>
        /// Gets or sets the mapping between file extension and mime types.
        /// </summary>
        public IDictionary<string, string> MimeMapping
        {
            get 
            {
                if (this._mimeMapping == null)
                {
                    this._mimeMapping = new Dictionary<string, string>(defaultMimeMapping);
                }
                return _mimeMapping; 
            }
            set { _mimeMapping = value; }
        }

        static ResourceHttpMessageConverter()
        {
            defaultMimeMapping = new Dictionary<string, string>(9, StringComparer.OrdinalIgnoreCase);
            defaultMimeMapping.Add(".bmp", "image/bmp");
            defaultMimeMapping.Add(".gif", "image/gif");
            defaultMimeMapping.Add(".jpg", "image/jpeg");
            defaultMimeMapping.Add(".jpeg", "image/jpeg");
            defaultMimeMapping.Add(".pdf", "application/pdf");
            defaultMimeMapping.Add(".png", "image/png");
            defaultMimeMapping.Add(".tif", "image/tiff");
            defaultMimeMapping.Add(".txt", "text/plain");
            defaultMimeMapping.Add(".zip", "application/x-zip-compressed");
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FileInfoHttpMessageConverter"/>.
        /// </summary>
        public ResourceHttpMessageConverter()
            :  base(MediaType.ALL)
        {
        }

        /// <summary>
        /// Indicates whether the given class is supported by this converter.
        /// </summary>
        /// <param name="type">The type to test for support.</param>
        /// <returns><see langword="true"/> if supported; otherwise <see langword="false"/></returns>
        protected override bool Supports(Type type)
        {
            return typeof(IResource).IsAssignableFrom(type);
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
            using (MemoryStream tempStream = new MemoryStream())
            {
                IoUtils.CopyStream(message.Body, tempStream);
                return new ByteArrayResource(tempStream.ToArray()) as T;
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
                using (Stream contentStream = ((IResource)content).GetStream())
                {
                    IoUtils.CopyStream(contentStream, stream);
                }
            };
        }


        /// <summary>
        /// Returns the default content type for the given object. 
        /// Called when <see cref="M:Write"/> is invoked without a specified content type parameter.
        /// </summary>
        /// <remarks>
        /// This implementation uses the mapping between file extension and mime types is used 
        /// to determine the Content-Type of written files. 
        /// If no Content-Type is available, 'application/octet-stream' is used.
        /// </remarks>
        /// <param name="content">The object to return the content type for.</param>
        /// <returns>The <see cref="MediaType">content type</see>, or null if not known.</returns>
        protected override MediaType GetDefaultContentType(object content)
        {
            Uri resourceUri = ((IResource)content).Uri;

            if (resourceUri != null)
            {
                string fileExtension = Path.GetExtension(resourceUri.ToString());
                IDictionary<string, string> mimeMapping =
                    (this._mimeMapping == null) ? defaultMimeMapping : this._mimeMapping;

                string mimeType;
                if (mimeMapping.TryGetValue(fileExtension, out mimeType))
                {
                    return MediaType.Parse(mimeType);
                }
            }
            return MediaType.APPLICATION_OCTET_STREAM;
        }
    }
}
