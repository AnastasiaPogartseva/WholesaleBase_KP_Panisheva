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
    /// Логика взаимодействия для InvoicesPage.xaml
    /// </summary>
    public partial class InvoicesPage : Page
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();

        public InvoicesPage()
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
                var invoices = from i in db.Invoices
                               join c in db.Customers on i.CustomerID equals c.CustomerID
                               orderby i.InvoiceDate descending
                               select new
                               {
                                   i.InvoiceID,
                                   i.InvoiceNumber,
                                   i.InvoiceDate,
                                   CustomerName = c.LastName + " " + c.FirstName,
                                   i.TotalAmount,
                                   i.Status
                               };
                dgInvoices.ItemsSource = invoices.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var window = new InvoiceEditWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
            {
                db = new WholesaleBaseEntities();
                LoadData();
            }
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoices.SelectedItem == null)
            {
                MessageBox.Show("Выберите накладную");
                return;
            }

            dynamic selected = dgInvoices.SelectedItem;
            int invoiceId = selected.InvoiceID; // получаем ID здесь

            var window = new InvoiceViewWindow(invoiceId); // передаем ID в конструктор
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void dgInvoices_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            View_Click(sender, null);
        }
        

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoices.SelectedItem == null)
            {
                MessageBox.Show("Выберите накладную");
                return;
            }

            dynamic selected = dgInvoices.SelectedItem;
            int invoiceId = selected.InvoiceID;

            var invoice = db.Invoices.Find(invoiceId);
            if (invoice != null)
            {
                var window = new InvoiceEditWindow(invoice);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true)
                {
                    db = new WholesaleBaseEntities();
                    LoadData();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoices.SelectedItem == null)
            {
                MessageBox.Show("Выберите накладную");
                return;
            }

            var result = MessageBox.Show("Удалить накладную?", "Подтверждение",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    dynamic selected = dgInvoices.SelectedItem;
                    int invoiceId = selected.InvoiceID;

                    var invoice = db.Invoices.Find(invoiceId);
                    if (invoice != null)
                    {
                        // Сначала удаляем все позиции
                        var items = db.InvoiceItems.Where(i => i.InvoiceID == invoiceId);
                        db.InvoiceItems.RemoveRange(items);

                        // Потом удаляем накладную
                        db.Invoices.Remove(invoice);
                        db.SaveChanges();

                        db = new WholesaleBaseEntities();
                        LoadData();
                        MessageBox.Show("Накладная удалена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            db = new WholesaleBaseEntities();
            LoadData();
        }
    }
}