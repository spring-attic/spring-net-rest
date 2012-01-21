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
using System.Globalization;

namespace Spring.IO
{
	/// <summary>
    /// A byte array backed resource.
	/// </summary>
	/// <author>Bruno Baia</author>
    /// <seealso cref="Spring.IO.IResource"/>
	public class ByteArrayResource : IResource
	{
        private byte[] bytes;

        /// <summary>
        /// Creates a new instance of the <see cref="Spring.IO.ByteArrayResource"/> class.
        /// </summary>
        /// <param name="bytes">The byte array for this resource.</param>
        public ByteArrayResource(byte[] bytes)
        {
            this.bytes = bytes ?? new byte[0];
        }

        /// <summary>
        /// Gets the byte array encapsulated by this resource.
        /// </summary>
        public byte[] Bytes
        {
            get { return this.bytes; }
        }

        #region IResource Members

        /// <summary>
        /// Gets a value indicating whether or not this resource represents a handle with an open stream? 
        /// Returns <see langword="true"/> if the source cannot be read multiple times.
        /// </summary>
        /// <remarks>
        /// This implementation always returns <see langword="false"/>.
        /// </remarks>
        public virtual bool IsOpen
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the <see cref="System.Uri"/> handle for this resource, 
        /// or <see langword="null"/> if the source cannot be represented by an <see cref="System.Uri"/>.
        /// </summary>
        /// <remarks>
        /// This implementation always returns <see langword="null"/>, 
        /// assuming that the resource cannot be represented as an <see cref="System.Uri"/>.
        /// </remarks>
        public virtual Uri Uri
        {
            get { return null; }
        }

        /// <summary>
        /// Returns a <see cref="System.IO.Stream"/> for this resource.
        /// </summary>
        /// <remarks>
        /// Clients of this interface must be aware that every access of this 
        /// method will create a <i>fresh</i> <see cref="System.IO.Stream"/>;
        /// it is the responsibility of the calling code to close any such <see cref="System.IO.Stream"/>.
        /// </remarks>
        /// <returns>
        /// An <see cref="System.IO.Stream"/>.
        /// </returns>
        /// <seealso cref="Spring.IO.IResource.IsOpen"/>
        public virtual Stream GetStream()
        {
            return new MemoryStream(this.bytes);
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current resource.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current resource.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Byte array resource [Length:{0}]", this.bytes.Length);
        }
	}
}