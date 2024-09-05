using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using attendance_management_app.Services;
using attendance_management_app.Models;
using System.Globalization;
using attendance_management_app.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace attendance_management_app
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            OpenLocationPermissionModal();
        }

        private async void OpenLocationPermissionModal()
        {
            var modal = new LocationPermissionModal(OnLocationReceived);
            await Navigation.PushModalAsync(modal);
        }

        private void OnLocationReceived(double latitude, double longitude)
        {
            DisplayAlert("Ubicación Obtenida", $"Latitud: {latitude}, Longitud: {longitude}", "OK");

            LatitudeLabel.Text = $"Latitud: {latitude}";
            LongitudeLabel.Text = $"Longitud: {longitude}";

            bool isButtonEnabled = GelocationService.OnLocationReceived(latitude, longitude);
            MarkAttendanceButton.IsEnabled = isButtonEnabled;

            if (!isButtonEnabled)
            {
                DisplayAlert("Fuera de rango", "No estás en la ubicación permitida para marcar asistencia.", "OK");
                return;
            }
        }

        private async void MarkAttendanceButton_Clicked(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string date = now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            string monthYear = now.ToString("MM-yyyy", CultureInfo.InvariantCulture);

            // Usar AuthServices - currentTurn
            // var currentTurn = TurnDataStore.Instance.GetTurnsDataStore().FirstOrDefault(); 

            if (currentTurn != null)
            {
                TimeSpan startTime = TimeSpan.Parse(currentTurn.StartTime);
                TimeSpan endTime = TimeSpan.Parse(currentTurn.EndTime);
                TimeSpan nowTime = now.TimeOfDay;
                TimeSpan allowedStartTime = startTime - TimeSpan.FromMinutes(60);

                if (nowTime < allowedStartTime)
                {
                    MarkAttendance(now, monthYear, date, Attendance.AttendanceType.Early);
                    await DisplayAlert("Asistencia", "Has marcado asistencia a tiempo.", "OK");

                }
                else
                {
                    MarkAttendance(now, monthYear, date, Attendance.AttendanceType.Late);
                    await DisplayAlert("Asistencia", "Has marcado asistencia tarde.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No hay turnos definidos.", "OK");
            }
        }

        private void MarkAttendance(DateTime now, string monthYear, string date, Attendance.AttendanceType attendanceType)
        {
            // Usar AuthServices - UserId
            var attendance = new Attendance
            {
                UserId = "Usuario123",
                DateTime = now,
                AttendanceId = Util.GenerateUniqueId(),
                Type = attendanceType
            };

            AttendanceDataStore.Instance.AddAttendanceDataStore(monthYear, date, attendance);
        }

    }
}
