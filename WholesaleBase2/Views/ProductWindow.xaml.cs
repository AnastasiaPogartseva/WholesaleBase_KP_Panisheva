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
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        private Products _product = null; // null значит добавляем новый
        public ProductWindow()
        {
            InitializeComponent();
            txtTitle.Text = "Добавление товара";
            LoadCategories();
        }

        // Конструктор для редактирования
        public ProductWindow(Products product)
        {
            InitializeComponent();
            txtTitle.Text = "Редактирование товара";
            _product = product;
            LoadCategories();
            LoadProductData();
        }

        // Загрузка списка категорий
        // Загрузка списка категорий
        private void LoadCategories()
        {
            try
            {
                // Принудительно создаем новый контекст
                using (var context = new WholesaleBaseEntities())
                {
                    var categories = context.Categories.ToList();
                    cmbCategory.ItemsSource = categories;
                    cmbCategory.DisplayMemberPath = "CategoryName";
                    cmbCategory.SelectedValuePath = "CategoryID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        // Загрузка данных товара для редактирования
        private void LoadProductData()
        {
            if (_product != null)
            {
                txtProductName.Text = _product.ProductName;
                txtPrice.Text = _product.Price.ToString();
                txtQuantity.Text = _product.Quantity.ToString();
                cmbCategory.SelectedValue = _product.CategoryID;
            }
        }

        // Сохранение
        private void Saveclick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка заполнения
                if (string.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    MessageBox.Show("Введите наименование товара");
                    txtProductName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Введите цену");
                    txtPrice.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtQuantity.Text))
                {
                    MessageBox.Show("Введите количество");
                    txtQuantity.Focus();
                    return;
                }

                if (cmbCategory.SelectedItem == null)
                {
                    MessageBox.Show("Выберите категорию");
                    return;
                }

                // Преобразование цены
                if (!decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Цена должна быть числом");
                    txtPrice.Focus();
                    return;
                }

                // Преобразование количества
                if (!int.TryParse(txtQuantity.Text, out int quantity))
                {
                    MessageBox.Show("Количество должно быть целым числом");
                    txtQuantity.Focus();
                    return;
                }

                int categoryId = (int)cmbCategory.SelectedValue;

                if (_product == null) // Добавление нового
                {
                    var product = new Products
                    {
                        ProductName = txtProductName.Text.Trim(),
                        Price = price,
                        Quantity = quantity,
                        CategoryID = categoryId
                    };
                    db.Products.Add(product);
                }
                else // Редактирование существующего
                {
                    // Находим товар в контексте базы данных
                    var productToUpdate = db.Products.Find(_product.ProductID);
                    if (productToUpdate != null)
                    {
                        productToUpdate.ProductName = txtProductName.Text.Trim();
                        productToUpdate.Price = price;
                        productToUpdate.Quantity = quantity;
                        productToUpdate.CategoryID = categoryId;
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

        // Отмена
        private void Cancelclick_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}