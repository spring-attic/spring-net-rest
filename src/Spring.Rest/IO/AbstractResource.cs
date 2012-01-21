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
    /// Convenience base class for <see cref="Spring.IO.IResource"/> implementations.
    /// </summary>
    /// <author>Bruno Baia</author>
    /// <seealso cref="Spring.IO.IResource"/>
    public abstract class AbstractResource : IResource
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Spring.IO.AbstractResource"/> class.
        /// </summary>
        protected AbstractResource()
        {
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
        public abstract Uri Uri { get; }

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
        public abstract Stream GetStream();

        #endregion

        /// <summary>
        /// Strips any protocol name from the supplied <paramref name="resourceName"/>.
        /// </summary>
        /// <remarks>
        /// If the supplied <paramref name="resourceName"/> does not have any protocol associated with it, 
        /// then the supplied <paramref name="resourceName"/> will be returned as-is.
        /// </remarks>
        /// <example>
        /// <code language="C#">
        /// GetResourceNameWithoutProtocol("http://www.mycompany.com/resource.txt");
        ///	// returns www.mycompany.com/resource.txt
        /// </code>
        /// </example>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns>The name of the resource without the protocol name.</returns>
        protected static string GetResourceNameWithoutProtocol(string resourceName)
        {
            int pos = resourceName.IndexOf(Uri.SchemeDelimiter);
            if (pos == -1)
            {
                return resourceName;
            }
            else
            {
                return resourceName.Substring(pos + Uri.SchemeDelimiter.Length);
            }
        }
    }
}