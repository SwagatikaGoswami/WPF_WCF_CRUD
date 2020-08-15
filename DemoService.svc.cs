using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace DemoWCFService
{
 
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class DemoService : IDemoServiceServer
    {
        
        public void Commit()
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateOrder(Order order)
        {
            return Task<int>.Factory.StartNew(() =>
            {
                using (var ctx = new NorthwindEntities())
                {

                    ctx.Orders.Add(order);
                    return ctx.SaveChanges();
                }
            });
        }

        public Task<int> DeleteCustomer(string customerId)
        {
            return Task<int>.Factory.StartNew(() =>
            {
                int Retval = -1;
                using (var ctx = new NorthwindEntities())
                {
                   var cust = (from c in ctx.Customers
                               where c.CustomerID == customerId
                                select c).FirstOrDefault();

                    Customer usrdtl = new Customer();
                    usrdtl.CustomerID = customerId;                   
                    if (cust != null)
                    {
                        foreach (var ord in cust.Orders.ToList())
                        {
                           DeleteOrder(ord.OrderID);
                        }
                        ctx.Customers.Remove(cust);
                        ctx.SaveChanges();
                        Retval = 0;
                    }
                    
                    return Retval;
                }
            });
        }

        public Task<int> DeleteOrder(int orderId)
        {
            return Task<int>.Factory.StartNew(() =>
            {
                using (var ctx = new NorthwindEntities())
                {

                    Order orddtl = new Order();
                    orddtl.OrderID = orderId;
                    ctx.Entry(orddtl).State = EntityState.Deleted;
                    int Retval = ctx.SaveChanges();
                    return Retval;
                }
            });
        }

        public Customer GetCustomerById(string customerId)
        {
            using (var ctx = new NorthwindEntities())
            {
                var item = (from c in ctx.Customers
                            where c.CustomerID == customerId
                            select c).FirstOrDefault();
                Customer usr = new Customer();
                usr.CustomerID = item.CustomerID;
                usr.Address = item.Address;
                usr.City = item.City;
                usr.CompanyName = item.ContactName;
                usr.ContactTitle = item.ContactTitle;
                usr.Country = item.Country;
                usr.Fax = item.Fax;
                // usr.Orders = item.Orders;
                usr.Phone = item.Phone;
                usr.PostalCode = item.PostalCode;
                usr.Region = item.Region;

                return usr;
            }
        }

        public List<string> GetCustomerIds()
        {
            List<string> customerlists = new List<string>();
            using (var ctx = new NorthwindEntities())
            {

                var lstOrder = from k in ctx.Customers select k.CustomerID;
                foreach (var item in lstOrder)
                {
                    customerlists.Add(item);
                }

                return customerlists;
            }
        }

        public List<Order> GetOrders(string custemerId)
        {
            List<Order> orderlist = new List<Order>();
            using (var ctx = new NorthwindEntities())
            {
                var cust = ctx.Orders.SqlQuery("Select * from Orders where CustomerID = '" +  custemerId + "'" );
                // return cust.ToList();
               // var lstOrder = from k in ctx.Orders select k;
                foreach (var item in cust)
                {
                    Order usr = new Order();
                    usr.CustomerID = item.CustomerID;
                    //usr.Customer = item.Customer;
                    usr.EmployeeID = item.EmployeeID;
                    usr.Freight = item.Freight;
                    usr.OrderDate = item.OrderDate;
                    usr.OrderID = item.OrderID;
                    usr.RequiredDate = item.RequiredDate;
                    usr.ShipAddress = item.ShipAddress;
                    usr.ShipCity = item.ShipCity;
                    usr.ShipCountry = item.ShipCountry;
                    usr.ShipName = item.ShipName;
                    usr.ShippedDate = item.ShippedDate;
                    usr.ShipPostalCode = item.ShipPostalCode;
                    usr.ShipRegion = item.ShipRegion;
                    usr.ShipVia = item.ShipVia;
                    orderlist.Add(usr);
                }

                }
                return orderlist;            
        }

        //public Task<List<Order>> GetOrders(string custemerId)
        //{
        //    return Task<List<Order>>.Factory.StartNew(() =>
        //    {
        //        List<Order> orderlist = new List<Order>();
        //        using (var ctx = new NorthwindEntities())
        //        {
        //            var cust = ctx.Orders.Where(s => s.CustomerID == custemerId);
        //            // return cust.ToList();
        //            var lstOrder = from k in ctx.Orders select k;
        //            foreach (var item in cust)
        //            {
        //                Order usr = new Order();
        //                usr.CustomerID = item.CustomerID;
        //                usr.Customer = item.Customer;
        //                usr.EmployeeID = item.EmployeeID;
        //                usr.Freight = item.Freight;
        //                usr.OrderDate = item.OrderDate;
        //                usr.OrderID = item.OrderID;
        //                usr.RequiredDate = item.RequiredDate;
        //                usr.ShipAddress = item.ShipAddress;
        //                usr.ShipCity = item.ShipCity;
        //                usr.ShipCountry = item.ShipCountry;
        //                usr.ShipName = item.ShipName;
        //                usr.ShippedDate = item.ShippedDate;
        //                usr.ShipPostalCode = item.ShipPostalCode;
        //                usr.ShipRegion = item.ShipRegion;
        //                usr.ShipVia = item.ShipVia;
        //                orderlist.Add(usr);

        //            }
        //            return orderlist;
        //        }
        //    });
        //}

        public Task<int> InsertCustomer(Customer customer)
        {
            return Task<int>.Factory.StartNew(() =>
            {
                using (var ctx = new NorthwindEntities())
                {

                    ctx.Customers.Add(customer);
                    return ctx.SaveChanges();
                }
            });
        }

        public Task<int> UpdateCustomer(Customer currentCustomer)
        {     
            return Task<int>.Factory.StartNew(() =>
            {
                using (var ctx = new NorthwindEntities())
                {
                    ctx.Entry(currentCustomer).State = EntityState.Modified;

                    int Retval = ctx.SaveChanges();
                    return Retval;
                }
            });
        }

        public Task<int> UpdateOrder(Order currentOrder)
        {
            return Task<int>.Factory.StartNew(() =>
            {
                using (var ctx = new NorthwindEntities())
                {
                    ctx.Entry(currentOrder).State = EntityState.Modified;

                    int Retval = ctx.SaveChanges();
                    return Retval;
                }
            });
        }

        Task<List<Customer>> IDemoServiceServer.GetCustomers()
        {
            return Task<List<Customer>>.Factory.StartNew(() =>
            {
                List<Customer> userlst = new List<Customer>();
                using (var ctx = new NorthwindEntities())
                {

                    var lstUsr = from k in ctx.Customers select k;
                    foreach (var item in lstUsr)
                    {
                        
                        try
                        {
                            Customer usr = new Customer();
                            usr.CustomerID = item.CustomerID;
                            usr.Address = item.Address;
                            usr.City = item.City;
                            usr.CompanyName = item.ContactName;
                            usr.ContactTitle = item.ContactTitle;
                            usr.Country = item.Country;
                            usr.Fax = item.Fax;                           
                           // usr.Orders = item.Orders;
                            usr.Phone = item.Phone;
                            usr.PostalCode = item.PostalCode;
                            usr.Region = item.Region;
                            userlst.Add(usr);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    return userlst;
                }
            });
        }
    }
}
