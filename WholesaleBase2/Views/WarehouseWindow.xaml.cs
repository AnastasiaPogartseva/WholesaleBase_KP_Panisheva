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
    /// Логика взаимодействия для WarehouseWindow.xaml
    /// </summary>
    public partial class WarehouseWindow : Window
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        private Warehouses _warehouse = null;

        public WarehouseWindow()
        {
            InitializeComponent();
            txtTitle.Text = "Добавление склада";
        }
        public WarehouseWindow(Warehouses warehouse)
        {
            InitializeComponent();
            txtTitle.Text = "Редактирование склада";
            _warehouse = warehouse;
            LoadWarehouseData();
        }

        private void LoadWarehouseData()
        {
            if (_warehouse != null)
            {
                txtWarehouseName.Text = _warehouse.WarehouseName;
                txtAddress.Text = _warehouse.Address;
                txtPhone.Text = _warehouse.Phone;
            }
        }
        private void saveclick1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtWarehouseName.Text))
                {
                    MessageBox.Show("Введите название склада");
                    txtWarehouseName.Focus();
                    return;
                }

                if (_warehouse == null)
                {
                    var warehouse = new Warehouses
                    {
                        WarehouseName = txtWarehouseName.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        Phone = txtPhone.Text.Trim()
                    };
                    db.Warehouses.Add(warehouse);
                }
                else
                {
                    var warehouse = db.Warehouses.Find(_warehouse.WarehouseID);
                    if (warehouse != null)
                    {
                        warehouse.WarehouseName = txtWarehouseName.Text.Trim();
                        warehouse.Address = txtAddress.Text.Trim();
                        warehouse.Phone = txtPhone.Text.Trim();
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

        private void cancelclick1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
