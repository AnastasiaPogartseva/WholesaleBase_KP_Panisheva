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
    /// Логика взаимодействия для InvoiceViewWindow.xaml
    /// </summary>
    public partial class InvoiceViewWindow : Window
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        public InvoiceViewWindow(int invoiceId)
        {
            InitializeComponent();
            var invoice = db.Invoices.Find(invoiceId); // используем параметр invoiceId
            if (invoice == null) return;

            var customer = db.Customers.Find(invoice.CustomerID);

            txtNumber.Text = invoice.InvoiceNumber;

            if (invoice.InvoiceDate != null)
                txtDate.Text = invoice.InvoiceDate.Value.ToString("dd.MM.yyyy");
            else
                txtDate.Text = "Нет даты";

            if (customer != null)
                txtCustomer.Text = customer.LastName + " " + customer.FirstName;
            else
                txtCustomer.Text = "Неизвестный клиент";

            var invoiceItems = db.InvoiceItems.Where(i => i.InvoiceID == invoiceId).ToList(); // используем параметр invoiceId
            var itemsList = new System.Collections.Generic.List<dynamic>();

            decimal total = 0;

            foreach (var item in invoiceItems)
            {
                var product = db.Products.Find(item.ProductID);

                string productName = "Неизвестный товар";
                if (product != null) productName = product.ProductName;

                int qty = 0;
                if (item.Quantity != null) qty = (int)item.Quantity;

                decimal price = 0;
                if (item.Price != null) price = (decimal)item.Price;

                decimal itemTotal = qty * price;
                total = total + itemTotal;

                itemsList.Add(new
                {
                    ProductName = productName,
                    Quantity = qty,
                    Price = price,
                    Total = itemTotal
                });
            }

            dgItems.ItemsSource = itemsList;
            txtTotal.Text = total.ToString("N2") + " ₽";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}