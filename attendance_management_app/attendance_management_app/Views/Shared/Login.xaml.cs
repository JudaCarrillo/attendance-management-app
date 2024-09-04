using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using attendance_management_app.Services;
using attendance_management_app.Views.Employee;
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

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            var users = UserDataStore.Instance.GetUsersDataStore();
            var currentUser = users.Find(user => user.Name == username && user.Password == password);

            if (currentUser == null)
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "Ok");
            }

            if (!currentUser.Enabled)
            {
                await DisplayAlert("Error", "Usuario deshabilitado", "Ok");
            }

            if (currentUser.FirstLogin)
            {
                await Navigation.PushAsync(new Shared.UpdatePassword());
            }

            AuthService.Instance.Login(currentUser);
            await Navigation.PushAsync(new Inicio());

        }
    }
}