using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Wingtip.Services.WCFCustomerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CustomerService.svc or CustomerService.svc.cs at the Solution Explorer and start debugging.
    public class CustomerService : ICustomerService
    {
        private static Customers customers;

        static CustomerService()
        {
            customers = new Customers(
                from i in Enumerable.Range(1, 40)
                select new Customer
                {
                    CustomerID = String.Format("ID{0:000}", i),
                    ContactName = String.Format("Customer {0}", i),
                    CompanyName = "Wingtip",
                    Country = i % 2 == 0 ? "Italy" : "USA",
                }
                );
        }

        public Customer GetCustomerById(string customerID)
        {
            Customer result =
                (from c in customers
                 where c.CustomerID == customerID
                 select c).FirstOrDefault();

            return (result);
        }

        public Customers ListAllCustomers()
        {
            return (customers);
        }

        public Customer AddCustomer(Customer item)
        {
            customers.Add(item);
            return (item);
        }

        public Customer UpdateCustomer(Customer item)
        {
            Customer customer = customers.Where(c => c.CustomerID == item.CustomerID).FirstOrDefault();
            if (customer != null)
            {
                // customer.CustomerID = item.CustomerID;
                customer.ContactName = item.ContactName;
                customer.CompanyName = item.CompanyName;
                customer.Country = item.Country;
            }

            return (customer);
        }

        public bool DeleteCustomer(Customer item)
        {
            Customer customer = customers.Where(c => c.CustomerID == item.CustomerID).FirstOrDefault();
            if (customer != null)
            {
                customers.Remove(customer);
                return (true);
            }
            return (false);
        }
    }
}
