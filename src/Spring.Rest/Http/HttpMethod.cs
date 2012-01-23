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

using Spring.Util;

namespace Spring.Http
{
    /// <summary>
    /// Represents an HTTP request method as defined in the HTTP specification. 
    /// <a href="http://tools.ietf.org/html/rfc2616#section-5.1.1">HTTP 1.1, section 5.1.1</a>
    /// </summary>
    /// <remarks>
    /// HTTP method equality and hashing are case insensitive.
    /// </remarks>
    /// <author>Bruno Baia</author>
#if !SILVERLIGHT && !CF_3_5
    [Serializable]
#endif
    public class HttpMethod : IEquatable<HttpMethod>
    {
        /// <summary>
        /// The OPTIONS method.
        /// </summary>
        public static readonly HttpMethod OPTIONS = new HttpMethod("OPTIONS");

        /// <summary>
        /// The GET method.
        /// </summary>
        public static readonly HttpMethod GET = new HttpMethod("GET");

        /// <summary>
        /// The HEAD method.
        /// </summary>
        public static readonly HttpMethod HEAD = new HttpMethod("HEAD");

        /// <summary>
        /// The POST method.
        /// </summary>
        public static readonly HttpMethod POST = new HttpMethod("POST");

        /// <summary>
        /// The PUT method.
        /// </summary>
        public static readonly HttpMethod PUT = new HttpMethod("PUT");

        /// <summary>
        /// The DELETE method.
        /// </summary>
        public static readonly HttpMethod DELETE = new HttpMethod("DELETE");

        /// <summary>
        /// The TRACE method.
        /// </summary>
        public static readonly HttpMethod TRACE = new HttpMethod("TRACE");

        /// <summary>
        /// The CONNECT method.
        /// </summary>
        public static readonly HttpMethod CONNECT = new HttpMethod("CONNECT");

        private string method;

        /// <summary>
        /// Creates a new instance of <see cref="HttpMethod"/> with the given HTTP method value.
        /// </summary>
        /// <param name="method">The HTTP method as a string value.</param>
        public HttpMethod(string method)
        {
            ArgumentUtils.AssertNotNull(method, "method");

            // TODO: check method (http://tools.ietf.org/html/rfc2616#section-2.2)
            this.method = method;
        }

        /// <summary>
        /// Indicates whether the current HTTP method is equal to another <see cref="HttpMethod"/>.
        /// </summary>
        /// <param name="other">An <see cref="HttpMethod"/> to compare with this HTTP method.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="HttpMethod"/> is equal to the current HTTP method; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(HttpMethod other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this.method, other.method))
            {
                return true;
            }
            return String.Equals(this.method, other.method, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Indicates whether the current HTTP method is equal to another <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> to compare with this HTTP method.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="T:System.Object"/> is equal to the current HTTP method; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as HttpMethod);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <remarks>
        /// This method is suitable for use in hashing algorithms and data structures like a hash table.
        /// </remarks>
        /// <returns>
        /// A hash code for the current HTTP method.
        /// </returns>
        public override int GetHashCode()
        {
            return this.method.ToUpper(CultureInfo.InvariantCulture).GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current HTTP method.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current HTTP method.
        /// </returns>
        public override string ToString()
        {
            return this.method;
        }

        /// <summary>
        /// Determines whether two specified HTTP methods have the same value.
        /// </summary>
        /// <param name="method1">The first HTTP method to compare, or <see langword="null"/>.</param>
        /// <param name="method2">The second HTTP method to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if values are the same; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(HttpMethod method1, HttpMethod method2)
        {
            if (Object.ReferenceEquals(method1, null))
            {
                return Object.ReferenceEquals(method2, null);
            }
            return method1.Equals(method2);
        }

        /// <summary>
        /// Determines whether two specified HTTP methods have different values.
        /// </summary>
        /// <param name="method1">The first HTTP method to compare, or <see langword="null"/>.</param>
        /// <param name="method2">The second HTTP method to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if values are differents; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(HttpMethod method1, HttpMethod method2)
        {
            return !(method1 == method2);
        }
    }
}
