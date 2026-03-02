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
    /// Логика взаимодействия для CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        WholesaleBaseEntities db = new WholesaleBaseEntities();
        private Categories _category = null;
        public CategoryWindow()
        {
            InitializeComponent();
            txtTitle.Text = "Добавление категории";
        }
        public CategoryWindow(Categories category)
        {
            InitializeComponent();
            txtTitle.Text = "Редактирование категории";
            _category = category;
            LoadCategoryData();
        }

        private void LoadCategoryData()
        {
            if (_category != null)
            {
                txtCategoryName.Text = _category.CategoryName;
                txtDescription.Text = _category.Description;
            }
        }

        private void SAVECLICK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    MessageBox.Show("Введите название категории");
                    txtCategoryName.Focus();
                    return;
                }

                if (_category == null)
                {
                    var category = new Categories
                    {
                        CategoryName = txtCategoryName.Text.Trim(),
                        Description = txtDescription.Text.Trim()
                    };
                    db.Categories.Add(category);
                }
                else
                {
                    var category = db.Categories.Find(_category.CategoryID);
                    if (category != null)
                    {
                        category.CategoryName = txtCategoryName.Text.Trim();
                        category.Description = txtDescription.Text.Trim();
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

        private void CANCELCLICK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
