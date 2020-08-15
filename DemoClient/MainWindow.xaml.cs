using DemoClient.DemoWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IDemoServiceServer = DemoClient.DemoWCFService.IDemoServiceServer;

namespace DemoClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CustomerDetails customerWindow;
        private OrderDetails orderWindow;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Close the child window only when this window closes
            if (customerWindow != null)
                customerWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (customerWindow == null)
                customerWindow = new CustomerDetails();

            // Show the window
            customerWindow.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (orderWindow == null)
                orderWindow = new OrderDetails();

            // Show the window
            orderWindow.Show();
        }
    }
}
