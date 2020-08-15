using DemoClient.DemoWCFService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    /// Interaction logic for CustomerDetails.xaml
    /// </summary>
    public partial class CustomerDetails : Window
    {
        public List<Customer> customerViewSource = new List<Customer>();
        int currentIndex = 0;
        private bool _close;
        private IDemoServiceServer serv;
        private List<string> customerIds = new List<string>();
        public CustomerDetails()
        {
            InitializeComponent();
            serv = new DemoWCFService.DemoServiceServerClient();
            customerViewSource = serv.GetCustomers().ToList();
            if (customerViewSource.Count > 0)
            {
                this.existingCustomerGrid.DataContext = customerViewSource.FirstOrDefault();
                this.currentIndex = 0;
            }

            // Populate customer id combo box
            customerIds = customerViewSource.Select(c => c.CustomerID).ToList();
            CustomerIds.ItemsSource = customerIds;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        // Shadow Window.Close to make sure we bypass the Hide call in 
        // the Closing event handler
        public new void Close()
        {
            _close = true;
            base.Close();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // If Close() was called, close the window (instead of hiding it)
            if (_close) return;

            // Hide the window (instead of closing it)
            e.Cancel = true;
            Hide();
        }

        private void LastCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            currentIndex = customerViewSource.Count - 1;
            if (currentIndex >= 0)
            {
                this.existingCustomerGrid.DataContext  = customerViewSource[currentIndex];
            }
            
        }

        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
           if(currentIndex > 1)
            {
                currentIndex--;
                this.existingCustomerGrid.DataContext = customerViewSource[currentIndex];
            }
        }

        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            
            if (currentIndex < customerViewSource.Count -1)
            {
                currentIndex++;
                this.existingCustomerGrid.DataContext = customerViewSource[currentIndex];
            }           
        }

        private void FirstCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if(customerViewSource.Count >0)
            {
                this.existingCustomerGrid.DataContext = customerViewSource.FirstOrDefault();
                currentIndex = 0;
            }
        }

        private void DeleteCustomerCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if(MessageBox.Show("Do you want to delete customer?",
            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var curcustomer = customerViewSource[currentIndex];
                serv.DeleteCustomer(curcustomer.CustomerID);
                customerViewSource = serv.GetCustomers().ToList();
                if (customerViewSource.Count > 0)
                {
                    this.existingCustomerGrid.DataContext = customerViewSource.FirstOrDefault();
                    this.currentIndex = 0;
                }
                // Populate customer id combo box
                customerIds = customerViewSource.Select(c => c.CustomerID).ToList();
                CustomerIds.ItemsSource = customerIds;
            }            
        }

        // Commit changes from the new customer form, the new order form,  
        // or edits made to the existing customer form.  
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newCustomerGrid.IsVisible)
            {
                // Create a new object because the old one  
                // is being tracked by EF now.  
                Customer newCustomer = new Customer
                {
                    Address = add_addressTextBox.Text,
                    City = add_cityTextBox.Text,
                    CompanyName = add_companyNameTextBox.Text,
                    ContactName = add_contactNameTextBox.Text,
                    ContactTitle = add_contactTitleTextBox.Text,
                    Country = add_countryTextBox.Text,
                    CustomerID = add_customerIDTextBox.Text,
                    Fax = add_faxTextBox.Text,
                    Phone = add_phoneTextBox.Text,
                    PostalCode = add_postalCodeTextBox.Text,
                    Region = add_regionTextBox.Text
                };


                // Perform very basic validation  
                if (newCustomer.CustomerID.Length == 5)
                {
                    // Insert the new customer at correct position:  
                    int len = customerViewSource.Count();
                    int pos = len;
                    for (int i = 0; i < len; ++i)
                    {
                        if (String.CompareOrdinal(newCustomer.CustomerID, customerViewSource[i].CustomerID) < 0)
                        {
                            pos = i;
                            break;
                        }
                    }
                    // Add the new  customer to db
                    serv.InsertCustomer(newCustomer);
                    customerViewSource = serv.GetCustomers().ToList();
                    if (customerViewSource.Count > 0)
                    {
                        this.existingCustomerGrid.DataContext = customerViewSource.FirstOrDefault();
                        this.currentIndex = 0;
                        // Populate customer id combo box
                        customerIds = customerViewSource.Select(c => c.CustomerID).ToList();
                        CustomerIds.ItemsSource = customerIds;
                    }
                }
                else
                {
                    MessageBox.Show("CustomerID must have 5 characters.");
                }

                newCustomerGrid.Visibility = Visibility.Collapsed;
                existingCustomerGrid.Visibility = Visibility.Visible;
            }
            else if (!newCustomerGrid.IsVisible)
            {
                var currentcutomer = this.existingCustomerGrid.DataContext as Customer;
                serv.UpdateCustomer(currentcutomer);
            }
                // Save the changes, either for a new customer, a new order  
                // or an edit to an existing customer or order.                
        }

        // Sets up the form so that user can enter data. Data is later  
        // saved when user clicks Commit.  
        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            existingCustomerGrid.Visibility = Visibility.Collapsed;
            //newOrderGrid.Visibility = Visibility.Collapsed;
            newCustomerGrid.Visibility = Visibility.Visible;

            // Clear all the text boxes before adding a new customer.  
            foreach (var child in newCustomerGrid.Children)
            {
                var tb = child as TextBox;
                if (tb != null)
                {
                    tb.Text = "";
                }
            }
        }

        private void CustomerIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerIds.SelectedIndex >= 0)
            {
                if (customerViewSource.Count > 0)
                {
                    this.existingCustomerGrid.DataContext = customerViewSource[CustomerIds.SelectedIndex];
                    currentIndex = 0;
                }
            }

        }

        // Cancels any input into the new customer form  
        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            add_addressTextBox.Text = "";
            add_cityTextBox.Text = "";
            add_companyNameTextBox.Text = "";
            add_contactNameTextBox.Text = "";
            add_contactTitleTextBox.Text = "";
            add_countryTextBox.Text = "";
            add_customerIDTextBox.Text = "";
            add_faxTextBox.Text = "";
            add_phoneTextBox.Text = "";
            add_postalCodeTextBox.Text = "";
            add_regionTextBox.Text = "";

            existingCustomerGrid.Visibility = Visibility.Visible;
            newCustomerGrid.Visibility = Visibility.Collapsed;           
        }      
    }
}
