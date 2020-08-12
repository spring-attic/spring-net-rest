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
using System.Collections.Generic;

using NUnit.Framework;

namespace Spring.Http
{
    /// <summary>
    /// Unit tests for the HttpUtils class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class HttpUtilsTests
    {
        [Test]
        public void UrlEncode()
        {
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~%21%27%28%29%2A", 
                HttpUtils.UrlEncode("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~!'()*"));
            Assert.AreEqual("%3F%3D%23", HttpUtils.UrlEncode("?=#"));
        }

        [Test]
        public void UrlDecode()
        {
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~!'()*",
                HttpUtils.UrlDecode("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~%21%27%28%29%2A"));
            Assert.AreEqual("?=#", HttpUtils.UrlDecode("%3F%3D%23"));
        }

        [Test]
        public void FormEncode()
        {
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~%21%27%28%29%2A",
                HttpUtils.FormEncode("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~!'()*"));
            Assert.AreEqual("%3F%3D%23", HttpUtils.FormEncode("?=#"));
            Assert.AreEqual("+", HttpUtils.FormEncode(" "));
            Assert.AreEqual("%2B", HttpUtils.FormEncode("+"));
        }

        [Test]
        public void FormDecode()
        {
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~!'()*",
                HttpUtils.FormDecode("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~%21%27%28%29%2A"));
            Assert.AreEqual("?=#", HttpUtils.FormDecode("%3F%3D%23"));
            Assert.AreEqual(" ", HttpUtils.FormDecode("+"));
            Assert.AreEqual("+", HttpUtils.FormDecode("%2B"));
        }
    }
}
