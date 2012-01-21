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
using System.Text;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

using Spring.Util;

namespace Spring.Http
{
    /// <summary>
    /// Represents an Internet Media Type, as defined in the HTTP specification. 
    /// <a href="http://tools.ietf.org/html/rfc2616#section-3.7">HTTP 1.1, section 3.7</a>
    /// </summary>
    /// <remarks>
    /// Consists of a <see cref="P:Type"/> and a <see cref="P:SubType"/>.  
    /// Also has functionality to parse media types from a string using <see cref="M:Parse(string)"/>.
    /// </remarks>
    /// <author>Arjen Poutsma</author>
    /// <author>Juergen Hoeller</author>
    /// <author>Bruno Baia (.NET)</author>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class MediaType : IEquatable<MediaType>, IComparable<MediaType> 
    {
        /// <summary>
        /// The media type that includes all media ranges (i.e. '*/*').
        /// </summary>
        public static readonly MediaType ALL = new MediaType("*", "*");

        /// <summary>
        /// The media type for 'application/atom+xml'.
        /// </summary>
        public static readonly MediaType APPLICATION_ATOM_XML = new MediaType("application", "atom+xml");

        /// <summary>
        /// The media type for 'application/x-www-form-urlencoded'.
        /// </summary>
        public static readonly MediaType APPLICATION_FORM_URLENCODED = new MediaType("application", "x-www-form-urlencoded");

        /// <summary>
        /// The media type for 'application/json'.
        /// </summary>
        public static readonly MediaType APPLICATION_JSON = new MediaType("application", "json");

        /// <summary>
        /// The media type for 'application/octet-stream'.
        /// </summary>
        public static readonly MediaType APPLICATION_OCTET_STREAM = new MediaType("application", "octet-stream");

        /// <summary>
        /// The media type for 'application/xhtml+xml'.
        /// </summary>
        public static readonly MediaType APPLICATION_XHTML_XML = new MediaType("application", "xhtml+xml");

        /// <summary>
        /// The media type for 'image/gif'.
        /// </summary>
        public static readonly MediaType IMAGE_GIF = new MediaType("image", "gif");

        /// <summary>
        /// The media type for 'image/jpeg'.
        /// </summary>
        public static readonly MediaType IMAGE_JPEG = new MediaType("image", "jpeg");

        /// <summary>
        /// The media type for 'image/png'.
        /// </summary>
        public static readonly MediaType IMAGE_PNG = new MediaType("image", "png");

        /// <summary>
        /// The media type for 'image/xml'.
        /// </summary>
        public static readonly MediaType APPLICATION_XML = new MediaType("application", "xml");

        /// <summary>
        /// The media type for 'multipart/form-data'.
        /// </summary>
        public static readonly MediaType MULTIPART_FORM_DATA = new MediaType("multipart", "form-data");

        /// <summary>
        /// The media type for 'text/html'.
        /// </summary>
        public static readonly MediaType TEXT_HTML = new MediaType("text", "html");

        /// <summary>
        /// The media type for 'text/plain'.
        /// </summary>
        public static readonly MediaType TEXT_PLAIN = new MediaType("text", "plain");

        /// <summary>
        /// The media type for 'text/xml'.
        /// </summary>
        public static readonly MediaType TEXT_XML = new MediaType("text", "xml");

        private const string WILDCARD_TYPE = "*";
        private const string PARAM_QUALITY_FACTOR = "q";
        private const string PARAM_CHARSET = "charset";

        private string type;
        private string subtype;
        private IDictionary<string, string> parameters;

        // Stable sort management
        private int SortIndex = 0;

        /// <summary>
        /// Gets the primary type.
        /// </summary>
        public string Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the subtype.
        /// </summary>
        public string Subtype
        {
            get { return this.subtype; }
        }

        /// <summary>
        /// Indicate whether the type is the wildcard character '*', or not.
        /// </summary>
        public bool IsWildcardType
        {
            get { return WILDCARD_TYPE == type; }
        }

        /// <summary>
        /// Indicate whether the subtype is the wildcard character '*', or not.
        /// </summary>
        public bool IsWildcardSubtype
        {
            get { return WILDCARD_TYPE == subtype; }
        }

        /// <summary>
        /// Gets the character set, as indicated by a 'charset' parameter, if any.
        /// </summary>
        public Encoding CharSet
        {
            get
            {
                string charSet = null;
                this.parameters.TryGetValue(PARAM_CHARSET, out charSet);
                return charSet != null ? Encoding.GetEncoding(charSet) : null;
            }
        }

        /// <summary>
        /// Gets the quality value, as indicated by a 'q' parameter, if any. 
        /// Defaults to '1.0'.
        /// </summary>
        public double QualityValue
        {
            get
            {
                string qualityFactory = null;
                return this.parameters.TryGetValue(PARAM_QUALITY_FACTOR, out qualityFactory) 
                    ? Double.Parse(qualityFactory, CultureInfo.InvariantCulture) 
                    : 1D;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="MediaType"/> for the given primary type. 
        /// The subtype is set to '*', parameters are empty.
        /// </summary>
        /// <param name="type">The primary type.</param>
        public MediaType(string type) :
            this(type, WILDCARD_TYPE)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MediaType"/> for the given primary type and subtype. 
        /// The parameters are empty.
        /// </summary>
        /// <param name="type">The primary type.</param>
        /// <param name="subtype">The subtype.</param>
        public MediaType(string type, string subtype) :
            this(type, subtype, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase))
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MediaType"/> for the given primary type, subtype and character set. 
        /// </summary>
        /// <param name="type">The primary type.</param>
        /// <param name="subtype">The subtype.</param>
        /// <param name="charSet">The character set</param>
        public MediaType(string type, string subtype, string charSet) :
            this(type, subtype)
        {
            this.parameters.Add(PARAM_CHARSET, charSet);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MediaType"/> for the given primary type, subtype and character set. 
        /// </summary>
        /// <param name="type">The primary type.</param>
        /// <param name="subtype">The subtype.</param>
        /// <param name="charSet">The character set</param>
        public MediaType(string type, string subtype, Encoding charSet) :
            this(type, subtype)
        {
            this.parameters.Add(PARAM_CHARSET, charSet.WebName);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MediaType"/> for the given primary type, subtype and quality value. 
        /// </summary>
        /// <param name="type">The primary type.</param>
        /// <param name="subtype">The subtype.</param>
        /// <param name="qualityValue">The quality value</param>
        public MediaType(String type, String subtype, double qualityValue) :
            this(type, subtype)
        {
            this.parameters.Add(PARAM_QUALITY_FACTOR, qualityValue.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Creates a new instance of <see cref="MediaType"/> by copying the type and subtype of the given MediaType, 
        /// and allows for different parameter.
        /// </summary>
        /// <param name="otherMediaType">The other media type.</param>
        /// <param name="parameters">The parameters, may be null.</param>
        public MediaType(MediaType otherMediaType, IDictionary<string, string> parameters) :
            this(otherMediaType.Type, otherMediaType.Subtype, parameters)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MediaType"/> for the given primary type, subtype and parameters. 
        /// </summary>
        /// <param name="type">The primary type.</param>
        /// <param name="subtype">The subtype.</param>
        /// <param name="parameters">The parameters, may be null.</param>
        public MediaType(string type, string subtype, IDictionary<string, string> parameters)
        {
            ArgumentUtils.AssertHasText(type, "type");
            ArgumentUtils.AssertHasText(subtype, "subtype");

            // TODO: check type (http://tools.ietf.org/html/rfc2616#section-2.2)
            this.type = type.ToLower(CultureInfo.InvariantCulture);
            // TODO: check subtype (http://tools.ietf.org/html/rfc2616#section-2.2)
            this.subtype = subtype.ToLower(CultureInfo.InvariantCulture);
            // TODO: check parameters (http://tools.ietf.org/html/rfc2616#section-2.2)
            this.parameters = new Dictionary<string, string>(parameters, StringComparer.OrdinalIgnoreCase);
        }

        #region IEquatable<MediaType> Members

        /// <summary>
        /// Indicates whether the current media type is equal to another <see cref="MediaType"/>.
        /// </summary>
        /// <param name="other">A <see cref="MediaType"/> to compare with this media type.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="MediaType"/> is equal to the current media type; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(MediaType other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (this.type == other.type && this.subtype == other.subtype)
            {
                if (other.parameters.Count == this.parameters.Count)
                {
                    foreach (string key in this.parameters.Keys)
                    {
                        if (!other.parameters.ContainsKey(key) ||
                            !String.Equals(other.parameters[key], this.parameters[key], StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Indicates whether the current media type is equal to another <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> to compare with this media type.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="T:System.Object"/> is equal to the current media type; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as MediaType);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <remarks>
        /// This method is suitable for use in hashing algorithms and data structures like a hash table.
        /// </remarks>
        /// <returns>
        /// A hash code for the current media type.
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().ToUpper(CultureInfo.InvariantCulture).GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current media type.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current media type.
        /// </returns>
        public override string ToString() 
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.type);
            builder.Append('/');
            builder.Append(this.subtype);
            foreach(string key in this.parameters.Keys) 
            {
                builder.Append(';');
                builder.Append(key);
                builder.Append('=');
                builder.Append(this.parameters[key]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Determines whether two specified media types have the same value.
        /// </summary>
        /// <param name="mediaType1">The first media type to compare, or <see langword="null"/>.</param>
        /// <param name="mediaType2">The second media type to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if values are the same; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(MediaType mediaType1, MediaType mediaType2)
        {
            if (Object.ReferenceEquals(mediaType1, null))
            {
                return Object.ReferenceEquals(mediaType2, null);
            }
            return mediaType1.Equals(mediaType2);
        }

        /// <summary>
        /// Determines whether two specified media types have different values.
        /// </summary>
        /// <param name="mediaType1">The first media type to compare, or <see langword="null"/>.</param>
        /// <param name="mediaType2">The second media type to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if values are differents; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(MediaType mediaType1, MediaType mediaType2)
        {
            return !(mediaType1 == mediaType2);
        }

        /// <summary>
        /// Return a generic parameter value, given a parameter name.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <returns>The parameter value; or null if not present.</returns>
        public string GetParameter(string name)
        {
            return this.parameters[name];
        }

        /// <summary>
        /// Indicate whether this <see cref="T:MediaType"/> includes the given media type.
        /// </summary>
        /// <remarks>
        /// For instance, 'text/*' includes 'text/plain', 'text/html', and 
        /// 'application/*+xml' includes 'application/soap+xml', etc. 
        /// This method is non-symmetric.
        /// </remarks>
        /// <param name="otherMediaType">The reference media type with which to compare.</param>
        /// <returns>
        /// <see langword="true"/> if this media type includes the given media type; otherwise <see langword="false"/>.
        /// </returns>
        public bool Includes(MediaType otherMediaType)
        {
            if (otherMediaType == null)
            {
                return false;
            }
            if (this.IsWildcardType)
            {
                // */* includes anything
                return true;
            }
            else if (this.type == otherMediaType.type)
            {
                if (this.subtype == otherMediaType.subtype || this.IsWildcardSubtype)
                {
                    return true;
                }
                // application/*+xml includes application/soap+xml
                int thisPlusIdx = this.subtype.IndexOf('+');
                int otherPlusIdx = otherMediaType.subtype.IndexOf('+');
                if (thisPlusIdx != -1 && otherPlusIdx != -1)
                {
                    string thisSubtypeNoSuffix = this.subtype.Substring(0, thisPlusIdx);

                    string thisSubtypeSuffix = this.subtype.Substring(thisPlusIdx + 1);
                    string otherSubtypeSuffix = otherMediaType.subtype.Substring(otherPlusIdx + 1);
                    if (thisSubtypeSuffix == otherSubtypeSuffix && WILDCARD_TYPE == thisSubtypeNoSuffix)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Indicate whether this <see cref="T:MediaType"/> is compatible with the given media type.
        /// </summary>
        /// <remarks>
        /// For instance, 'text/*' is compatible 'text/plain', 'text/html', and vice versa.
        /// In effect, this method is similar to <see cref="M:Includes(MediaType)"/>, except that it's symmetric.
        /// </remarks>
        /// <param name="otherMediaType">The reference media type with which to compare.</param>
        /// <returns>
        /// <see langword="true"/> if this media type is compatible with the given media type; otherwise <see langword="false"/>.
        /// </returns>
        public bool IsCompatibleWith(MediaType otherMediaType)
        {
            if (otherMediaType == null)
            {
                return false;
            }
            if (this.IsWildcardType || otherMediaType.IsWildcardType)
            {
                return true;
            }
            else if (this.type == otherMediaType.type)
            {
                if (this.subtype == otherMediaType.subtype || this.IsWildcardSubtype || otherMediaType.IsWildcardSubtype)
                {
                    return true;
                }
                // application/*+xml is compatible with application/soap+xml, and vice-versa
                int thisPlusIdx = this.subtype.IndexOf('+');
                int otherPlusIdx = otherMediaType.subtype.IndexOf('+');
                if (thisPlusIdx != -1 && otherPlusIdx != -1)
                {
                    string thisSubtypeNoSuffix = this.subtype.Substring(0, thisPlusIdx);
                    string otherSubtypeNoSuffix = otherMediaType.subtype.Substring(0, otherPlusIdx);

                    string thisSubtypeSuffix = this.subtype.Substring(thisPlusIdx + 1);
                    string otherSubtypeSuffix = otherMediaType.subtype.Substring(otherPlusIdx + 1);

                    if (thisSubtypeSuffix == otherSubtypeSuffix &&
                        (WILDCARD_TYPE == thisSubtypeNoSuffix || WILDCARD_TYPE == otherSubtypeNoSuffix))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #region IComparable<MediaType> Members

        /// <summary>
        /// Compares this <see cref="MediaType"/> to another alphabetically.
        /// </summary>
        /// <param name="other">The media type to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects 
        /// being compared. The return value has the following meanings: Value Meaning 
        /// Less than zero This object is less than the other parameter.  Zero This object 
        /// is equal to other. Greater than zero This object is greater than other.
        /// </returns>
        public int CompareTo(MediaType other)
        {
            int comp = this.type.CompareTo(other.type);
            if (comp != 0)
            {
                return comp;
            }
            comp = this.subtype.CompareTo(other.subtype);
            if (comp != 0)
            {
                return comp;
            }
            comp = this.parameters.Count - other.parameters.Count;
            if (comp != 0)
            {
                return comp;
            }
            string[] thisKeys = new string[this.parameters.Keys.Count];
            this.parameters.Keys.CopyTo(thisKeys, 0);
            Array.Sort(thisKeys, StringComparer.OrdinalIgnoreCase);
            string[] otherKeys = new string[other.parameters.Keys.Count];
            other.parameters.Keys.CopyTo(otherKeys, 0);
            Array.Sort(otherKeys, StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < this.parameters.Count; i++)
            {
                string thisKey = thisKeys[i];
                string otherKey = otherKeys[i];
                comp = String.Compare(thisKey, otherKey, StringComparison.OrdinalIgnoreCase);
                if (comp != 0)
                {
                    return comp;
                }
                comp = String.Compare(this.parameters[thisKey], other.parameters[otherKey]);
                if (comp != 0)
                {
                    return comp;
                }
            }
            return this.SortIndex.CompareTo(other.SortIndex); // Stable sort
        }

        #endregion

        /// <summary>
        /// Parse the given String into a single <see cref="MediaType"/>.
        /// </summary>
        /// <remarks>
        /// This method can be used to parse a 'Content-Type' header.
        /// </remarks>
        /// <param name="mediaType">The string to parse.</param>
        /// <returns>The media type.</returns>
        public static MediaType Parse(string mediaType)
        {
            if (!StringUtils.HasText(mediaType))
            {
                return null;
            }
            
            string[] parts = mediaType.Split(';');
            string fullType = parts[0].Trim();
            if (fullType == WILDCARD_TYPE)
            {
                fullType = "*/*";
            }
            int subIndex = fullType.IndexOf('/');
            if (subIndex == -1)
            {
                throw new ArgumentException(
                    String.Format("'{0}' does not contain '/'", mediaType), 
                    "mediaType");
            }
            if (subIndex == fullType.Length - 1)
            {
                throw new ArgumentException(
                   String.Format("'{0}' does not contain subtype after '/'", mediaType),
                   "mediaType");
            }
            string type = fullType.Substring(0, subIndex);
            string subtype = fullType.Substring(subIndex + 1);

            IDictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (parts.Length > 1)
            {
                for (int i = 1; i < parts.Length; i++)
                {
                    string parameter = parts[i].Trim();
                    int eqIndex = parameter.IndexOf('=');
                    if (eqIndex != -1)
                    {
                        string attribute = parameter.Substring(0, eqIndex);
                        string value = parameter.Substring(eqIndex + 1);
                        parameters.Add(attribute, value);
                    }
                }
            }

            return new MediaType(type, subtype, parameters);
        }

        /// <summary>
        /// Return a string representation of the given list of <see cref="MediaType"/> objects.
        /// </summary>
        /// <remarks>
        /// This method can be used to for an 'Accept' or 'Content-Type' header.
        /// </remarks>
        /// <param name="mediaTypes">The list of media types to convert.</param>
        /// <returns>The string representation of the given list.</returns>
        public static string ToString(IEnumerable<MediaType> mediaTypes)
        {
            StringBuilder builder = new StringBuilder();
            foreach(MediaType mediaType in mediaTypes)
            {
                if (builder.Length > 0)
                {
                    builder.Append(',');
                }
                builder.Append(mediaType);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Sorts the given list of <see cref="MediaType"/> objects by specificity. 
        /// <a href="http://tools.ietf.org/html/rfc2616#section-14.1">HTTP 1.1, section 14.1</a>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Given two media types:
        /// <ol>
        ///     <li>if either media type has a wildcard type, then the media type without the 
        ///     wildcard is ordered before the other.</li>
        ///     <li>if the two media types have different types, then they are considered equal and 
        ///     remain their current order.</li>
        ///     <li>if either media type has a wildcard subtype, then the media type without 
        ///     the wildcard is sorted before the other.</li>
        ///     <li>if the two media types have different subtypes, then they are considered equal 
        ///     and remain their current order.</li>
        ///     <li>if the two media types have different quality value, then the media type 
        ///     with the highest quality value is ordered before the other.</li>
        ///     <li>if the two media types have a different amount of parameters, then the 
        ///     media type with the most parameters is ordered before the other.</li>
        /// </ol>
        /// </para>
        /// <para>
        /// For example:
        /// <blockquote>audio/basic &lt; audio/* &lt; *&#047;*</blockquote>
        /// <blockquote>audio/* &lt; audio/*;q=0.7; audio/*;q=0.3</blockquote>
        /// <blockquote>audio/basic;level=1 &lt; audio/basic</blockquote>
        /// <blockquote>audio/basic == text/html</blockquote>
        /// <blockquote>audio/basic == audio/wave</blockquote>
        /// </para>
        /// </remarks>
        /// <param name="mediaTypes">The list of media types to be sorted.</param>
        public static void SortBySpecificity(List<MediaType> mediaTypes)
        {
            ArgumentUtils.AssertNotNull(mediaTypes, "mediaTypes");

            if (mediaTypes.Count > 1)
            {
                // Stable sort
                for(int i = 0; i < mediaTypes.Count; i++)
                {
                    mediaTypes[i].SortIndex = i;
                }
                mediaTypes.Sort(SPECIFICITY_COMPARER);
            }
        }

        /// <summary>
        /// Sorts the given list of <see cref="MediaType"/> objects by quality value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Given two media types:
        /// <ol>
        ///     <li>if the two media types have different quality value, then the media type 
        ///     with the highest quality value is ordered before the other.</li>
        ///     <li>if either media type has a wildcard type, then the media type without the 
        ///     wildcard is ordered before the other.</li>
        ///     <li>if the two media types have different types, then they are considered equal and 
        ///     remain their current order.</li>
        ///     <li>if either media type has a wildcard subtype, then the media type without 
        ///     the wildcard is sorted before the other.</li>
        ///     <li>if the two media types have different subtypes, then they are considered equal 
        ///     and remain their current order.</li>
        ///     <li>if the two media types have a different amount of parameters, then the 
        ///     media type with the most parameters is ordered before the other.</li>
        /// </ol>
        /// </para>
        /// </remarks>
        /// <param name="mediaTypes">The list of media types to be sorted</param>
        public static void SortByQualityValue(List<MediaType> mediaTypes)
        {
            ArgumentUtils.AssertNotNull(mediaTypes, "mediaTypes");

            if (mediaTypes.Count > 1)
            {
                // Stable sort
                for (int i = 0; i < mediaTypes.Count; i++)
                {
                    mediaTypes[i].SortIndex = i;
                }
                mediaTypes.Sort(QUALITY_VALUE_COMPARER);
            }
        }

        /// <summary>
        /// <see cref="IComparer&lt;MediaType>"/> implementation by specificity value.
        /// </summary>
        public static IComparer<MediaType> SPECIFICITY_COMPARER = new SpecificityComparer();

        /// <summary>
        /// <see cref="IComparer&lt;MediaType>"/> implementation by quality value.
        /// </summary>
        public static IComparer<MediaType> QUALITY_VALUE_COMPARER = new QualityValueComparer();

        #region SpecificityComparer

        private class SpecificityComparer : IComparer<MediaType>
        {
            public int Compare(MediaType x, MediaType y)
            {
                if (x.IsWildcardType && !y.IsWildcardType)
                { // */* < audio/*
                    return 1;
                }
                else if (y.IsWildcardType && !x.IsWildcardType)
                { // audio/* > */*
                    return -1;
                }
                else if (x.type != y.type)
                { // audio/basic == text/html
                    return x.SortIndex.CompareTo(y.SortIndex); // Stable sort
                }
                else
                { // mediaType1.type == mediaType2.type
                    if (x.IsWildcardSubtype && !y.IsWildcardSubtype)
                    { // audio/* < audio/basic
                        return 1;
                    }
                    else if (y.IsWildcardSubtype && !x.IsWildcardSubtype)
                    { // audio/basic > audio/*
                        return -1;
                    }
                    else if (x.subtype != y.subtype)
                    { // audio/basic == audio/wave
                        return x.SortIndex.CompareTo(y.SortIndex); // Stable sort
                    }
                    else
                    { // mediaType2.subtype == mediaType2.subtype
                        double quality1 = x.QualityValue;
                        double quality2 = y.QualityValue;
                        int qualityComparison = quality2.CompareTo(quality1);
                        if (qualityComparison != 0)
                        {
                            return qualityComparison;  // audio/*;q=0.7 < audio/*;q=0.3
                        }
                        else
                        {
                            int paramsSize1 = x.parameters.Count;
                            int paramsSize2 = y.parameters.Count;
                            int comp = (paramsSize2 < paramsSize1 ? -1 : (paramsSize2 == paramsSize1 ? 0 : 1)); // audio/basic;level=1 < audio/basic
                            if (comp != 0)
                            {
                                return comp;
                            }
                            return x.SortIndex.CompareTo(y.SortIndex); // Stable sort
                        }
                    }
                }
            }
        }

        #endregion

        #region QualityValueComparer

        private class QualityValueComparer : IComparer<MediaType>
        {
            public int Compare(MediaType x, MediaType y)
            {
                double quality1 = x.QualityValue;
                double quality2 = y.QualityValue;
                int qualityComparison = quality2.CompareTo(quality1);
                if (qualityComparison != 0)
                {
                    return qualityComparison;  // audio/*;q=0.7 < audio/*;q=0.3
                }
                else if (x.IsWildcardType && !y.IsWildcardType)
                { // */* < audio/*
                    return 1;
                }
                else if (y.IsWildcardType && !x.IsWildcardType)
                { // audio/* > */*
                    return -1;
                }
                else if (x.type != y.type)
                { // audio/basic == text/html
                    return x.SortIndex.CompareTo(y.SortIndex); // Stable sort
                }
                else
                { // mediaType1.type == mediaType2.type
                    if (x.IsWildcardSubtype && !y.IsWildcardSubtype)
                    { // audio/* < audio/basic
                        return 1;
                    }
                    else if (y.IsWildcardSubtype && !x.IsWildcardSubtype)
                    { // audio/basic > audio/*
                        return -1;
                    }
                    else if (x.subtype != y.subtype)
                    { // audio/basic == audio/wave
                        return x.SortIndex.CompareTo(y.SortIndex); // Stable sort
                    }
                    else
                    {
                        int paramsSize1 = x.parameters.Count;
                        int paramsSize2 = y.parameters.Count;
                        int comp = (paramsSize2 < paramsSize1 ? -1 : (paramsSize2 == paramsSize1 ? 0 : 1)); // audio/basic;level=1 < audio/basic
                        if (comp != 0)
                        {
                            return comp;
                        }
                        return x.SortIndex.CompareTo(y.SortIndex);  // Stable sort
                    }
                }
            }
        }

        #endregion
    }
}
