using attendance_management_app.Models;
using attendance_management_app.Services;
using attendance_management_app.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace attendance_management_app.Views.Administrador
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrarEmpleado : ContentPage
    {
        private Button _selectedButton;
        private List<Turn> TurnosList;
        private List<User> UserList;
        public RegistrarEmpleado()
        {
            InitializeComponent();
            LoadData();
            OnButtonSelected(GestionButton);
            registerFrame.IsVisible = false;

        }
        private void OnButtonSelected(Button button)
        {
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Color.Black;

            }

            // Aplicamos el estilo de "seleccionado" al botón que fue presionado
            button.BackgroundColor = Color.Gray;


            // Actualizamos el botón seleccionado
            _selectedButton = button;

        }
        private void OnButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            OnButtonSelected(button);

            employeeTableFrame.IsVisible = false;
            registerFrame.IsVisible = true;
            title.Text = "Registrar Empleado";


        }
        private void LoadData()
        {
            TurnosList = TurnDataStore.Instance.GetTurnsDataStore();
            pickerTurn.ItemsSource = TurnosList;
            pickerTurn.ItemDisplayBinding = new Binding("Name");

            UserList = UserDataStore.Instance.GetUsersDataStore();
            listEmployees.ItemsSource = UserList;
        }

        private void GestionButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            OnButtonSelected(button);

            registerFrame.IsVisible = false;
            employeeTableFrame.IsVisible = true;
            title.Text = "Empleados";
        }

        private void BtnLogout_Clicked(object sender, EventArgs e)
        {
            AuthService.Instance.Logout();
            Navigation.PopToRootAsync();
        }

        private void BtnCharts_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        private void ClearEntries()
        {
            txtName.Text = "";
            txtLastName.Text = "";
            txtPhoneNumber.Text = "";
            txtDirection.Text = "";
            txtDni.Text = "";
            txtPassword.Text = "";
            pickerTurn.SelectedItem = null;
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            ClearEntries();
        }

        private async void BtnNewUser_Clicked(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string lastName = txtLastName.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string direction = txtDirection.Text;
            string dni = txtDni.Text;
            string password = txtPassword.Text;
            Turn turn = pickerTurn.SelectedItem as Turn;

            if (name == null ||
                lastName == null ||
                phoneNumber == null ||
                dni == null ||
                password == null ||
                turn == null)
            {
                await DisplayAlert("Empleado", "Formulario Invalido, llenar los campos obligatorios", "OK");
                return;
            }

            User newUser = new User
            {
                UserId = (UserDataStore.Instance.GetUsersDataStore().Count + 1).ToString(),
                Name = name,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Direction = direction,
                DNI = dni,
                Enabled = true,
                FirstLogin = true,
                Password = password,
                ProfileId = 2,
                TurnId = turn.TurnId,
            };

            UserDataStore.Instance.AddUsersDataStore(newUser);
            ClearEntries();
            UserList = UserDataStore.Instance.GetUsersDataStore();
            listEmployees.ItemsSource = null;
            listEmployees.ItemsSource = UserList;

            await DisplayAlert("Empleado", "Empleado registrado correctamente", "OK");
        }

        private void BtnUpdUserState_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var userId = button.CommandParameter as string;
            UserDataStore.Instance.UpdateUserState(userId);

            listEmployees.ItemsSource = null;
            listEmployees.ItemsSource = UserDataStore.Instance.GetUsersDataStore();
        }
    }
}
