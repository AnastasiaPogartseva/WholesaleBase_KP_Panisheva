using System.Windows;
using System;
using System.Windows.Controls;
using System.Linq;
using WholesaleBase2.Views;

namespace WholesaleBase2
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private WholesaleBaseEntities db = new WholesaleBaseEntities();
        public ProductsPage()
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
                var products = from p in db.Products
                               join c in db.Categories on p.CategoryID equals c.CategoryID into cat
                               from c in cat.DefaultIfEmpty()
                               select new
                               {
                                   p.ProductID,
                                   p.ProductName,
                                   CategoryName = c != null ? c.CategoryName : "Без категории",
                                   p.Price,
                                   p.Quantity,
                                   p.ImagePath
                               };

                // Принудительно выполняем запрос и сохраняем в список
                var productList = products.ToList();
                dgProducts.ItemsSource = productList;

                // Для отладки - показываем количество записей
                System.Diagnostics.Debug.WriteLine($"Загружено товаров: {productList.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new ProductWindow();
                window.Owner = Window.GetWindow(this);

                if (window.ShowDialog() == true)
                {
                    // Обновляем контекст базы данных
                    db = new WholesaleBaseEntities();
                    LoadData();
                    MessageBox.Show("Товар успешно добавлен", "Успех",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message, "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProducts.SelectedItem == null)
                {
                    MessageBox.Show("Выберите товар для редактирования", "Информация",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Получаем ID выбранного товара
                dynamic selected = dgProducts.SelectedItem;
                int productId = selected.ProductID;

                // Находим товар в базе
                var product = db.Products.Find(productId);
                if (product != null)
                {
                    var window = new ProductWindow(product);
                    window.Owner = Window.GetWindow(this);

                    if (window.ShowDialog() == true)
                    {
                        // Обновляем контекст базы данных
                        db = new WholesaleBaseEntities();
                        LoadData();
                        MessageBox.Show("Товар успешно обновлен", "Успех",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании: " + ex.Message, "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProducts.SelectedItem == null)
                {
                    MessageBox.Show("Выберите товар для удаления", "Информация",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                dynamic selected = dgProducts.SelectedItem;
                int productId = selected.ProductID;

                var result = MessageBox.Show("Вы уверены, что хотите удалить этот товар?",
                                            "Подтверждение удаления",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var product = db.Products.Find(productId);
                    if (product != null)
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();

                        // Обновляем контекст базы данных
                        db = new WholesaleBaseEntities();
                        LoadData();
                        MessageBox.Show("Товар успешно удален", "Успех",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // Обновляем контекст базы данных
                db = new WholesaleBaseEntities();
                LoadData();
                MessageBox.Show("Данные обновлены", "Информация",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message, "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}