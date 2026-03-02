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

namespace WholesaleBase2.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        public CustomersPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgCustomers.ItemsSource = db.Customers.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }
        private void addClick_Click(object sender, RoutedEventArgs e)
        {
            var window = new CustomerWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
            {
                db = new WholesaleBaseEntities();
                LoadData();
            }
        }

        private void editClick_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента");
                return;
            }

            var selected = (Customers)dgCustomers.SelectedItem;
            var customer = db.Customers.Find(selected.CustomerID);

            if (customer != null)
            {
                var window = new CustomerWindow(customer);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true)
                {
                    db = new WholesaleBaseEntities();
                    LoadData();
                }
            }
        }

        private void deleteClick_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента");
                return;
            }

            var result = MessageBox.Show("Удалить клиента?", "Подтверждение",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var selected = (Customers)dgCustomers.SelectedItem;
                    var customer = db.Customers.Find(selected.CustomerID);

                    if (customer != null)
                    {
                        db.Customers.Remove(customer);
                        db.SaveChanges();
                        db = new WholesaleBaseEntities();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }
            }
        }

        private void refreshClick_Click(object sender, RoutedEventArgs e)
        {
            db = new WholesaleBaseEntities();
            LoadData();
        }
    }
}




