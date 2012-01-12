using System;

using Spring.Http;
using Spring.Http.Converters.Xml;
using Spring.Rest.Client;

namespace Spring.RestBucksClient.Api
{
    /// <summary>
    /// Basic REST Client for the example of the book "REST in practice": http://restinpractice.com/. 
    /// Uses http://restbuckson.net/ for the server side.
    /// </summary>
    public class RestBucksClient
    {
        private const string BaseUrl = "http://restbuckson.net/";

        private RestTemplate restTemplate;

        /// <summary>
        /// Gets a reference to the REST client backing this API binding and used to perform API calls. 
        /// </summary>
        public RestTemplate RestTemplate
        {
            get { return this.restTemplate; }
        }

        public RestBucksClient()
        {
            // Create and configure RestTemplate
            restTemplate = new RestTemplate(BaseUrl);
            DataContractHttpMessageConverter xmlConverter = new DataContractHttpMessageConverter();
            xmlConverter.SupportedMediaTypes = new MediaType[]{ new MediaType("application", "vnd.restbucks+xml") };
            restTemplate.MessageConverters.Add(xmlConverter);
        }


        // Methods

        public Uri CreateOrder(Order order)
        {
            return restTemplate.PostForLocation("orders", order);
        }

        public Order GetOrder(Uri orderUri)
        {
            return restTemplate.GetForObject<Order>(orderUri);
        }
    }
}
