using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MassageSalon.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = TBoxLogin.Text.Trim();
                string pass = PBoxPassword.Password;

                if (login == "" || pass == "")
                {
                    MessageBox.Show("Введите логин и пароль.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var user = App.Context.Users.FirstOrDefault(u => u.Login == login && u.Password == pass);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                App.CurrentUser = user;

                if (user.RoleID != 3)
                    NavigationService.Navigate(new AdminPage());
                else
                    NavigationService.Navigate(new MainPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при входе: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}