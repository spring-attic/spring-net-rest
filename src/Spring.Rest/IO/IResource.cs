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

namespace Spring.IO
{
    // Light version of the Spring.NET Core IResource abstraction

	/// <summary>
    /// The central abstraction for access to resources such as <see cref="System.IO.Stream"/>s.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface encapsulates a resource descriptor that abstracts away from the underlying type of resource; 
    /// possible resource types include files, memory streams, and databases (this list is not exhaustive).
	/// </para>
	/// <para>
	/// A <see cref="System.IO.Stream"/> can definitely be opened and accessed for every such resource; 
    /// if the resource exists in a physical form (for example, the resource is not an in-memory stream 
    /// or one that has been extracted from an assembly or ZIP file), a <see cref="System.Uri"/> 
    /// can also be accessed. The actual behavior is implementation-specific.
	/// </para>
	/// <para>
	/// Third party extensions or libraries that want to integrate external resources 
    /// are encouraged expose such resources via this abstraction.
	/// </para>
	/// </remarks>
	/// <author>Bruno Baia</author>
	public interface IResource
	{
		/// <summary>
        /// Gets a value indicating whether or not this resource represents a handle with an open stream? 
        /// Returns <see langword="true"/> if the source cannot be read multiple times.
		/// </summary>
		bool IsOpen { get; }

        /// <summary>
        /// Gets the <see cref="System.Uri"/> handle for this resource, 
        /// or <see langword="null"/> if the source cannot be represented by an <see cref="System.Uri"/>.
        /// </summary>
		Uri Uri { get; }

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
        Stream GetStream();
	}
}
