using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace attendance_management_app
{
    public partial class MainPage : ContentPage
    {
        // Coordenadas de la empresa
        private readonly double _empresaLatitud = -11.8934203;
        private readonly double _empresaLongitud = -77.0664731;
        private readonly double _rangoPermitido = 0.01;

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
            await DisplayAlert("Asistencia", "Asistencia marcada exitosamente.", "OK");
        }
    }
}
