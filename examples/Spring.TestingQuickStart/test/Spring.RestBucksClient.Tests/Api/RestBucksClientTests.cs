using System;
using System.Collections.Generic;

using NUnit.Framework; // Here we are using NUnit, but any other unit test framework will work as well.

using Spring.IO;
using Spring.Http;
using Spring.Rest.Client.Testing;

namespace Spring.RestBucksClient.Api
{
    /// <summary>
    /// Unit tests for the RestBucksClient class.
    /// </summary>
    [TestFixture]
    public sealed class RestBucksClientTests
    {
        private RestBucksClient restBucksClient;
        private MockRestServiceServer mockServer;
        private HttpHeaders responseHeaders;

        [SetUp]
        public void Setup()
        {
            restBucksClient = new RestBucksClient();
            mockServer = MockRestServiceServer.CreateServer(this.restBucksClient.RestTemplate);

            responseHeaders = new HttpHeaders();
            responseHeaders.ContentType = new MediaType("application", "vnd.restbucks+xml");
        }

        [TearDown]
        public void TearDown()
        {
            this.mockServer.Verify();
        }


        // Test methods

        [Test]
        public void CreateOrder()
        {
            responseHeaders.Location = new Uri("http://restbucks.com/order/123456");
            mockServer.ExpectNewRequest()
                .AndExpectUri("http://restbuckson.net/orders")
                .AndExpectMethod(HttpMethod.POST)
                .AndExpectBody("<order xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://restbucks.com\"><location>inShop</location><items><item><name>Latte</name><quantity>1</quantity></item></items></order>")
                .AndRespondWith("", responseHeaders);

            Order order = new Order()
            {
                Location = "inShop",
                Items = new List<OrderItem>() 
                {
                    new OrderItem() 
                    {
                        Name = "Latte",
                        Quantity = 1
                    }
                }
            };
            Uri uri = restBucksClient.CreateOrder(order);
            Assert.AreEqual(responseHeaders.Location, uri);
        }

        [Test]
        public void GetOrder()
        {
            Uri orderUri = new Uri("http://restbuckson.net/order/123456");
            mockServer.ExpectNewRequest()
                .AndExpectUri(orderUri)
                .AndExpectMethod(HttpMethod.GET)
                .AndRespondWith(XmlResource("Order"), responseHeaders);

            Order order = restBucksClient.GetOrder(orderUri);
            Assert.IsNotNull(order);
            Assert.AreEqual("inShop", order.Location);
            Assert.AreEqual(7.6, order.Cost);
            Assert.AreEqual(1, order.Items.Count);
            Assert.AreEqual("Latte", order.Items[0].Name);
            Assert.AreEqual(1, order.Items[0].Quantity);
            Assert.AreEqual("unpaid", order.Status);
        }


        // Helper methods

        private static IResource XmlResource(string filename)
        {
            return new AssemblyResource(filename + ".xml", typeof(RestBucksClientTests));
            // or
            //return new AssemblyResource("assembly://Spring.RestBucksClient.Tests/Spring.RestBucksClient.Api/" + filename + ".xml");
        }
    }
}
