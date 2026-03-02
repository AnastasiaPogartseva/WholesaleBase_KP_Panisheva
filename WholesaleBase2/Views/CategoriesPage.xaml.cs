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
    /// Логика взаимодействия для CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        public CategoriesPage()
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
                dgCategories.ItemsSource = db.Categories.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void AddClick_Click(object sender, RoutedEventArgs e)
        {
            var window = new CategoryWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
            {
                db = new WholesaleBaseEntities();
                LoadData();
            }
        }

        private void EditClick_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategories.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            var selected = (Categories)dgCategories.SelectedItem;
            var category = db.Categories.Find(selected.CategoryID);

            if (category != null)
            {
                var window = new CategoryWindow(category);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true)
                {
                    db = new WholesaleBaseEntities();
                    LoadData();
                }
            }
        }

        private void DeleteClick_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategories.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            var result = MessageBox.Show("Удалить категорию?", "Подтверждение",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var selected = (Categories)dgCategories.SelectedItem;
                    var category = db.Categories.Find(selected.CategoryID);

                    if (category != null)
                    {
                        db.Categories.Remove(category);
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

        private void RefreshClick_Click(object sender, RoutedEventArgs e)
        {
            db = new WholesaleBaseEntities();
            LoadData();
        }
    }
}
