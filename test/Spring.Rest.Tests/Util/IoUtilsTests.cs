#region License

/*
 * Copyright 2002-2011 the original author or authors.
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

namespace Spring.Util
{
    /// <summary>
    /// Unit tests for the IoUtils class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public sealed class IoUtilsTests
    {
        [Test]
        public void CopyStream()
        {
            string text = "Hello !";

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            MemoryStream source = new MemoryStream(bytes);

            MemoryStream destination = new MemoryStream();

            IoUtils.CopyStream(source, destination);

            Assert.AreEqual(text.Length, destination.Length, "Invalid copy !");
            Assert.AreEqual(text.Length, destination.Position, "Invalid copy !");

            destination.Position = 0;
            using (StreamReader reader = new StreamReader(destination, Encoding.UTF8))
            {
                Assert.AreEqual(text, reader.ReadToEnd(), "Invalid copy !");
            }
        }
    }
}
