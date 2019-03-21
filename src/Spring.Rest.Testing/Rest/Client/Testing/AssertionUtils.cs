#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;

namespace Spring.Rest.Client.Testing
{
    /// <summary>
    /// Represents an unit test framework independent assertion class.
    /// </summary>
    /// <author>Lukas Krecan</author>
    /// <author>Arjen Poutsma</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public static class AssertionUtils
    {
        /// <summary>
        /// Fails a test with the given message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="AssertionException">Always.</exception>
        public static void Fail(string message)
        {
            throw new AssertionException(message);
        }

        /// <summary>
        /// Asserts that a condition is true. If not, throws an <see cref="AssertionException"/> with the given message.
        /// </summary>
        /// <param name="condition">The condition to test for.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="AssertionException">If condition is false.</exception>
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                Fail(message);
            }
        }

        /// <summary>
        /// Asserts that two objects are equal. If not, an <see cref="AssertionException"/> is thrown with the given message.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="AssertionException">If objects are not equal.</exception>
        public static void AreEqual(object expected, object actual, string message)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            if (expected != null && expected.Equals(actual))
            {
                return;
            }
            Fail(String.Format("{0} [expected:<{1}> but was:<{2}>]", message, expected, actual));
        }
    }
}
