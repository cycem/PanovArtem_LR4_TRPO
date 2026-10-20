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

namespace MassageSalon.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
       
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
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

            if (user.roleID == 2)
                NavigationService.Navigate(new AdminPage());
            else
                NavigationService.Navigate(new MainPage());
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}

