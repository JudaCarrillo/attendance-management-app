using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using attendance_management_app.Models;
using attendance_management_app.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace attendance_management_app.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatePassword : ContentPage
    {
        public UpdatePassword()
        {
            InitializeComponent();
        }

        public void ClearEntries()
        {
            currentPassword.Text = string.Empty;
            newPassword.Text = string.Empty;
            repeatNewPassword.Text = string.Empty;
        }

        private async void btnUpdatePassword_Clicked(object sender, EventArgs e)
        {
            string password = currentPassword.Text;
            string newUserPassword = newPassword.Text;
            string confirmNewUserPassword = repeatNewPassword.Text;

            User currentUser = AuthService.Instance.currentUser;

            if (currentUser.Password != password)
            {
                await DisplayAlert("Error", "Contraseña actual incorrecta", "Ok");
                return;
            }

            if (newUserPassword != confirmNewUserPassword)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden", "Ok");
                return;
            }

            ClearEntries();
            AuthService.Instance.UpdatePassword(newUserPassword);
            await DisplayAlert("Éxito", "Contraseña actualizada", "Ok");
            await Navigation.PopAsync();
        }
    }
}