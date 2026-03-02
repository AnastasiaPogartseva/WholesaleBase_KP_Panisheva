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
    /// Логика взаимодействия для InvoiceEditWindow.xaml
    /// </summary>
    public partial class InvoiceEditWindow : Window
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        private Invoices _invoice = null;
        private List<InvoiceItemModel> items = new List<InvoiceItemModel>();

        public class InvoiceItemModel
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total => Quantity * Price;
        }

        public InvoiceEditWindow()
        {
            InitializeComponent();
            txtTitle.Text = "Создание накладной";
            dpInvoiceDate.SelectedDate = DateTime.Now;
            txtInvoiceNumber.Text = "INV-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            LoadCustomers();
            LoadProducts();
            dgItems.ItemsSource = items;
        }

        public InvoiceEditWindow(Invoices invoice)
        {
            InitializeComponent();
            txtTitle.Text = "Редактирование накладной";
            _invoice = invoice;
            LoadCustomers();
            LoadProducts();
            LoadInvoiceData();
            dgItems.ItemsSource = items;
        }

        private void LoadCustomers()
        {
            var customers = db.Customers.ToList();
            var customerList = new List<dynamic>();

            foreach (var c in customers)
            {
                customerList.Add(new
                {
                    c.CustomerID,
                    FullName = c.LastName + " " + c.FirstName
                });
            }

            cmbCustomer.ItemsSource = customerList;
        }

        private void LoadProducts()
        {
            cmbProduct.ItemsSource = db.Products.ToList();
        }

        private void LoadInvoiceData()
        {
            if (_invoice == null) return;

            txtInvoiceNumber.Text = _invoice.InvoiceNumber;
            dpInvoiceDate.SelectedDate = _invoice.InvoiceDate;
            cmbCustomer.SelectedValue = _invoice.CustomerID;

            var invoiceItems = db.InvoiceItems.Where(i => i.InvoiceID == _invoice.InvoiceID).ToList();

            foreach (var item in invoiceItems)
            {
                var product = db.Products.Find(item.ProductID);

                int pid = 0;
                if (item.ProductID != null) pid = (int)item.ProductID;

                int qty = 0;
                if (item.Quantity != null) qty = (int)item.Quantity;

                decimal prc = 0;
                if (item.Price != null) prc = (decimal)item.Price;

                string pname = "Неизвестный товар";
                if (product != null) pname = product.ProductName;

                items.Add(new InvoiceItemModel
                {
                    ProductID = pid,
                    ProductName = pname,
                    Quantity = qty,
                    Price = prc
                });
            }

            UpdateTotal();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProduct.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }

            int quantity = 0;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите количество (целое число больше 0)");
                return;
            }

            var product = (Products)cmbProduct.SelectedItem;

            var existing = null as InvoiceItemModel;
            foreach (var i in items)
            {
                if (i.ProductID == product.ProductID)
                {
                    existing = i;
                    break;
                }
            }

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                decimal price = 0;
                if (product.Price != null) price = (decimal)product.Price;

                items.Add(new InvoiceItemModel
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Quantity = quantity,
                    Price = price
                });
            }

            dgItems.Items.Refresh();
            UpdateTotal();
            txtQuantity.Text = "";
            cmbProduct.SelectedItem = null;
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (InvoiceItemModel)button.Tag;

            var itemToRemove = null as InvoiceItemModel;
            foreach (var i in items)
            {
                if (i.ProductID == item.ProductID)
                {
                    itemToRemove = i;
                    break;
                }
            }

            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
            }

            dgItems.Items.Refresh();
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (var item in items)
            {
                total = total + item.Total;
            }
            txtTotalAmount.Text = total.ToString("N2") + " ₽";
        }

      
       

        private void CANCEL_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SAVE_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtInvoiceNumber.Text))
                {
                    MessageBox.Show("Введите номер накладной");
                    return;
                }

                if (cmbCustomer.SelectedItem == null)
                {
                    MessageBox.Show("Выберите клиента");
                    return;
                }

                if (items.Count == 0)
                {
                    MessageBox.Show("Добавьте хотя бы один товар");
                    return;
                }

                decimal total = 0;
                foreach (var item in items)
                {
                    total = total + item.Total;
                }

                if (_invoice == null)
                {
                    _invoice = new Invoices();
                    _invoice.InvoiceNumber = txtInvoiceNumber.Text.Trim();
                    _invoice.InvoiceDate = dpInvoiceDate.SelectedDate;
                    _invoice.CustomerID = (int)cmbCustomer.SelectedValue;
                    _invoice.TotalAmount = total;
                    _invoice.Status = "Новый";

                    db.Invoices.Add(_invoice);
                    db.SaveChanges();

                    foreach (var item in items)
                    {
                        var invoiceItem = new InvoiceItems();
                        invoiceItem.InvoiceID = _invoice.InvoiceID;
                        invoiceItem.ProductID = item.ProductID;
                        invoiceItem.Quantity = item.Quantity;
                        invoiceItem.Price = item.Price;

                        db.InvoiceItems.Add(invoiceItem);
                    }
                }
                else
                {
                    _invoice.InvoiceNumber = txtInvoiceNumber.Text.Trim();
                    _invoice.InvoiceDate = dpInvoiceDate.SelectedDate;
                    _invoice.CustomerID = (int)cmbCustomer.SelectedValue;
                    _invoice.TotalAmount = total;

                    var oldItems = db.InvoiceItems.Where(i => i.InvoiceID == _invoice.InvoiceID).ToList();
                    foreach (var oldItem in oldItems)
                    {
                        db.InvoiceItems.Remove(oldItem);
                    }

                    foreach (var item in items)
                    {
                        var invoiceItem = new InvoiceItems();
                        invoiceItem.InvoiceID = _invoice.InvoiceID;
                        invoiceItem.ProductID = item.ProductID;
                        invoiceItem.Quantity = item.Quantity;
                        invoiceItem.Price = item.Price;

                        db.InvoiceItems.Add(invoiceItem);
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
    }
}