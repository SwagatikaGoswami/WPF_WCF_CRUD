using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace DemoWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDemoServiceServer
    {

        [OperationContract]
        Task<List<Customer>>  GetCustomers();

        [OperationContract]
        List<string> GetCustomerIds();

        [OperationContract]
        Customer GetCustomerById(string customerId);

        [OperationContract]
        Task<int> InsertCustomer(Customer customer);

        [OperationContract]
        Task<int> UpdateCustomer(Customer currentCustomer);

        [OperationContract]
        Task<int> DeleteCustomer(String customerId);

        [OperationContract]
        //Task<List<Order>> GetOrders(string custemerId);
        List<Order> GetOrders(string custemerId);

        [OperationContract]
        Task<int> CreateOrder(Order order);

        [OperationContract]
        Task<int> UpdateOrder(Order currentOrder);

        [OperationContract]
        Task<int> DeleteOrder(int orderId);       
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
