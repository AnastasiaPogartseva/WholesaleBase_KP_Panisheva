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
    /// Логика взаимодействия для SuppliersPage.xaml
    /// </summary>
    public partial class SuppliersPage : Page
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();

        public SuppliersPage()
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
                dgSuppliers.ItemsSource = db.Suppliers.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }
        private void ADDclick_Click(object sender, RoutedEventArgs e)
        {
            var window = new SupplierWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
            {
                db = new WholesaleBaseEntities();
                LoadData();
            }
        }

        private void EDITclick_Click(object sender, RoutedEventArgs e)
        {
            if (dgSuppliers.SelectedItem == null)
            {
                MessageBox.Show("Выберите поставщика");
                return;
            }

            var selected = (Suppliers)dgSuppliers.SelectedItem;
            var supplier = db.Suppliers.Find(selected.SupplierID);

            if (supplier != null)
            {
                var window = new SupplierWindow(supplier);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true)
                {
                    db = new WholesaleBaseEntities();
                    LoadData();
                }
            }
        }

        private void DELETEclick_Click(object sender, RoutedEventArgs e)
        {
            if (dgSuppliers.SelectedItem == null)
            {
                MessageBox.Show("Выберите поставщика");
                return;
            }

            var result = MessageBox.Show("Удалить поставщика?", "Подтверждение",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var selected = (Suppliers)dgSuppliers.SelectedItem;
                    var supplier = db.Suppliers.Find(selected.SupplierID);

                    if (supplier != null)
                    {
                        db.Suppliers.Remove(supplier);
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

     
        private void REFRESHclick_Click(object sender, RoutedEventArgs e)
        {
            db = new WholesaleBaseEntities();
            LoadData();
        }
    }
}


