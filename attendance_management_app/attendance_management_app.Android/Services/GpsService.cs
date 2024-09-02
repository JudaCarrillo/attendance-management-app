using Android.Content;
using Android.Locations;
using attendance_management_app.Droid.Services;
using attendance_management_app.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(GpsService))]
namespace attendance_management_app.Droid.Services
{
    public class GpsService : IGpsService
    {
        private LocationManager _locationManager;

        public void OpenSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
            intent.SetFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }

        public bool IsGpsEnabled()
        {
            _locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            return _locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        public (double Latitude, double Longitude) GetLocation()
        {
            double latitude = 0;
            double longitude = 0;

            // Obtener la ubicación más reciente
            var criteria = new Criteria { Accuracy = Accuracy.Fine };
            var provider = _locationManager.GetBestProvider(criteria, true);
            if (provider != null)
            {
                var location = _locationManager.GetLastKnownLocation(provider);
                if (location != null)
                {
                    latitude = location.Latitude;
                    longitude = location.Longitude;
                }
            }

            return (latitude, longitude);
        }
    }
}
