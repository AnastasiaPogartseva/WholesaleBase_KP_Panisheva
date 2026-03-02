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
    /// Логика взаимодействия для WarehousesPage.xaml
    /// </summary>
    public partial class WarehousesPage : Page
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        public WarehousesPage()
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
                dgWarehouses.ItemsSource = db.Warehouses.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void Addclick_Click(object sender, RoutedEventArgs e)
        {
            var window = new WarehouseWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
            {
                db = new WholesaleBaseEntities();
                LoadData();
            }
        }

        private void Editclick_Click(object sender, RoutedEventArgs e)
        {
            if (dgWarehouses.SelectedItem == null)
            {
                MessageBox.Show("Выберите склад");
                return;
            }

            var selected = (Warehouses)dgWarehouses.SelectedItem;
            var warehouse = db.Warehouses.Find(selected.WarehouseID);

            if (warehouse != null)
            {
                var window = new WarehouseWindow(warehouse);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true)
                {
                    db = new WholesaleBaseEntities();
                    LoadData();
                }
            }
        }

        private void Deleteclick_Click(object sender, RoutedEventArgs e)
        {
            if (dgWarehouses.SelectedItem == null)
            {
                MessageBox.Show("Выберите склад");
                return;
            }

            var result = MessageBox.Show("Удалить склад?", "Подтверждение",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var selected = (Warehouses)dgWarehouses.SelectedItem;
                    var warehouse = db.Warehouses.Find(selected.WarehouseID);

                    if (warehouse != null)
                    {
                        db.Warehouses.Remove(warehouse);
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

        private void Refreshclick_Click(object sender, RoutedEventArgs e)
        {
            db = new WholesaleBaseEntities();
            LoadData();
        }
    }
}

