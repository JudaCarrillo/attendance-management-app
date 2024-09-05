using attendance_management_app.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using attendance_management_app.Models;
using attendance_management_app.Services;
using Microcharts;
using Microcharts.Forms;
using ChartEntry = Microcharts.ChartEntry;
using SkiaSharp;
using Xamarin.Forms;
using System.Globalization;
using System.Diagnostics;

namespace attendance_management_app.Views.Employee
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : ContentPage
    {
        private Button _selectedButton;
        private bool markAttendanceEnabled;

        public Inicio()
        {
            InitializeComponent();
            LoadData();
            OpenLocationPermissionModal();
        }

        private void LoadData()
        {
            User currentUser = AuthService.Instance.currentUser;
            Turn userTurn = TurnDataStore.Instance.GetTurnsDataStore().Find(turn => turn.TurnId == currentUser.TurnId);
            BindingContext = userTurn;

            DateTime today = DateTime.Today;
            CreatePieChartForDate(new DateTime(2024, 8, 30));
        }

        private void CreatePieChartForDate(DateTime date)
        {
            var chart = Util.CreateEntriesForMonthChart(date);
            chartView.Chart = new DonutChart { Entries = chart };
        }


        private void OnButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            string clickedDay = button.CommandParameter as string;

            var dayOfWeek = Util.GetDayOfWeek(clickedDay);
            DateTime today = DateTime.Today;
            var targetDate = Util.GetClosestDateForDayOfWeek(today, dayOfWeek);
            OnSelectDay(targetDate);

            ButtonSelectedStyle(button);
        }

        private void OnSelectDay(DateTime date)
        {
            string targetMonht = date.ToString("MM/yyyy");
            string targetDate = date.ToString("dd/MM/yyyy");

            var currentUser = AuthService.Instance.currentUser;

            var attendanceData = AttendanceDataStore.Instance.GetAttendancesData();

            if (!attendanceData.ContainsKey(targetMonht) || !attendanceData[targetMonht].ContainsKey(targetDate))
            {
                AttendanceDataStore.Instance.InitializedAttendanceDataStore(targetMonht, targetDate);
            }

            var record = attendanceData[targetMonht][targetDate];
            var attendanceRecord = record.Early.Find(attendance => attendance.UserId == currentUser.UserId) ??
                                   record.Late.Find(attendance => attendance.UserId == currentUser.UserId) ??
                                   record.Absent.Find(attendance => attendance.UserId == currentUser.UserId);

            if (attendanceRecord == null)
            {
                AttendanceFrame.BackgroundColor = Color.White;
                AttendanceHistoryFrame.IsVisible = false;
                return;
            }

            AttendanceHistoryFrame.IsVisible = true;
            UpdateLabelText(attendanceRecord.Type, true);

            switch (attendanceRecord.Type)
            {
                case Attendance.AttendanceType.Early:
                    AttendanceFrame.BackgroundColor = Color.FromHex("#801CDE3B");
                    AttendanceHistoryImage.Source = "check.png";
                    break;
                case Attendance.AttendanceType.Late:
                    AttendanceFrame.BackgroundColor = Color.FromHex("#80FFAE00");
                    AttendanceHistoryImage.Source = "warning.png";
                    break;
                case Attendance.AttendanceType.Absent:
                    AttendanceFrame.BackgroundColor = Color.FromHex("#80FF0000");
                    AttendanceHistoryImage.Source = "absent.png";
                    break;
            }
        }

        private void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            AuthService.Instance.Logout();
            Navigation.PopToRootAsync();
        }

        private void ButtonSelectedStyle(Button clickedButton)
        {
            // Restaurar el estado del botón previamente seleccionado
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Color.Black;
                _selectedButton.TextColor = Color.White;
            }

            // Establecer el estado del botón actualmente seleccionado
            clickedButton.BackgroundColor = Color.Gray;
            clickedButton.TextColor = Color.Black;

            // Guardar la referencia del botón seleccionado
            _selectedButton = clickedButton;
        }

        private void UpdateLabelText(Attendance.AttendanceType attendanceType, bool hasAttendance)
        {
            AttendanceFrame.IsVisible = hasAttendance;
            if (!hasAttendance) return;

            switch (attendanceType)
            {
                case Attendance.AttendanceType.Early:
                    AttendanceHistoryLabel.Text = "Asistencia Registrada";
                    break;
                case Attendance.AttendanceType.Late:
                    AttendanceHistoryLabel.Text = "Tardanza Registrada";
                    break;
                case Attendance.AttendanceType.Absent:
                    AttendanceHistoryLabel.Text = "Ausencia Registrada";
                    break;
            }
        }

        private async void OnMarkAttendance(object sender, EventArgs e)
        {
            if (!markAttendanceEnabled) return;
            if (AttendanceHistoryFrame.IsVisible) return;

            DateTime now = DateTime.Now;
            string date = now.ToString("dd/MM/yyyy");
            string monthYear = now.ToString("MM/yyyy");

            User currentUser = AuthService.Instance.currentUser;
            Turn currentTurn = TurnDataStore.Instance.GetTurnsDataStore().Find(turn => turn.TurnId == currentUser.TurnId);

            if (currentTurn == null)
            {
                await DisplayAlert("Error", "No hay turnos definidos.", "OK");
            }

            TimeSpan startTime = TimeSpan.Parse(currentTurn.StartTime);
            TimeSpan endTime = TimeSpan.Parse(currentTurn.EndTime);
            TimeSpan nowTime = now.TimeOfDay;
            TimeSpan allowedStartTime = startTime - TimeSpan.FromMinutes(60);
            TimeSpan allowedMarkAttendanceTime = startTime + TimeSpan.FromMinutes(60);

            if (nowTime >= allowedStartTime && nowTime <= startTime)
            {
                MarkAttendance(now, monthYear, date, Attendance.AttendanceType.Early, currentUser);
                await DisplayAlert("Asistencia", "Has marcado asistencia a tiempo.", "OK");
            }
            else if (nowTime > startTime && nowTime <= allowedMarkAttendanceTime)
            {
                MarkAttendance(now, monthYear, date, Attendance.AttendanceType.Late, currentUser);
                await DisplayAlert("Asistencia", "Has marcado asistencia tarde.", "OK");
            }
            else
            {
                await DisplayAlert("Asistencia", "No es posible marcar la asistencia.", "OK");
            }
        }

        private void MarkAttendance(DateTime now, string monthYear, string date, Attendance.AttendanceType attendanceType, User user)
        {
            var attendance = new Attendance
            {
                UserId = user.UserId,
                DateTime = now,
                AttendanceId = Util.GenerateUniqueId(),
                Type = attendanceType
            };

            AttendanceDataStore.Instance.AddAttendanceDataStore(monthYear, date, attendance);
            var today = DateTime.Today;
            OnSelectDay(today);
        }

        private async void OpenLocationPermissionModal()
        {
            var modal = new LocationPermissionModal(OnLocationReceived);
            await Navigation.PushModalAsync(modal);
        }

        private void OnLocationReceived(double latitude, double longitude)
        {
            markAttendanceEnabled = GelocationService.OnLocationReceived(latitude, longitude);

            if (!markAttendanceEnabled)
            {
                DisplayAlert("Fuera de rango", "No estás en la ubicación permitida para marcar asistencia.", "OK");
                return;
            }
        }

    }
}