using Xamarin.Forms;

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
        }

    }
}
