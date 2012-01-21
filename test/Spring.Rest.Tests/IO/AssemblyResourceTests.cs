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

using NUnit.Framework;

namespace Spring.IO
{
    // From Spring.Core.Tests 

    /// <summary>
    /// Unit tests for AssemblyResource
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class AssemblyResourceTests
    {
        /// <summary>
        /// Use incorrect format for an assembly resource.  Using
        /// comma delimited instead of '/'.
        /// </summary>
        [Test]
        [ExpectedException(typeof(UriFormatException))]
        public void CreateWithMalformedResourceName()
        {
            new AssemblyResource("assembly://Spring.Core.Tests,Spring.TestResource.txt");
        }

         /// <summary>
        /// Use the correct format but with an invalid assembly name.
        /// </summary>
        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreateFromInvalidAssembly()
        {
            new AssemblyResource("assembly://Xyz.Invalid.Assembly/Spring/TestResource.txt");
        }

        /// <summary>
        /// Sunny day scenario that creates IResources with full resource name 
        /// and ensures the correct contents can be read from them.
        /// </summary>
        [Test]
        public void CreateValidAssemblyResourceWithFullResourceName()
        {
            AssemblyResource res = new AssemblyResource("assembly://Spring.Rest.Tests/Spring/TestResource.txt");
            Assert.IsFalse(res.IsOpen);
            Assert.AreEqual(new Uri("assembly://Spring.Rest.Tests/Spring/TestResource.txt"), res.Uri);
            AssertResourceContent(res, "Spring.TestResource.txt");
            IResource res2 = new AssemblyResource("assembly://Spring.Rest.Tests/Spring.IO/TestResource.txt");
            AssertResourceContent(res2, "Spring.IO.TestResource.txt");
        }

        /// <summary>
        /// Sunny day scenario that creates IResources with relative resource name 
        /// and ensures the correct contents can be read from them.
        /// </summary>
        [Test]
        public void CreateValidAssemblyResourceWithRelativeResourceName()
        {
            AssemblyResource res = new AssemblyResource("TestResource.txt", typeof(AssemblyResourceTests));
            Assert.IsFalse(res.IsOpen);
            Assert.AreEqual(new Uri("assembly://Spring.Rest.Tests/Spring.IO/TestResource.txt"), res.Uri);
            AssertResourceContent(res, "Spring.IO.TestResource.txt");
        }

        /// <summary>
        /// Utility method to compare a resource that contains a single string with an exemplar.
        /// </summary>
        /// <param name="res">The resource to read a line from</param>
        /// <param name="expectedContent">the expected value of the line.</param>
        private void AssertResourceContent(IResource res, string expectedContent)
        {
            using (StreamReader reader = new StreamReader(res.GetStream()))
            {
                Assert.AreEqual(expectedContent, reader.ReadLine(), "Resource content is not as expected");
            }
        }
    }
}