using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MassageSalon.Entities;

namespace MassageSalon.Pages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = tbName.Text.Trim();
                string surname = tbSurName.Text.Trim();
                string fath = tbFath.Text.Trim();
                string login = tbLogin.Text.Trim();
                string pass = pbPassword.Password;
                string email = tbEmail.Text.Trim();
                string phone = tbPhone.Text.Trim();

                if (name == "" || surname == "" || fath == "" || login == "" || pass == "" || phone == "")
                {
                    MessageBox.Show("Не все поля заполнены.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (pass.Length < 8)
                {
                    MessageBox.Show("Пароль должен быть не короче 8 символов.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (email != "" && !email.Contains("@"))
                {
                    MessageBox.Show("Введите корректную почту.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (App.Context.Users.Any(u => u.Login == login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Users newUser = new Users
                {
                    FullName = surname + " " + name + " " + fath,
                    Login = login,
                    Password = pass,
                    Email = email == "" ? null : email,
                    Phone = phone,
                    RoleID = 1
                };

                App.Context.Users.Add(newUser);
                App.Context.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно.");
                NavigationService.Navigate(new LoginPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}