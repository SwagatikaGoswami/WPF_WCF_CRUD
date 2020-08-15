using DemoClient.DemoWCFService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DemoClient
{
    /// <summary>
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : Window
    {
        private bool _close;
        private IDemoServiceServer serv;        
        public List<string> customers = new List<string>();

        public OrderDetails()
        {
            InitializeComponent();
            serv = new DemoWCFService.DemoServiceServerClient();
            customers = serv.GetCustomerIds().ToList();       
            this.CustomerIds.ItemsSource = customers;           
        }

        public new void Close()
        {
            _close = true;
            base.Close();
        }

         private void Window_Closing(object sender, CancelEventArgs e)
        {
            // If Close() was called, close the window (instead of hiding it)
            if (_close) return;

            // Hide the window (instead of closing it)
            e.Cancel = true;
            Hide();
        }

        //private void LastCommandHandler(object sender, ExecutedRoutedEventArgs e)
        //{
        //    custViewSource.View.MoveCurrentToLast();
        //}

        //private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        //{
        //    custViewSource.View.MoveCurrentToPrevious();
        //}

        //private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        //{
        //    custViewSource.View.MoveCurrentToNext();
        //}

        //private void FirstCommandHandler(object sender, ExecutedRoutedEventArgs e)
        //{
        //    custViewSource.View.MoveCurrentToFirst();
        //}

         // Commit changes from the new customer form, the new order form,  
        // or edits made to the existing customer form.  
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.CustomerIds.SelectedIndex < 0)
            {
                MessageBox.Show("No customer selected.");
                return;
            }
           
            Order obj = e.Parameter as Order;
            serv.UpdateOrder(obj);            
        }
   
        // Sets up the form so that user can enter data. Data is later  
        // saved when user clicks Commit.  
        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newOrderGrid.IsVisible)
            {
                // Order ID is auto-generated so we don't set it here.  
                // For CustomerID, address, etc we use the values from current customer.  
                // User can modify these in the datagrid after the order is entered.  

                var currentCustomerId = CustomerIds.SelectedItem.ToString();
                var currentCustomer = serv.GetCustomerById(currentCustomerId);

                Order newOrder = new Order()
                {
                    OrderDate = add_orderDatePicker.SelectedDate,
                    RequiredDate = add_requiredDatePicker.SelectedDate,
                    ShippedDate = add_shippedDatePicker.SelectedDate,
                    CustomerID = currentCustomerId,
                    ShipAddress = currentCustomer.Address,
                    ShipCity = currentCustomer.City,
                    ShipCountry = currentCustomer.Country,
                    ShipName = currentCustomer.CompanyName,
                    ShipPostalCode = currentCustomer.PostalCode,
                    ShipRegion = currentCustomer.Region
                };

                try
                {
                    newOrder.EmployeeID = Int32.Parse(add_employeeIDTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("EmployeeID must be a valid integer value.");
                    return;
                }

                //Add the order by calling service 
                serv.CreateOrder(newOrder);
                if (CustomerIds.SelectedIndex >= 0)
                {
                    string selection = CustomerIds.SelectedItem.ToString();
                    var custOrders = serv.GetOrders(selection).ToList();
                    this.ordersDataGrid.ItemsSource = custOrders;
                }
            }
            //existingCustomerGrid.Visibility = Visibility.Collapsed;
            newOrderGrid.Visibility = Visibility.Collapsed;
            // newCustomerGrid.Visibility = Visibility.Visible;         
        }

        private void NewOrder_click(object sender, RoutedEventArgs e)
        {
            //var cust = ;
            if (this.CustomerIds.SelectedIndex < 0)
            {
                MessageBox.Show("No customer selected.");
                return;
            }

            newOrderGrid.UpdateLayout();
            newOrderGrid.Visibility = Visibility.Visible;
        }

        // Cancels any input into the new customer form  
        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            //add_addressTextBox.Text = "";
            //add_cityTextBox.Text = "";
            //add_companyNameTextBox.Text = "";
            //add_contactNameTextBox.Text = "";
            //add_contactTitleTextBox.Text = "";
            //add_countryTextBox.Text = "";
            //add_customerIDTextBox.Text = "";
            //add_faxTextBox.Text = "";
            //add_phoneTextBox.Text = "";
            //add_postalCodeTextBox.Text = "";
            //add_regionTextBox.Text = "";

            //existingCustomerGrid.Visibility = Visibility.Visible;
            //newCustomerGrid.Visibility = Visibility.Collapsed;
            //newOrderGrid.Visibility = Visibility.Collapsed;
        }

        private void Delete_Order(Order order)
        {
            if (MessageBox.Show("Do you want to delete the order?",
          "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                serv.DeleteOrder(order.OrderID);
            }
        }

        private void DeleteOrderCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

          //  Get the Order in the row in which the Delete button was clicked.  
            Order obj = e.Parameter as Order;
            Delete_Order(obj);
        }

        private void CustomerIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CustomerIds.SelectedIndex >=0)
            {
                string selection = CustomerIds.SelectedItem.ToString();
                var custOrders = serv.GetOrders(selection).ToList();
                this.ordersDataGrid.ItemsSource = custOrders;                
            }
            
        }
    }
}
