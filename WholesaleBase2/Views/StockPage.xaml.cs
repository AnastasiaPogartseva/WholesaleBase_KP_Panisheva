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
    /// Логика взаимодействия для StockPage.xaml
    /// </summary>
    public partial class StockPage : Page
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        public StockPage()
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
                var stock = from s in db.Stock
                            join p in db.Products on s.ProductID equals p.ProductID
                            join w in db.Warehouses on s.WarehouseID equals w.WarehouseID
                            select new
                            {
                                ProductName = p.ProductName,
                                WarehouseName = w.WarehouseName,
                                s.Quantity
                            };
                dgStock.ItemsSource = stock.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки остатков: " + ex.Message);
            }
        }

        private void refreshclick_Click(object sender, RoutedEventArgs e) => LoadData();
    }
}