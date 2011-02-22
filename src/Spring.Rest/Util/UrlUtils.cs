#if !SILVERLIGHT
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

namespace Spring.Util
{
    // Removes dependency to System.Web.dll to allow targeting .NET Framework Client profile

    /// <summary>
    /// Utility methods for Url encoding and decoding with 
    /// the <c>application/x-www-form-urlencoded</c> MIME format.
    /// </summary>
    /// <remarks>
    /// Based on the following rules:
    /// <ul>
    /// <li>Alphanumeric characters <c>'a'</c> through <c>'z'</c>, 
    /// <c>'A'</c> through <c>'Z'</c>, and <c>'0'</c> through <c>'9'</c> 
    /// stay the same.</li>
    /// <li>Special characters <c>'-'</c>, <c>'_'</c>, <c>'.'</c>, and 
    /// <c>'*'</c> stay the same.</li>
    /// <li>All other characters are converted into one or more bytes using the 
    /// given encoding scheme. Each of the resulting bytes is written as a 
    /// hexadecimal string in the <c>%xy</c> format.</li>
    /// <li>A sequence <code>%xy</code> is interpreted as a hexadecimal 
    /// representation of the character.</li>
    /// </ul>
    /// </remarks>
    /// <author>Bruno Baia</author>
    internal sealed class UrlUtils
    {
        /// <summary>
        /// Decodes the given encoded string url.
        /// </summary>
        /// <param name="url">The string url to decode</param>
        /// <param name="encoding">The encoding to use</param>
        /// <returns>The decoded url</returns>
        public static string Decode(string url, Encoding encoding)
        {
            if (url == null)
            {
                return null;
            }
            byte[] bytes = encoding.GetBytes(url);
            return encoding.GetString(Decode(bytes));
        }

        /// <summary>
        /// Encodes the given string url.
        /// </summary>
        /// <param name="url">The string url to encode</param>
        /// <param name="encoding">The encoding to use</param>
        /// <returns>The encoded url</returns>
        public static string Encode(string url, Encoding encoding)
        {
            if (url == null)
            {
                return null;
            }
            byte[] bytes = encoding.GetBytes(url);
            return encoding.GetString(Encode(bytes));
        }


        private static byte[] Decode(byte[] bytes)
        {
            int index = 0;
            byte[] decodedBytes = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                if (b == '+')
                {
                    b = (byte)' ';
                }
                else if ((b == '%') && (i < (bytes.Length - 2)))
                {
                    int hex1 = HexToInt((char)bytes[i + 1]);
                    int hex2 = HexToInt((char)bytes[i + 2]);
                    if ((hex1 >= 0) && (hex2 >= 0))
                    {
                        b = (byte)((hex1 << 4) | hex2);
                        i += 2;
                    }
                }
                decodedBytes[index++] = b;
            }
            if (index < decodedBytes.Length)
            {
                byte[] trimedDecodedBytes = new byte[index];
                Array.Copy(decodedBytes, trimedDecodedBytes, index);
                decodedBytes = trimedDecodedBytes;
            }
            return decodedBytes;
        }

        private static byte[] Encode(byte[] bytes)
        {
            int spaceCount = 0;
            int unsafeCount = 0;
            for(int i = 0; i < bytes.Length; i++)
            {
                char c = (char)bytes[i];
                if (c == ' ')
                {
                    spaceCount++;
                }
                else if (!IsSafe(c))
                {
                    unsafeCount++;
                }
            }
            if (spaceCount == 0 && unsafeCount == 0)
            {
                return bytes;
            }
            byte[] encodedBytes = new byte[bytes.Length + (unsafeCount * 2)];
            int index = 0;
            foreach (byte b in bytes)
            {
                if (IsSafe((char)b))
                {
                    encodedBytes[index++] = b;
                }
                else if (b == ' ')
                {
                    encodedBytes[index++] = (byte)'+';
                }
                else
                {
                    encodedBytes[index++] = (byte)'%';
                    encodedBytes[index++] = (byte)IntToHex((b >> 4) & 15);
                    encodedBytes[index++] = (byte)IntToHex(b & 15);
                }
            }
            return encodedBytes;
        }

        private static int HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + '0');
            }
            return (char)((n - 10) + 'a');
        }

        private static bool IsSafe(char ch)
        {
            return (
                ((ch >= 'a') && (ch <= 'z')) ||
                ((ch >= 'A') && (ch <= 'Z')) ||
                ((ch >= '0') && (ch <= '9')) ||
                ch == '-' || ch == '_' || ch == '.' || ch == '*');
        }
    }
}
#endif