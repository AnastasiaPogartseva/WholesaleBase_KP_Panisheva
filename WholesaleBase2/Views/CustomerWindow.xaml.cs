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
using System.Windows.Shapes;

namespace WholesaleBase2.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        private Customers _customer = null;
        public CustomerWindow()
        {
            InitializeComponent();
            txtTitle.Text = "Добавление клиента";
        }
        public CustomerWindow(Customers customer)
        {
            InitializeComponent();
            txtTitle.Text = "Редактирование клиента";
            _customer = customer;
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            if (_customer != null)
            {
                txtFirstName.Text = _customer.FirstName;
                txtLastName.Text = _customer.LastName;
                txtPhone.Text = _customer.Phone;
                txtEmail.Text = _customer.Email;
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    MessageBox.Show("Введите имя клиента");
                    txtFirstName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    MessageBox.Show("Введите фамилию клиента");
                    txtLastName.Focus();
                    return;
                }

                if (_customer == null)
                {
                    var customer = new Customers
                    {
                        FirstName = txtFirstName.Text.Trim(),
                        LastName = txtLastName.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Email = txtEmail.Text.Trim()
                    };
                    db.Customers.Add(customer);
                }
                else
                {
                    var customer = db.Customers.Find(_customer.CustomerID);
                    if (customer != null)
                    {
                        customer.FirstName = txtFirstName.Text.Trim();
                        customer.LastName = txtLastName.Text.Trim();
                        customer.Phone = txtPhone.Text.Trim();
                        customer.Email = txtEmail.Text.Trim();
                    }
                }

                db.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
