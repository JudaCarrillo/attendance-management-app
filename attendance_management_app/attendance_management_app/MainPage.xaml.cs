using System; // Asegúrate de incluir esto
using Xamarin.Forms;
using attendance_management_app.Services;  // Asegúrate de que el espacio de nombres sea correcto

namespace attendance_management_app
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Verificar si el GPS está habilitado
            var gpsService = DependencyService.Get<IGpsService>();

            if (!gpsService.IsGpsEnabled())
            {
                // Mostrar alerta y ofrecer abrir la configuración
                DisplayAlert("GPS deshabilitado", "El GPS está deshabilitado, ¿desea habilitarlo?", "Sí", "No").ContinueWith(async (task) =>
                {
                    if (task.Result)
                    {
                        // Si el usuario acepta, abrir la configuración
                        gpsService.OpenSettings();
                    }
                });
            }
            else
            {
                // GPS está habilitado, continuar con la lógica de la aplicación
            }
        }

        private void OnCheckGpsClicked(object sender, EventArgs e)
        {
            var gpsService = DependencyService.Get<IGpsService>();

            if (!gpsService.IsGpsEnabled())
            {
                gpsService.OpenSettings();
            }
            else
            {
                var (latitude, longitude) = gpsService.GetLocation();
                LatitudeLabel.Text = $"Latitud: {latitude}";
                LongitudeLabel.Text = $"Longitud: {longitude}";
            }
        }
    }
}
