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
    /// Логика взаимодействия для SupplierWindow.xaml
    /// </summary>
    public partial class SupplierWindow : Window
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        private Suppliers _supplier = null;
        public SupplierWindow()
        {
            InitializeComponent();
            txtTitle.Text = "Добавление поставщика";
        }
        public SupplierWindow(Suppliers supplier)
        {
            InitializeComponent();
            txtTitle.Text = "Редактирование поставщика";
            _supplier = supplier;
            LoadSupplierData();
        }

        private void LoadSupplierData()
        {
            if (_supplier != null)
            {
                txtCompanyName.Text = _supplier.CompanyName;
                txtContactPerson.Text = _supplier.ContactPerson;
                txtPhone.Text = _supplier.Phone;
                txtEmail.Text = _supplier.Email;
            }
        }

        private void SaveClick2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
                {
                    MessageBox.Show("Введите название компании");
                    txtCompanyName.Focus();
                    return;
                }

                if (_supplier == null)
                {
                    var supplier = new Suppliers
                    {
                        CompanyName = txtCompanyName.Text.Trim(),
                        ContactPerson = txtContactPerson.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Email = txtEmail.Text.Trim()
                    };
                    db.Suppliers.Add(supplier);
                }
                else
                {
                    var supplier = db.Suppliers.Find(_supplier.SupplierID);
                    if (supplier != null)
                    {
                        supplier.CompanyName = txtCompanyName.Text.Trim();
                        supplier.ContactPerson = txtContactPerson.Text.Trim();
                        supplier.Phone = txtPhone.Text.Trim();
                        supplier.Email = txtEmail.Text.Trim();
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

        private void Cancelclick2_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
