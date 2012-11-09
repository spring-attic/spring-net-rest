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

namespace Spring.Http
{
    /// <summary>
    /// Miscellaneous HTTP utility methods.
    /// </summary>
    /// <author>Bruno Baia</author>
    public static class HttpUtils
    {
        /// <summary>
        /// Decodes 'application/x-www-form-urlencoded' data.
        /// </summary>
        /// <param name="s">The string to decode.</param>
        /// <returns>The decoded string.</returns>
        public static string FormDecode(string s)
        {
            if (s == null)
            {
                return null;
            }
            return Uri.UnescapeDataString(s.Replace('+', ' '));
        }

        /// <summary>
        /// Encodes 'application/x-www-form-urlencoded' data.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string FormEncode(string s)
        {
            if (s == null)
            {
                return null;
            }
            return UrlEncode(s).Replace("%20", "+");
        }

        /// <summary>
        /// Decodes URI data according to RFC 3986.
        /// </summary>
        /// <param name="s">The string to decode.</param>
        /// <returns>The decoded string.</returns>
        public static string UrlDecode(string s)
        {
            if (s == null)
            {
                return null;
            }
            return Uri.UnescapeDataString(s);
        }

        /// <summary>
        /// Encodes URI data according to RFC 3986.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string UrlEncode(string s)
        {
            if (s == null)
            {
                return null;
            }
            return Uri.EscapeDataString(s)
                .Replace("!", "%21")
                .Replace("'", "%27")
                .Replace("(", "%28")
                .Replace(")", "%29")
                .Replace("*", "%2A");
        }
    }
}