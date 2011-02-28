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

using NUnit.Framework;

namespace Spring.Util
{
    // From Spring.Core

    /// <summary>
    /// Unit tests for the AssertUtils class.
    /// </summary>
    /// <author>Rick Evans</author>
    [TestFixture]
    public sealed class AssertUtilsTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNull()
        {
            AssertUtils.ArgumentNotNull(null, "foo");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNullWithMessage()
        {
            AssertUtils.ArgumentNotNull(null, "foo", "Bang!");
        }

        [Test]
        public void ArgumentHasTextWithValidText()
        {
            AssertUtils.ArgumentHasText("... and no-one's getting fat 'cept Mama Cas!", "foo");
        }

        [Test]
        public void ArgumentHasTextWithValidTextAndMessage()
        {
            AssertUtils.ArgumentHasText("... and no-one's getting fat 'cept Mama Cas!", "foo", "Bang!");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentHasText()
        {
            AssertUtils.ArgumentHasText(null, "foo");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentHasTextWithMessage()
        {
            AssertUtils.ArgumentHasText(null, "foo", "Bang!");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentHasTextWithWhiteSpaceCharacters()
        {
            AssertUtils.ArgumentHasText("   ", "foo");
        }
    }
}
