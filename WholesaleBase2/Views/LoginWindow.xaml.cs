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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public string Role { get; set; }
        }

        private User[] users = new User[]
{
    new User { Username = "admin", Password = "123", FullName = "Администратор", Role = "Администратор" },
    new User { Username = "director", Password = "123", FullName = "Директор", Role = "Директор" },
    new User { Username = "manager", Password = "123", FullName = "Менеджер", Role = "Менеджер" },
    new User { Username = "warehouse", Password = "123", FullName = "Кладовщик", Role = "Кладовщик" },
    new User { Username = "accountant", Password = "123", FullName = "Бухгалтер", Role = "Бухгалтер" }
};
        public LoginWindow()
        {
            InitializeComponent();
            txtUsername.Focus();
        }

       
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void Login()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Введите логин и пароль";
                return;
            }

            // Ищем пользователя в списке
            User foundUser = null;
            foreach (var user in users)
            {
                if (user.Username == username && user.Password == password)
                {
                    foundUser = user;
                    break;
                }
            }

            if (foundUser != null)
            {
                // Сохраняем данные пользователя
                CurrentUser.UserID = 1; // Просто для идентификации
                CurrentUser.Username = foundUser.Username;
                CurrentUser.FullName = foundUser.FullName;
                CurrentUser.Role = foundUser.Role;

                // Открываем главное окно
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                txtError.Text = "Неверный логин или пароль";
                txtPassword.Password = "";
                txtPassword.Focus();
            }
        }

        private void BtnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}