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
        // Coordenadas de la empresa
        private readonly double _empresaLatitud = -12.0003777;
        private readonly double _empresaLongitud = -76.8611125;
        private readonly double _rangoPermitido = 0.01;

        public MainPage()
        {
            InitializeComponent();
            OpenLocationPermissionModal();
            ShowCurrentTime();
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

            var distancia = CalculateDistance(latitude, longitude, _empresaLatitud, _empresaLongitud);

            if (distancia <= _rangoPermitido)
            {
                MarkAttendanceButton.IsEnabled = true;
            }
            else
            {
                MarkAttendanceButton.IsEnabled = false;
                DisplayAlert("Fuera de rango", "No estás en la ubicación permitida para marcar asistencia.", "OK");
            }
        }

        // Método para mostrar la hora actual
        private void ShowCurrentTime()
        {
            // Obtener la hora actual
            string currentTime = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            CurrentTimeLabel.Text = $"Hora actual: {currentTime}"; 
        }

        // Método para calcular la distancia en kilómetros
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;

            // Convertir grados a radianes
            lat1 = lat1 * Math.PI / 180.0;
            lat2 = lat2 * Math.PI / 180.0;

            // Aplicar fórmula del haversine
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Radio de la Tierra en km
            double r = 6371;

            // Retornar distancia en kilómetros
            return r * c;
        }

        private async void MarkAttendanceButton_Clicked(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string date = now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string monthYear = now.ToString("yyyy-MM", CultureInfo.InvariantCulture);

            var currentTurn = TurnDataStore.Instance.GetTurnsDataStore().FirstOrDefault(); 

            if (currentTurn != null)
            {
                TimeSpan startTime = TimeSpan.Parse(currentTurn.StartTime);
                TimeSpan endTime = TimeSpan.Parse(currentTurn.EndTime);
                TimeSpan nowTime = now.TimeOfDay;
                TimeSpan allowedStartTime = startTime - TimeSpan.FromMinutes(60);

                if (nowTime < allowedStartTime )
                {
                    await MarkAttendance(now, monthYear, date, Attendance.AttendanceType.Early);
                    await DisplayAlert("Asistencia", "Has marcado asistencia a tiempo.", "OK");
                    
                }
                else
                {
                    await MarkAttendance(now, monthYear, date, Attendance.AttendanceType.Late);
                    await DisplayAlert("Asistencia", "Has marcado asistencia tarde.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No hay turnos definidos.", "OK");
            }
        }

        private async Task MarkAttendance(DateTime now, string monthYear, string date, Attendance.AttendanceType attendanceType)
        {
            var attendance = new Attendance
            {
                UserId = "Usuario123", 
                DateTime = now,
                AttendanceId = Util.GenerateUniqueId(),
                Type = attendanceType
            };

            AttendanceDataStore.Instance.AddAttendanceDataStore(monthYear, date, attendance);
        }


        private async void ShowAttendanceButton_Clicked(object sender, EventArgs e)
        {
            string attendanceSummary = AttendanceDataStore.Instance.GetAttendanceSummary();
            await DisplayAlert("Resumen de Asistencia", attendanceSummary, "OK");
        }

    }
}
