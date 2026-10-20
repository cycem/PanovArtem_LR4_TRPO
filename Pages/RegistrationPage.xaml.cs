
using Microsoft.Win32;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MassageSalon.Entities;


namespace MassageSalon.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

     

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text.Trim();
            string surname = tbSurName.Text.Trim();
            string fath = tbFath.Text.Trim();
            string login = tbLogin.Text.Trim();
            string pass = pbPassword.Password;

            if (name == "" || surname == "" || fath == "" || login == "" || pass == "")
            {
                MessageBox.Show("Не все поля заполнены.");
                return;
            }
            
            if (pass.Length < 8)
            {
                MessageBox.Show("Пароль должен быть не короче 8 символов.");
                return;
            }

            if (App.Context.Users.Any(u => u.Login == login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            Users newUser = new Users
            {
                FullName = $"{surname} {name} {fath}",
                Login = login,
                Password = pass,
                RoleID = 1
            };

            App.Context.Users.Add(newUser);
            App.Context.SaveChanges();

            MessageBox.Show("Регистрация прошла успешно.");
            NavigationService.Navigate(new LoginPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        
    }
}
