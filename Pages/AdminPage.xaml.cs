using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MassageSalon.Entities;

namespace MassageSalon.Pages
{
    public partial class AdminPage : Page
    {
        private MassageSalonEntities1 db = new MassageSalonEntities1();

        public AdminPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
              
                TbUser.Text = "Администратор: " + App.CurrentUser.FullName;

                CbUser.ItemsSource = db.Users.ToList();
                CbService.ItemsSource = db.Services.ToList();
                CbMasseur.ItemsSource = db.Masseurs.ToList();
                CbRoom.ItemsSource = db.Rooms.ToList();
                CbStatus.ItemsSource = new List<string> { "Запланирована", "Завершена", "Отменена" };

                LoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadList()
        {
            db = new MassageSalonEntities1();
            DgAppointments.ItemsSource = db.Appointments
                .Include(a => a.Users)
                .Include(a => a.Services)
                .Include(a => a.Masseurs)
                .Include(a => a.Rooms)
                .OrderBy(a => a.StartTime)
                .ToList();
        }

        private void DgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = DgAppointments.SelectedItem as Appointments;
            if (a == null)
                return;

            CbUser.SelectedValue = a.UserID;
            CbService.SelectedValue = a.ServiceID;
            CbMasseur.SelectedValue = a.MasseurID;
            CbRoom.SelectedValue = a.RoomID;
            CbStatus.SelectedItem = a.Status;
        }

        private bool FieldsFilled()
        {
            if (CbUser.SelectedValue == null || CbService.SelectedValue == null ||
                CbMasseur.SelectedValue == null || CbRoom.SelectedValue == null ||
                CbStatus.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!FieldsFilled())
                    return;

                Appointments a = new Appointments
                {
                    UserID = (int)CbUser.SelectedValue,
                    ServiceID = (int)CbService.SelectedValue,
                    MasseurID = (int)CbMasseur.SelectedValue,
                    RoomID = (int)CbRoom.SelectedValue,
                    Status = (string)CbStatus.SelectedItem,
                    StartTime = DateTime.Now
                };

                db.Appointments.Add(a);
                db.SaveChanges();
                LoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить запись: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = DgAppointments.SelectedItem as Appointments;
                if (a == null)
                {
                    MessageBox.Show("Выберите запись в таблице.", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!FieldsFilled())
                    return;

                a.UserID = (int)CbUser.SelectedValue;
                a.ServiceID = (int)CbService.SelectedValue;
                a.MasseurID = (int)CbMasseur.SelectedValue;
                a.RoomID = (int)CbRoom.SelectedValue;
                a.Status = (string)CbStatus.SelectedItem;

                db.SaveChanges();
                LoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось изменить запись: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = DgAppointments.SelectedItem as Appointments;
                if (a == null)
                {
                    MessageBox.Show("Выберите запись в таблице.", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                int id = a.AppointmentID;
                db.Payments.RemoveRange(db.Payments.Where(p => p.AppointmentID == id).ToList());
                db.Appointments.Remove(a);
                db.SaveChanges();
                LoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось удалить запись: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
    }
}