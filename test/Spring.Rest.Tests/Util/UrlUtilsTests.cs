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
using System.Text;

using NUnit.Framework;

namespace Spring.Util
{
    /// <summary>
    /// Unit tests for the UrlUtils class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class UrlUtilsTests
    {
        [Test]
        public void Decode()
        {
            Assert.AreEqual("bbaïa", UrlUtils.Decode("bba%c3%afa", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("foobar", UrlUtils.Decode("foobar", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("foo bar", UrlUtils.Decode("foo+bar", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("foo bar", UrlUtils.Decode("foo%20bar", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("foo+bar", UrlUtils.Decode("foo%2bbar", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("/Z\u00fcrich", UrlUtils.Decode("/Z%C3%BCrich", Encoding.UTF8), "Invalid encoded result");
        }

        [Test]
        public void Encode()
        {
            Assert.AreEqual("bba%c3%afa", UrlUtils.Encode("bbaïa", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("foo+bar", UrlUtils.Encode("foo bar", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("foo%2bbar", UrlUtils.Encode("foo+bar", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("foo%26bar", UrlUtils.Encode("foo&bar", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("foobar%2f%2b", UrlUtils.Encode("foobar/+", Encoding.UTF8), "Invalid encoded result");
            Assert.AreEqual("Z%c3%bcrich", UrlUtils.Encode("Z\u00fcrich", Encoding.UTF8), "Invalid encoded result");
        }
    }
}
