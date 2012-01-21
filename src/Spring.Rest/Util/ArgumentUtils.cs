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
using System.Globalization;

namespace Spring.Util
{
    // From Spring.Core.AssertUtils (Renamed to differentiate it from Core Spring.NET)

	/// <summary>
	/// Assertion utility methods that assists in validating arguments. 
    /// Useful for identifying programmer errors early and clearly at runtime.
	/// </summary>
	/// <remarks>
    /// <para>
    /// For example, if the contract of a public method states it does not allow <code>null</code> arguments, 
    /// ArgumentUtils can be used to validate that contract. 
    /// Doing this clearly indicates a contract violation when it occurs and protects the class's invariants.
    /// </para>
	/// <para>
    /// Mainly for internal use within the framework.
	/// </para>
	/// </remarks>
	/// <author>Aleksandar Seovic</author>
	/// <author>Erich Eichinger</author>
	public sealed class ArgumentUtils
	{
        /// <summary>
		/// Checks the value of the supplied <paramref name="argument"/> and throws an
		/// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/>.
		/// </summary>
		/// <param name="argument">The object to check.</param>
		/// <param name="name">The argument name.</param>
		/// <exception cref="System.ArgumentNullException">
		/// If the supplied <paramref name="argument"/> is <see langword="null"/>.
		/// </exception>
        public static void AssertNotNull(object argument, string name)
		{
			if (argument == null)
			{
				throw new ArgumentNullException (name,
                    String.Format(CultureInfo.InvariantCulture, 
                        "Argument '{0}' must not be null.", name));
			}
		}

		/// <summary>
		/// Checks the value of the supplied string <paramref name="argument"/> and throws an
        /// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/> or
		/// contains only whitespace character(s).
		/// </summary>
		/// <param name="argument">The string to check.</param>
		/// <param name="name">The argument name.</param>
		/// <exception cref="System.ArgumentNullException">
		/// If the supplied <paramref name="argument"/> is <see langword="null"/> or
		/// contains only whitespace character(s).
		/// </exception>
        public static void AssertHasText(string argument, string name)
		{
			if (!StringUtils.HasText(argument))
			{
				throw new ArgumentNullException(name,
					String.Format (CultureInfo.InvariantCulture, 
                    "String argument '{0}' must have text; it must not be null, empty, or blank.", name));
			}
		}
	}
}