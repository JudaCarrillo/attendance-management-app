using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace attendance_management_app
{
    public partial class LocationPermissionModal : ContentPage
    {
        private readonly Action<double, double> _onLocationReceived;

        public LocationPermissionModal(Action<double, double> onLocationReceived)
        {
            InitializeComponent();
            _onLocationReceived = onLocationReceived;
        }

        private async void AllowButton_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    _onLocationReceived(location.Latitude, location.Longitude);
                }
                else
                {
                    await DisplayAlert("Ubicación", "No se pudo obtener la ubicación actual.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Ubicación", "Ubicación no permitida", "OK");
            }

            await Navigation.PopModalAsync();
        }

        private async void DenyButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Ubicación", "Ubicación no permitida", "OK");
            await Navigation.PopModalAsync();
        }
    }
}
