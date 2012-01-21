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

using Spring.Util;

namespace Spring.IO
{
    /// <summary>
    /// A <see cref="System.IO.FileInfo"/> backed resource.
    /// </summary>
    /// <author>Bruno Baia</author>
    /// <seealso cref="Spring.IO.IResource"/>
    public class FileResource : AbstractResource
    {
        private FileInfo fileInfo;
        private Uri fileUri;

        /// <summary>
        /// Creates a new instance of the <see cref="Spring.IO.FileResource"/> class.
        /// </summary>
        /// <param name="resourceName">The name of the file system resource.</param>
        public FileResource(string resourceName)
        {
            ArgumentUtils.AssertHasText(resourceName, "resourceName");

            // Remove protocol (if any)
            string fileName = GetResourceNameWithoutProtocol(resourceName);
            // Remove extra slashes used to indicate that resource is local (handle the case "/C:/path1/...")
            if (fileName.Length > 2 && fileName[0] == '/' && fileName[2] == ':')
            {
                fileName = fileName.Substring(1);
            }

            this.fileInfo = new FileInfo(fileName);
            this.fileUri = new Uri(this.fileInfo.FullName);
        }

        /// <summary>
        /// Gets the underlying <see cref="System.IO.FileInfo"/> handle for this resource.
        /// </summary>
        public FileInfo File
        {
            get { return this.fileInfo; }
        }

        /// <summary>
        /// Gets the <see cref="System.Uri"/> handle for this resource.
        /// </summary>
        public override Uri Uri
        {
            get { return this.fileUri; }
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
        public override Stream GetStream()
        {
            return new FileStream(this.fileUri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current resource.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current resource.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "File resource [{0}]", this.fileInfo.FullName);
        }
    }
}