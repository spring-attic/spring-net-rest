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
using System.Collections.Generic;

using NUnit.Framework;

namespace Spring.Http
{
    /// <summary>
    /// Unit tests for the MediaType class.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Juergen Hoeller</author>
    /// <author>Bruno Baia (.NET)</author>
    [TestFixture]
    public class MediaTypeTests
    {
        [Test]
        public void Equals()
        {
            Assert.IsTrue(MediaType.TEXT_HTML.Equals(new MediaType("text", "html")));
            Assert.IsTrue(((object)MediaType.TEXT_HTML).Equals(new MediaType("text", "html")));
        }

        [Test]
        public void EqualsCaseInsensitive()
        {
            Assert.IsTrue(MediaType.TEXT_PLAIN.Equals(new MediaType("Text", "Plain")));
        }

        [Test]
        public void EqualsNull()
        {
            Assert.IsFalse(MediaType.IMAGE_GIF.Equals(null));
        }

        [Test]
        public void EqualsOperator()
        {
            Assert.IsTrue(MediaType.TEXT_PLAIN == new MediaType("text", "plain"));
        }

        [Test]
        public void NotEqualsOperator()
        {
            Assert.IsTrue(MediaType.TEXT_PLAIN != MediaType.TEXT_HTML);
        }

        [Test]
        public void GetHashCodeTest()
        {
            IDictionary<MediaType, string> dictionary = new Dictionary<MediaType, string>();
            dictionary.Add(MediaType.TEXT_HTML, "value for text/html");
            Assert.IsTrue(dictionary.ContainsKey(new MediaType("Text", "Html")));
        }

        [Test]
        public void Equatable()
        {
            IList<MediaType> list = new List<MediaType>();
            list.Add(MediaType.APPLICATION_JSON);
            Assert.IsTrue(list.Contains(new MediaType("application", "json")));
        }

        [Test]
        public void ToStringTest()
        {
            MediaType mediaType = new MediaType("text", "plain", 0.7);
            String result = mediaType.ToString();
            Assert.AreEqual("text/plain;q=0.7", result, "Invalid toString() returned");
        }

        [Test]
        public void Includes()
        {
            MediaType textPlain = MediaType.TEXT_PLAIN;
            Assert.IsTrue(textPlain.Includes(textPlain), "Equal types is not inclusive");
            MediaType allText = new MediaType("text");

            Assert.IsTrue(allText.Includes(textPlain), "All subtypes is not inclusive");
            Assert.IsFalse(textPlain.Includes(allText), "All subtypes is inclusive");

            Assert.IsTrue(MediaType.ALL.Includes(textPlain), "All types is not inclusive");
            Assert.IsFalse(textPlain.Includes(MediaType.ALL), "All types is inclusive");

            Assert.IsTrue(MediaType.ALL.Includes(textPlain), "All types is not inclusive");
            Assert.IsFalse(textPlain.Includes(MediaType.ALL), "All types is inclusive");

            MediaType applicationSoapXml = new MediaType("application", "soap+xml");
            MediaType applicationWildcardXml = new MediaType("application", "*+xml");

            Assert.IsTrue(applicationSoapXml.Includes(applicationSoapXml));
            Assert.IsTrue(applicationWildcardXml.Includes(applicationWildcardXml));

            Assert.IsTrue(applicationWildcardXml.Includes(applicationSoapXml));
            Assert.IsFalse(applicationSoapXml.Includes(applicationWildcardXml));
        }
	
        [Test]
        public void IsCompatible() 
        {
            MediaType textPlain = MediaType.TEXT_PLAIN;
            Assert.IsTrue(textPlain.IsCompatibleWith(textPlain), "Equal types is not compatible");
            MediaType allText = new MediaType("text");

            Assert.IsTrue(allText.IsCompatibleWith(textPlain), "All subtypes is not compatible");
            Assert.IsTrue(textPlain.IsCompatibleWith(allText), "All subtypes is not compatible");

            Assert.IsTrue(MediaType.ALL.IsCompatibleWith(textPlain), "All types is not compatible");
            Assert.IsTrue(textPlain.IsCompatibleWith(MediaType.ALL), "All types is not compatible");

            Assert.IsTrue(MediaType.ALL.IsCompatibleWith(textPlain), "All types is not compatible");
            Assert.IsTrue(textPlain.IsCompatibleWith(MediaType.ALL), "All types is compatible");

            MediaType applicationSoapXml = new MediaType("application", "soap+xml");
            MediaType applicationWildcardXml = new MediaType("application", "*+xml");

            Assert.IsTrue(applicationSoapXml.IsCompatibleWith(applicationSoapXml));
            Assert.IsTrue(applicationWildcardXml.IsCompatibleWith(applicationWildcardXml));

            Assert.IsTrue(applicationWildcardXml.IsCompatibleWith(applicationSoapXml));
            Assert.IsTrue(applicationSoapXml.IsCompatibleWith(applicationWildcardXml));
        }

        [Test]
        public void GetDefaultQualityValue()
        {
            MediaType mediaType = new MediaType("text", "plain");
            Assert.AreEqual(1, mediaType.QualityValue, "Invalid quality value");
        }

        [Test]
        public void Parse()
        {
            string s = "audio/*; q=0.2";
            MediaType mediaType = MediaType.Parse(s);
            Assert.AreEqual("audio", mediaType.Type, "Invalid type");
            Assert.AreEqual("*", mediaType.Subtype, "Invalid subtype");
            Assert.AreEqual(0.2, mediaType.QualityValue, "Invalid quality factor");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseNoSubtype() 
        {
            MediaType.Parse("audio");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseNoSubtypeSlash() 
        {
            MediaType.Parse("audio/");
        }

        [Test]
        public void ParseURLConnectionMediaType()
        {
            string s = "*; q=.2";
            MediaType mediaType = MediaType.Parse(s);
            Assert.AreEqual(mediaType.Type, "*", "Invalid type");
            Assert.AreEqual("*", mediaType.Subtype, "Invalid subtype");
            Assert.AreEqual(0.2, mediaType.QualityValue, "Invalid quality factor");
        }

        [Test]
        public void CompareTo()
        {
            MediaType audioBasic = new MediaType("audio", "basic");
            MediaType audio = new MediaType("audio");
            MediaType audioWave = new MediaType("audio", "wave");
            MediaType audioBasicLevel = new MediaType("audio", "basic", SingletonDictionary<string, string>("level", "1"));
            MediaType audioBasic07 = new MediaType("audio", "basic", 0.7);

            // equal
            Assert.AreEqual(0, audioBasic.CompareTo(audioBasic), "Invalid comparison result");
            Assert.AreEqual(0, audio.CompareTo(audio), "Invalid comparison result");
            Assert.AreEqual(0, audioBasicLevel.CompareTo(audioBasicLevel), "Invalid comparison result");

            Assert.IsTrue(audioBasicLevel.CompareTo(audio) > 0, "Invalid comparison result");

            List<MediaType> expected = new List<MediaType>();
            expected.Add(audio);
            expected.Add(audioBasic);
            expected.Add(audioBasicLevel);
            expected.Add(audioBasic07);
            expected.Add(audioWave);

            List<MediaType> result = new List<MediaType>(expected);
            // shuffle & sort 10 times
            for (int i = 0; i < 10; i++)
            {
                Shuffle<MediaType>(result);
                result.Sort();

                for (int j = 0; j < result.Count; j++)
                {
                    Assert.AreSame(expected[j], result[j], "Invalid media type at " + j + ", run " + i);
                }
            }
        }

        [Test]
        public void CompareToConsistentWithEquals()
        {
            MediaType m1 = MediaType.Parse("text/html; q=0.7; charset=iso-8859-1");
            MediaType m2 = MediaType.Parse("text/html; charset=iso-8859-1; q=0.7");

            Assert.AreEqual(m1, m2, "Media types not equal");
            Assert.AreEqual(0, m1.CompareTo(m2), "compareTo() not consistent with equals");
            Assert.AreEqual(0, m2.CompareTo(m1), "compareTo() not consistent with equals");

            m1 = MediaType.Parse("text/html; q=0.7; charset=iso-8859-1");
            m2 = MediaType.Parse("text/html; Q=0.7; charset=iso-8859-1");
            Assert.AreEqual(m1, m2, "Media types not equal");
            Assert.AreEqual(0, m1.CompareTo(m2), "compareTo() not consistent with equals");
            Assert.AreEqual(0, m2.CompareTo(m1), "compareTo() not consistent with equals");
        }

        [Test]
        public void CompareToCaseSensitivity()
        {
            MediaType m1 = new MediaType("audio", "basic");
            MediaType m2 = new MediaType("Audio", "Basic");
            Assert.AreEqual(0, m1.CompareTo(m2), "Invalid comparison result");
            Assert.AreEqual(0, m2.CompareTo(m1), "Invalid comparison result");

            m1 = new MediaType("audio", "basic", SingletonDictionary<string, string>("foo", "bar"));
            m2 = new MediaType("audio", "basic", SingletonDictionary<string, string>("Foo", "bar"));
            Assert.AreEqual(0, m1.CompareTo(m2), "Invalid comparison result");
            Assert.AreEqual(0, m2.CompareTo(m1), "Invalid comparison result");

            m1 = new MediaType("audio", "basic", SingletonDictionary<string, string>("foo", "bar"));
            m2 = new MediaType("audio", "basic", SingletonDictionary<string, string>("foo", "Bar"));
            Assert.IsTrue(m1.CompareTo(m2) != 0, "Invalid comparison result");
            Assert.IsTrue(m2.CompareTo(m1) != 0, "Invalid comparison result");
        }

        [Test]
        public void SpecificityComparator() 
        {
            MediaType audioBasic = new MediaType("audio", "basic");
            MediaType audioWave = new MediaType("audio", "wave");
            MediaType audio = new MediaType("audio");
            MediaType audio03 = new MediaType("audio", "*", 0.3);
            MediaType audio07 = new MediaType("audio", "*", 0.7);
            MediaType audioBasicLevel = new MediaType("audio", "basic", SingletonDictionary<string, string>("level", "1"));
            MediaType textHtml = new MediaType("text", "html");
            MediaType all = MediaType.ALL;

            IComparer<MediaType> comp = MediaType.SPECIFICITY_COMPARER;

            // equal
            Assert.AreEqual(0, comp.Compare(audioBasic,audioBasic), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audio, audio), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audio07, audio07), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audio03, audio03), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audioBasicLevel, audioBasicLevel), "Invalid comparison result");

            // specific to unspecific
            Assert.IsTrue(comp.Compare(audioBasic, audio) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audioBasic, all) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio, all) < 0, "Invalid comparison result");

            // unspecific to specific
            Assert.IsTrue(comp.Compare(audio, audioBasic) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(all, audioBasic) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(all, audio) > 0, "Invalid comparison result");

            // qualifiers
            Assert.IsTrue(comp.Compare(audio, audio07) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio07, audio) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio07, audio03) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio03, audio07) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio03, all) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(all, audio03) > 0, "Invalid comparison result");

            // other parameters
            Assert.IsTrue(comp.Compare(audioBasic, audioBasicLevel) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audioBasicLevel, audioBasic) < 0, "Invalid comparison result");

            // different types
            Assert.AreEqual(0, comp.Compare(audioBasic, textHtml), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(textHtml, audioBasic), "Invalid comparison result");

            // different subtypes
            Assert.AreEqual(0, comp.Compare(audioBasic, audioWave), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audioWave, audioBasic), "Invalid comparison result");
        }

        [Test]
        public void SortBySpecificityRelated()
        {
            MediaType audioBasic = new MediaType("audio", "basic");
            MediaType audio = new MediaType("audio");
            MediaType audio03 = new MediaType("audio", "*", 0.3);
            MediaType audio07 = new MediaType("audio", "*", 0.7);
            MediaType audioBasicLevel = new MediaType("audio", "basic", SingletonDictionary<string, string>("level", "1"));
            MediaType all = MediaType.ALL;

            List<MediaType> expected = new List<MediaType>();
            expected.Add(audioBasicLevel);
            expected.Add(audioBasic);
            expected.Add(audio);
            expected.Add(audio07);
            expected.Add(audio03);
            expected.Add(all);

            List<MediaType> result = new List<MediaType>(expected);
            // shuffle & sort 10 times
            for (int i = 0; i < 10; i++)
            {
                Shuffle<MediaType>(result);
                MediaType.SortBySpecificity(result);

                for (int j = 0; j < result.Count; j++)
                {
                    Assert.AreSame(expected[j], result[j], "Invalid media type at " + j);
                }
            }
        }

        [Test]
        public void SortBySpecificityUnrelated() // Check stable sort
        {
            MediaType audioBasic = new MediaType("audio", "basic");
            MediaType audioWave = new MediaType("audio", "wave");
            MediaType textHtml = new MediaType("text", "html");

            List<MediaType> expected = new List<MediaType>();
            expected.Add(textHtml);
            expected.Add(audioBasic);
            expected.Add(audioWave);

            List<MediaType> result = new List<MediaType>(expected);
            MediaType.SortBySpecificity(result);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreSame(expected[i], result[i], "Invalid media type at " + i);
            }
        }

        [Test]
        public void QualityComparator() 
        {
            MediaType audioBasic = new MediaType("audio", "basic");
            MediaType audioWave = new MediaType("audio", "wave");
            MediaType audio = new MediaType("audio");
            MediaType audio03 = new MediaType("audio", "*", 0.3);
            MediaType audio07 = new MediaType("audio", "*", 0.7);
            MediaType audioBasicLevel = new MediaType("audio", "basic", SingletonDictionary<string, string>("level", "1"));
            MediaType textHtml = new MediaType("text", "html");
            MediaType all = MediaType.ALL;

            IComparer<MediaType> comp = MediaType.QUALITY_VALUE_COMPARER;

            // equal
            Assert.AreEqual(0, comp.Compare(audioBasic, audioBasic), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audio, audio), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audio07, audio07), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audio03, audio03), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audioBasicLevel, audioBasicLevel), "Invalid comparison result");

            // specific to unspecific
            Assert.IsTrue(comp.Compare(audioBasic, audio) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audioBasic, all) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio, all) < 0, "Invalid comparison result");

            // unspecific to specific
            Assert.IsTrue(comp.Compare(audio, audioBasic) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(all, audioBasic) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(all, audio) > 0, "Invalid comparison result");

            // qualifiers
            Assert.IsTrue(comp.Compare(audio, audio07) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio07, audio) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio07, audio03) < 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio03, audio07) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audio03, all) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(all, audio03) < 0, "Invalid comparison result");

            // other parameters
            Assert.IsTrue(comp.Compare(audioBasic, audioBasicLevel) > 0, "Invalid comparison result");
            Assert.IsTrue(comp.Compare(audioBasicLevel, audioBasic) < 0, "Invalid comparison result");

            // different types
            Assert.AreEqual(0, comp.Compare(audioBasic, textHtml), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(textHtml, audioBasic), "Invalid comparison result");

            // different subtypes
            Assert.AreEqual(0, comp.Compare(audioBasic, audioWave), "Invalid comparison result");
            Assert.AreEqual(0, comp.Compare(audioWave, audioBasic), "Invalid comparison result");
        }

        [Test]
        public void SortByQualityRelated()
        {
            MediaType audioBasic = new MediaType("audio", "basic");
            MediaType audio = new MediaType("audio");
            MediaType audio03 = new MediaType("audio", "*", 0.3);
            MediaType audio07 = new MediaType("audio", "*", 0.7);
            MediaType audioBasicLevel = new MediaType("audio", "basic", SingletonDictionary<string, string>("level", "1"));
            MediaType all = MediaType.ALL;

            List<MediaType> expected = new List<MediaType>();
            expected.Add(audioBasicLevel);
            expected.Add(audioBasic);
            expected.Add(audio);
            expected.Add(all);
            expected.Add(audio07);
            expected.Add(audio03);

            List<MediaType> result = new List<MediaType>(expected);
            // shuffle & sort 10 times
            for (int i = 0; i < 10; i++)
            {
                Shuffle<MediaType>(result);
                MediaType.SortByQualityValue(result);

                for (int j = 0; j < result.Count; j++)
                {
                    Assert.AreSame(expected[j], result[j], "Invalid media type at " + j);
                }
            }
        }

        [Test]
        public void SortByQualityUnrelated() // Check stable sort
        {
            MediaType audioBasic = new MediaType("audio", "basic");
            MediaType audioWave = new MediaType("audio", "wave");
            MediaType textHtml = new MediaType("text", "html");

            List<MediaType> expected = new List<MediaType>();
            expected.Add(textHtml);
            expected.Add(audioBasic);
            expected.Add(audioWave);

            List<MediaType> result = new List<MediaType>(expected);
            MediaType.SortByQualityValue(result);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreSame(expected[i], result[i], "Invalid media type at " + i);
            }
        }

        #region Utils

        public static void Shuffle<T>(IList<T> list)
        {
            Random rnd = new Random();

            int i = list.Count;
            while (i >= 1)
            {
                i--;
                int nextIndex = rnd.Next(i, list.Count);
                T val = list[nextIndex];
                list[nextIndex] = list[i];
                list[i] = val;
            }
        }

        private static IDictionary<TKey, TValue> SingletonDictionary<TKey, TValue>(TKey key, TValue value)
        {
            IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(1);
            dictionary.Add(key, value);
            return dictionary;
        }

        #endregion
    }
}
