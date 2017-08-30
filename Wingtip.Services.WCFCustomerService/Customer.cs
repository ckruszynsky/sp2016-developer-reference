using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Wingtip.Services.WCFCustomerService
{
    //[DataContract(Name = "Customer",
    //   Namespace = "http://schemas.wingtip.com/Customers")]
    public class Customer
    {
        [DataMember(Name = "CustomerID", Order = 1)]
        public String CustomerID { get; set; }

        [DataMember(Name = "ContactName", Order = 1)]
        public String ContactName { get; set; }

        [DataMember(Name = "CompanyName", Order = 1)]
        public String CompanyName { get; set; }

        [DataMember(Name = "Country", Order = 1)]
        public String Country { get; set; }
    }

    //[CollectionDataContract(ItemName = "Customer", Name = "Customers",
    //    Namespace = "http://schemas.wingtip.com/Customers")]
    public class Customers : List<Customer>
    {
        public Customers() : base() { }
        public Customers(IEnumerable<Customer> collection) : base(collection) { }
    }
}