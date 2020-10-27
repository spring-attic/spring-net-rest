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
using System.Collections.Generic;

using Spring.Util;
using Spring.IO;

namespace Spring.Http.Converters
{
    /// <summary>
    /// Implementation of <see cref="IHttpMessageConverter"/> that can write files.
    /// </summary>
    /// <remarks>
    /// A mapping between file extension and mime types is used to determine the Content-Type of written files. 
    /// If no Content-Type is available, 'application/octet-stream' is used.
    /// </remarks>
    /// <author>Bruno Baia</author>
    [Obsolete("This class is obsolete; use ResourceHttpMessageConverter instead.")]
    public class FileInfoHttpMessageConverter : ResourceHttpMessageConverter
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FileInfoHttpMessageConverter"/>.
        /// </summary>
        public FileInfoHttpMessageConverter()
            :base()
        {
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
            return false;
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
            return type.Equals(typeof(FileInfo));
        }

        /// <summary>
        /// Read an object of the given type form the given HTTP message, and returns it.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to return. This type must have previously been passed to the 
        /// <see cref="M:CanRead"/> method of this interface, which must have returned <see langword="true"/>.
        /// </typeparam>
        /// <param name="message">The HTTP message to read from.</param>
        /// <returns>The converted object.</returns>
        /// <exception cref="HttpMessageNotReadableException">In case of conversion errors</exception>
        public override T Read<T>(IHttpInputMessage message)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Write an given object to the given HTTP message.
        /// </summary>
        /// <param name="content">
        /// The object to write to the HTTP message. The type of this object must have previously been 
        /// passed to the <see cref="M:CanWrite"/> method of this interface, which must have returned <see langword="true"/>.
        /// </param>
        /// <param name="contentType">
        /// The content type to use when writing. May be null to indicate that the default content type of the converter must be used. 
        /// If not null, this media type must have previously been passed to the <see cref="M:CanWrite"/> method of this interface, 
        /// which must have returned <see langword="true"/>.
        /// </param>
        /// <param name="message">The HTTP message to write to.</param>
        /// <exception cref="HttpMessageNotWritableException">In case of conversion errors</exception>
        public override void Write(object content, MediaType contentType, IHttpOutputMessage message)
        {
            base.Write(new FileResource(((FileInfo)content).FullName), contentType, message);
        }
    }
}
