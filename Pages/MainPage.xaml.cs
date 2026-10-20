using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MassageSalon.Entities;

namespace MassageSalon.Pages
{
    public partial class MainPage : Page
    {
        private MassageSalonEntities1 db = new MassageSalonEntities1();

        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.CurrentUser == null)
                {
                    NavigationService.Navigate(new LoginPage());
                    return;
                }

                TbUser.Text = "Вы вошли как: " + App.CurrentUser.FullName;
                DgServices.ItemsSource = db.Services.ToList();
                CbMasseur.ItemsSource = db.Masseurs.ToList();
                CbRoom.ItemsSource = db.Rooms.ToList();
                DpDate.SelectedDate = DateTime.Today;
                LoadMyList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMyList()
        {
            int userId = App.CurrentUser.UserID;
            DgMyAppointments.ItemsSource = db.Appointments
                .Include(a => a.Services)
                .Include(a => a.Masseurs)
                .Include(a => a.Rooms)
                .Where(a => a.UserID == userId)
                .OrderBy(a => a.StartTime)
                .ToList();
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = DgServices.SelectedItem as Services;
                if (service == null || CbMasseur.SelectedValue == null ||
                    CbRoom.SelectedValue == null || DpDate.SelectedDate == null)
                {
                    MessageBox.Show("Выберите массаж и заполните все поля.", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime time;
                if (!DateTime.TryParseExact(TbTime.Text.Trim(), "H:mm", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out time))
                {
                    MessageBox.Show("Введите время в формате ЧЧ:мм, например 14:30.", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime start = DpDate.SelectedDate.Value.Date + time.TimeOfDay;
                if (start <= DateTime.Now)
                {
                    MessageBox.Show("Нельзя записаться на прошедшее время.", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int masseurId = (int)CbMasseur.SelectedValue;
                DateTime end = start.AddMinutes(service.DurationMin);
                DateTime dayStart = start.Date;
                DateTime dayEnd = dayStart.AddDays(1);

                var masseurAppointments = db.Appointments
                    .Include(a => a.Services)
                    .Where(a => a.MasseurID == masseurId && a.Status != "Отменена"
                                && a.StartTime >= dayStart && a.StartTime < dayEnd)
                    .ToList();

                bool busy = masseurAppointments.Any(a => a.StartTime < end &&
                    a.StartTime.AddMinutes(a.Services.DurationMin) > start);

                if (busy)
                {
                    MessageBox.Show("У массажиста это время занято. Выберите другое время.", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Appointments appointment = new Appointments
                {
                    UserID = App.CurrentUser.UserID,
                    ServiceID = service.ServiceID,
                    MasseurID = masseurId,
                    RoomID = (int)CbRoom.SelectedValue,
                    StartTime = start,
                    Status = "Запланирована"
                };

                db.Appointments.Add(appointment);
                db.SaveChanges();
                LoadMyList();

                MessageBox.Show("Вы успешно записаны!", "Готово",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось выполнить запись: " + ex.Message, "Ошибка",
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