using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Wingtip.Services.WCFCustomerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICustomerService" in both code and config file together.
    [ServiceContract(Namespace = "http://schemas.wingtip.com/CustomerService")]
    public interface ICustomerService
    {
        [OperationContract]
        Customer GetCustomerById(String customerID);

        [OperationContract]
        Customers ListAllCustomers();

        [OperationContract]
        Customer AddCustomer(Customer item);

        [OperationContract]
        Customer UpdateCustomer(Customer item);

        [OperationContract]
        Boolean DeleteCustomer(Customer item);
    }
}
