using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spring.RestBucksClient.Api
{
    [DataContract(Name="order", Namespace="http://restbucks.com")]
    public class Order
    {
        [DataMember(Name = "location", Order = 0)]
	    public string Location { get; set; }

        [DataMember(Name = "cost", EmitDefaultValue = false, Order = 1)]
        public decimal Cost { get; set; }

        [DataMember(Name = "items", Order = 2)]
        public List<OrderItem> Items { get; set; }

        [DataMember(Name = "status", EmitDefaultValue = false, Order = 3)]
        public string Status { get; set; }
	}
}
