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
using System.Reflection;

using Spring.Util;

namespace Spring.IO
{
    /// <summary>
    /// An <see cref="Spring.IO.IResource"/> implementation for resources stored within assemblies 
    /// (aka embedded resources).
    /// </summary>
    /// <remarks>
    /// This implementation expects any resource name passed to the
    /// constructor to adhere to the following format:
    /// <code>
    /// assembly://<i>assemblyName</i>/<i>namespace</i>/<i>resourceName</i>
    /// </code>
    /// </remarks>
    /// <author>Bruno Baia</author>
    /// <seealso cref="Spring.IO.IResource"/>
    public class AssemblyResource : AbstractResource
    {
        private Assembly assembly;
        private string fullResourceName;
        private string resourceName;
        private string resourceNamespace;
        private string resourceAssemblyName;

        /// <summary>
        /// Creates a new instance of the <see cref="Spring.IO.AssemblyResource"/> class 
        /// with the specified resource name.
        /// </summary>
        /// <param name="resourceName">
        /// The resource name that must follow the format 'assembly://assemblyName/namespace/resourceName'.
        /// </param>
        /// <exception cref="System.UriFormatException">
        /// If the supplied <paramref name="resourceName"/> did not conform to the expected format.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// If the assembly specified in the supplied <paramref name="resourceName"/> could not be found.
        /// </exception>
        public AssemblyResource(string resourceName)
            : base(resourceName)
        {
            string[] info = GetResourceNameWithoutProtocol(resourceName).Split('/');
            if (info.Length != 3)
            {
                throw new UriFormatException(String.Format(
                    "Invalid resource name. Name has to be in 'assembly://<assemblyName>/<namespace>/<resourceName>' format.", resourceName));
            }
            this.assembly = Assembly.Load(info[0]);
            if (this.assembly == null)
            {
                throw new FileNotFoundException(String.Format(
                    "Unable to load assembly [{0}].", info[0]));
            }
            this.fullResourceName = resourceName;
            this.resourceAssemblyName = info[0];
            this.resourceNamespace = info[1];
            this.resourceName = String.Format("{0}.{1}", info[1], info[2]);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Spring.IO.AssemblyResource"/> class. 
        /// Generates the full assembly resource name from a file name and a type used to obtain the assembly name and the namespace.
        /// </summary>
        /// <param name="resourceName">The name of the file in the assembly.</param>
        /// <param name="type">The type to determine the assembly and the namespace.</param>
        /// <exception cref="System.UriFormatException">
        /// If the generated resource name did not conform to the expected format.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">
        /// If the assembly specified in the generated resource name could not be found.
        /// </exception>
        public AssemblyResource(string resourceName, Type type)
            : base(GetFullAssemblyResourceName(resourceName, type))
        {
        }

        /// <summary>
        /// Constructs a "assembly://assemblyName/namespace/resourceName" qualified resource name 
        /// using the given type to obtain the assembly name and the namespace.
        /// </summary>
        private static string GetFullAssemblyResourceName(string resourceName, Type type)
        {
            ArgumentUtils.AssertHasText(resourceName, "resourceName");
            ArgumentUtils.AssertNotNull(type, "type");

            return String.Format("assembly://{0}/{1}/{2}", type.Assembly.FullName, type.Namespace, resourceName);
        }

        /// <summary>
        /// Gets the <see cref="System.Uri"/> handle for this resource.
        /// </summary>
        public override Uri Uri
        {
            get { return new Uri(this.fullResourceName); }
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
            Stream stream = this.assembly.GetManifestResourceStream(this.resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException(String.Format(
                    "Could not load resource [{0}] from assembly [{1}]. Spring.NET URI syntax is 'assembly://MyAssembly/MyNamespace/MyResource.ext'", this.resourceName, this.assembly));
            }
            return stream;
        }
      
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current resource.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current resource.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, 
                "Resource [{0}] from assembly [{1}]", this.resourceName, this.assembly.FullName);
        }
    }
}