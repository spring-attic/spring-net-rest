﻿#region License

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

using NUnit.Framework;

namespace Spring.Util
{
    // From Spring.Core.Tests

    /// <summary>
    /// Unit tests for the ArgumentUtils class.
    /// </summary>
    /// <author>Rick Evans</author>
    [TestFixture]
    public sealed class ArgumentUtilsTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNull()
        {
            ArgumentUtils.AssertNotNull(null, "foo");
        }

        [Test]
        public void ArgumentHasTextWithValidText()
        {
            ArgumentUtils.AssertHasText("... and no-one's getting fat 'cept Mama Cas!", "foo");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentHasText()
        {
            ArgumentUtils.AssertHasText(null, "foo");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentHasTextWithWhiteSpaceCharacters()
        {
            ArgumentUtils.AssertHasText("   ", "foo");
        }
    }
}
