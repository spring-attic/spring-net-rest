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
using System.Collections.Generic;

using NUnit.Framework;

namespace Spring.Http
{
    /// <summary>
    /// Unit tests for the HttpMethod class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class HttpMethodTests
    {
        [Test]
        public void Equals()
        {
            Assert.IsTrue(HttpMethod.GET.Equals(new HttpMethod("GET")));
            Assert.IsTrue(((object)HttpMethod.GET).Equals(new HttpMethod("GET")));
        }

        [Test]
        public void EqualsCaseInsensitive()
        {
            Assert.IsTrue(new HttpMethod("get").Equals(HttpMethod.GET));
        }

        [Test]
        public void EqualsNull()
        {
            Assert.IsFalse(HttpMethod.GET.Equals(null));
        }

        [Test]
        public void EqualsOperator()
        {
            Assert.IsTrue(HttpMethod.DELETE == new HttpMethod("Delete"));
        }

        [Test]
        public void NotEqualsOperator()
        {
            Assert.IsTrue(HttpMethod.GET != HttpMethod.OPTIONS);
        }

        [Test]
        public void GetHashCodeTest()
        {
            IDictionary<HttpMethod, string> dictionary = new Dictionary<HttpMethod, string>();
            dictionary.Add(HttpMethod.GET, "value for get");
            Assert.IsTrue(dictionary.ContainsKey(new HttpMethod("get")));
        }

        [Test]
        public void Equatable()
        {
            IList<HttpMethod> list = new List<HttpMethod>();
            list.Add(HttpMethod.GET);
            Assert.IsTrue(list.Contains(new HttpMethod("Get")));
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual("GET", HttpMethod.GET.ToString());
            Assert.AreEqual("Get", new HttpMethod("Get").ToString());
        }
    }
}
