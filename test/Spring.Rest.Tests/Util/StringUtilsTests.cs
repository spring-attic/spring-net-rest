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

namespace Spring.Util
{
    // From Spring.Core

    /// <summary>
    /// Unit tests for the StringUtils class.
    /// </summary>
    /// <author>Aleksandar Seovic</author>
    /// <author>Rick Evans</author>
    /// <author>Griffin Caprio</author>
    [TestFixture]
    public sealed class StringtUtilsTests
    {
        [Test]
        public void HasLengthTests()
        {
            Assert.IsFalse(StringUtils.HasLength(null));
            Assert.IsFalse(StringUtils.HasLength(""));
            Assert.IsTrue(StringUtils.HasLength(" "));
            Assert.IsTrue(StringUtils.HasLength("Hello"));
        }

        [Test]
        public void HasTextTests()
        {
            Assert.IsFalse(StringUtils.HasText(null));
            Assert.IsFalse(StringUtils.HasText(""));
            Assert.IsFalse(StringUtils.HasText(" "));
            Assert.IsTrue(StringUtils.HasText("12345"));
            Assert.IsTrue(StringUtils.HasText("  12345  "));
        }
    }
}
