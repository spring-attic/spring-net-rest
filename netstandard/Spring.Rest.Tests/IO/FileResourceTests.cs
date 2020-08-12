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
using System.Reflection;

using NUnit.Framework;

namespace Spring.IO
{
    // From Spring.Core.Tests

    /// <summary>
    /// Unit tests for the FileResource class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class FileResourceTests
    {
        protected const string TemporaryFileName = "temp.file";

        /// <summary>
        /// Creates a FileInfo instance representing the original location of the given assembly
        /// </summary>
        /// <remarks>
        /// Use this instead of the "Assembly.Location" property to get the original location before shadow copying!
        /// </remarks>
        protected static FileInfo GetAssemblyLocation(Assembly assembly)
        {
            return new FileInfo(new Uri(assembly.CodeBase).LocalPath);
        }

        protected static FileInfo CreateFileForTheCurrentDirectory()
        {
            return new FileInfo(Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemporaryFileName)));
        }

        [Test]
        public void CreateFileSystemResourceWithPathName()
        {
            FileResource res = new FileResource(TemporaryFileName);
            Assert.AreEqual(TemporaryFileName, res.File.Name);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileSystemResourceOpenNonExistanceFile()
        {
            FileResource res = new FileResource(TemporaryFileName);
            Stream stream = res.GetStream();
        }

        [Test]
        public void FileResourceValidStream()
        {
            FileInfo file = GetAssemblyLocation(Assembly.GetExecutingAssembly());
            FileResource res = new FileResource(file.FullName);
            using (Stream stream = res.GetStream())
            {
                Assert.IsNotNull(stream);
                Assert.IsTrue(stream.CanRead);
            }
        }

        [Test]
        public void ReadStreamMultipleTimes()
        {
            FileInfo file = GetAssemblyLocation(Assembly.GetExecutingAssembly());
            FileResource res = new FileResource(file.FullName);
            Assert.IsFalse(res.IsOpen);
            using (Stream stream = res.GetStream())
            {
                Assert.IsNotNull(stream);
            }
            using (Stream stream = res.GetStream())
            {
                Assert.IsNotNull(stream);
            }
        }

        [Test]
        public void GetUri()
        {
            FileResource res = new FileResource(TemporaryFileName);
            Assert.IsNotNull(res.Uri);
        }

        [Test]
        public void FileInfo()
        {
            FileInfo file = CreateFileForTheCurrentDirectory();
            FileResource res = new FileResource(TemporaryFileName);
            Assert.AreEqual(file.FullName.ToLowerInvariant(), res.File.FullName.ToLowerInvariant(),
                "The bare file name all by itself must have resolved to a file in the current directory of the currently executing domain.");
        }
    }
}