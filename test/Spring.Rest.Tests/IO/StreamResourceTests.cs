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
using System.Text;

using NUnit.Framework;

namespace Spring.IO
{
    // From Spring.Core.Tests 

	/// <summary>
	/// Unit tests for the StreamResource class.
    /// </summary>
    /// <author>Bruno Baia</author>
	[TestFixture]
    public sealed class StreamResourceTests
    {
        [Test]
        public void Instantiation () 
        {
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes("A temporary resource.")))
            {
                StreamResource res = new StreamResource(stream);
                Assert.IsTrue(res.IsOpen);
                Assert.IsNull(res.Uri);
                Assert.IsNotNull(res.GetStream());
            } 
        }

        [Test]
        [ExpectedException (typeof (ArgumentNullException))]
        public void InstantiationWithNull () 
        {
            new StreamResource (null);
        }

        [Test]
        [ExpectedException (typeof(InvalidOperationException))]
        public void ReadStreamMultipleTimes () 
        {
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes("A temporary resource.")))
            {
                StreamResource res = new StreamResource(stream);
                Assert.IsTrue(res.IsOpen);
                Assert.IsNull(res.Uri);
                res.GetStream();
                res.GetStream();
            } 
        }
	}
}
