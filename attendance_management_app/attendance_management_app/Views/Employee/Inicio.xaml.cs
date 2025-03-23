using attendance_management_app.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using attendance_management_app.Models;
using attendance_management_app.Services;
using Xamarin.Forms;
using System.Globalization;
using System.Diagnostics;

namespace attendance_management_app.Views.Employee
{
    public partial class Inicio : ContentPage
    {
        public Inicio()
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
            bool markAttendanceEnabled = GelocationService.OnLocationReceived(latitude, longitude);

            if (!markAttendanceEnabled)
            {
                DisplayAlert("Fuera de rango", "No estás en la ubicación permitida para marcar asistencia.", "OK");
            }
        }

        private void OnDashboardView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Dashboard());
        }

        private void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            AuthService.Instance.Logout();
            Navigation.PopToRootAsync();
        }
    }
}
