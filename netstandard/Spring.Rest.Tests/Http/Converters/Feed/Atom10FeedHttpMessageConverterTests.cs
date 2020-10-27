﻿#if NET_3_5
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
using System.ServiceModel.Syndication;

using NUnit.Framework;

namespace Spring.Http.Converters.Feed
{
    /// <summary>
    /// Unit tests for the Atom10FeedHttpMessageConverter class.
    /// </summary>
    /// <author>Bruno Baia</author>
    [TestFixture]
    public class Atom10FeedHttpMessageConverterTests
    {
        private Atom10FeedHttpMessageConverter converter;

	    [SetUp]
	    public void SetUp() 
        {
            converter = new Atom10FeedHttpMessageConverter();
	    }

        [Test]
        public void CanRead() 
        {
            Assert.IsTrue(converter.CanRead(typeof(SyndicationFeed), new MediaType("application", "atom+xml")));
            Assert.IsTrue(converter.CanRead(typeof(SyndicationItem), new MediaType("application", "atom+xml")));
            Assert.IsFalse(converter.CanRead(typeof(string), new MediaType("application", "atom+xml")));
            Assert.IsFalse(converter.CanRead(typeof(SyndicationFeed), new MediaType("text", "plain")));
        }

        [Test]
        public void CanWrite() 
        {
            Assert.IsTrue(converter.CanWrite(typeof(SyndicationFeed), new MediaType("application", "atom+xml")));
            Assert.IsTrue(converter.CanWrite(typeof(SyndicationItem), new MediaType("application", "atom+xml")));
            Assert.IsFalse(converter.CanWrite(typeof(string), new MediaType("application", "atom+xml")));
            Assert.IsFalse(converter.CanWrite(typeof(SyndicationFeed), new MediaType("text", "plain")));
        }

        [Test]
        public void Read()
        {
            DateTimeOffset offset = DateTimeOffset.UtcNow;

            string body = String.Format("<feed xmlns=\"http://www.w3.org/2005/Atom\"><title type=\"text\">Test Feed</title><subtitle type=\"text\">This is a test feed</subtitle><id>Atom10FeedHttpMessageConverterTests.Write</id><rights type=\"text\">Copyright 2010</rights><updated>{0}</updated><author><name>Bruno Baïa</name><uri>http://www.springframework.net/bbaia</uri><email>bruno.baia@springframework.net</email></author><link rel=\"alternate\" href=\"http://www.springframework.net/Feed\" /></feed>",
                offset.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));

            MockHttpInputMessage message = new MockHttpInputMessage(body, Encoding.UTF8);

            SyndicationFeed result = converter.Read<SyndicationFeed>(message);
            Assert.IsNotNull(result, "Invalid result");
            Assert.AreEqual("Atom10FeedHttpMessageConverterTests.Write", result.Id, "Invalid result");
            Assert.AreEqual("Test Feed", result.Title.Text, "Invalid result");
            Assert.AreEqual("This is a test feed", result.Description.Text, "Invalid result");
            Assert.IsTrue(result.Links.Count == 1, "Invalid result");
            Assert.AreEqual(new Uri("http://www.springframework.net/Feed"), result.Links[0].Uri, "Invalid result");
            Assert.AreEqual("Copyright 2010", result.Copyright.Text, "Invalid result");
            Assert.IsTrue(result.Authors.Count == 1, "Invalid result");
            Assert.AreEqual("Bruno Baïa", result.Authors[0].Name, "Invalid result");
            Assert.AreEqual("bruno.baia@springframework.net", result.Authors[0].Email, "Invalid result");
            Assert.AreEqual("http://www.springframework.net/bbaia", result.Authors[0].Uri, "Invalid result");
        }

        [Test]
        public void Write()
        {
            DateTimeOffset offset = DateTimeOffset.UtcNow;

            string expectedBody = String.Format("<feed xmlns=\"http://www.w3.org/2005/Atom\"><title type=\"text\">Test Feed</title><subtitle type=\"text\">This is a test feed</subtitle><id>Atom10FeedHttpMessageConverterTests.Write</id><rights type=\"text\">Copyright 2010</rights><updated>{0}</updated><author><name>Bruno Baïa</name><uri>http://www.springframework.net/bbaia</uri><email>bruno.baia@springframework.net</email></author><link rel=\"alternate\" href=\"http://www.springframework.net/Feed\" /></feed>", 
                offset.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
            
            SyndicationFeed body = new SyndicationFeed("Test Feed", "This is a test feed", new Uri("http://www.springframework.net/Feed"), "Atom10FeedHttpMessageConverterTests.Write", offset);
            SyndicationPerson sp = new SyndicationPerson("bruno.baia@springframework.net", "Bruno Baïa", "http://www.springframework.net/bbaia");
            body.Authors.Add(sp);
            body.Copyright = new TextSyndicationContent("Copyright 2010");

            MockHttpOutputMessage message = new MockHttpOutputMessage();

            converter.Write(body, null, message);

            Assert.AreEqual(expectedBody, message.GetBodyAsString(Encoding.UTF8), "Invalid result");
            Assert.AreEqual(new MediaType("application", "atom+xml"), message.Headers.ContentType, "Invalid content-type");
            //Assert.IsTrue(message.Headers.ContentLength > -1, "Invalid content-length");
        }
    }
}
#endif
