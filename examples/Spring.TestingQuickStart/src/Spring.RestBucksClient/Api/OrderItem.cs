using System.Runtime.Serialization;

namespace Spring.RestBucksClient.Api
{
    [DataContract(Name = "item", Namespace = "http://restbucks.com")]
    public class OrderItem
    {
        [DataMember(Name = "name", Order = 0)]
        public string Name { get; set; }

        [DataMember(Name = "quantity", Order = 1)]
        public int Quantity { get; set; }
    }
}
