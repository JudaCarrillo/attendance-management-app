using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using attendance_management_app.Services;
using attendance_management_app.Views.Employee;
using attendance_management_app.Views.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace attendance_management_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ClearEntries()
        {
            usernameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
        }
        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            var users = UserDataStore.Instance.GetUsersDataStore();
            var currentUser = users.Find(user => user.Name == username && user.Password == password);

            if (currentUser == null)
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "Ok");
                return;
            }

            if (!currentUser.Enabled)
            {
                await DisplayAlert("Error", "Usuario deshabilitado", "Ok");
                return;
            }

            AuthService.Instance.Login(currentUser);

            if (currentUser.FirstLogin)
            {
                await Navigation.PushAsync(new UpdatePassword());
                return;
            }

            ClearEntries();
            await Navigation.PushAsync(new Inicio());

        }
    }
}