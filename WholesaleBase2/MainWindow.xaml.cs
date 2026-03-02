using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WholesaleBase2.Views;

namespace WholesaleBase2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ProductsPage());
        }



        private void Products_Click_1(object sender, RoutedEventArgs e) => MainFrame.Navigate(new ProductsPage());
        private void Categories_Click_1(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CategoriesPage());
        private void Warehouses_Click_1(object sender, RoutedEventArgs e) => MainFrame.Navigate(new WarehousesPage());
        private void Stock_Click_1(object sender, RoutedEventArgs e) => MainFrame.Navigate(new StockPage());
        private void Suppliers_Click_1(object sender, RoutedEventArgs e) => MainFrame.Navigate(new SuppliersPage());
        private void Customers_Click_1(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CustomersPage());
        private void Invoices_Click_1(object sender, RoutedEventArgs e) => MainFrame.Navigate(new InvoicesPage());

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Clear();
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
