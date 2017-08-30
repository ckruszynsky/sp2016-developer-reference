using Wingtip.BCS.CRM;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Wingtip.BCS.CRM.SampleCRMModel
{
    public partial class CustomerService
    {
        public static IEnumerable<Customer> ReadList()
        {
            SampleCRMEntities ctx = SampleCRMEntities.CreateContext();
            return (ctx.Customers);
        }

        public static Customer ReadItem(string customerID)
        {
            SampleCRMEntities ctx = SampleCRMEntities.CreateContext();
            return (ctx.Customers.FirstOrDefault(c => c.CustomerID == customerID));
        }

        public static Customer Create(Customer newCustomer)
        {
            SampleCRMEntities ctx = SampleCRMEntities.CreateContext();
            ctx.Customers.Add(newCustomer);
            ctx.SaveChanges();

            return (newCustomer);
        }

        public static void Update(Customer customer)
        {
            SampleCRMEntities ctx = SampleCRMEntities.CreateContext();
            ctx.Customers.Attach(customer);
            ctx.SaveChanges();

        }

        public static void Delete(string customerID)
        {
            SampleCRMEntities ctx = SampleCRMEntities.CreateContext();
            ctx.Customers.Remove(ctx.Customers.FirstOrDefault
                     (c => c.CustomerID == customerID));
            ctx.SaveChanges();

        }
      
    }
}