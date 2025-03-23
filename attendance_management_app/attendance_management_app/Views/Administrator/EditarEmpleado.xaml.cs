using attendance_management_app.Models;
using attendance_management_app.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace attendance_management_app.Views.Administrador
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarEmpleado : ContentPage
    {
        private User _userToEdit;
        private List<Turn> TurnosList;

        public EditarEmpleado(User user)
        {
            InitializeComponent();
            _userToEdit = user;
            LoadData();
            LoadUserData();
        }

        private void LoadData()
        {
            TurnosList = TurnDataStore.Instance.GetTurnsDataStore();
            pickerTurn.ItemsSource = TurnosList;
            pickerTurn.ItemDisplayBinding = new Binding("Name");
        }

        private void LoadUserData()
        {
            // Cargar los datos del usuario en los campos del formulario
            txtName.Text = _userToEdit.Name;
            txtLastName.Text = _userToEdit.LastName;
            txtPhoneNumber.Text = _userToEdit.PhoneNumber;
            txtEmail.Text = _userToEdit.Direction; // Asumiendo que Direction guarda el email
            txtDni.Text = _userToEdit.DNI;
            
            // Seleccionar el turno del usuario
            foreach (Turn turn in TurnosList)
            {
                if (turn.TurnId == _userToEdit.TurnId)
                {
                    pickerTurn.SelectedItem = turn;
                    break;
                }
            }
            
            // Si tenemos información de cumpleaños, cargarla también
            // Por ahora, dejamos este campo vacío ya que no tenemos esta información
        }

        private async void BtnSaveUser_Clicked(object sender, EventArgs e)
        {
            string name = txtName.Text?.Trim();
            string lastName = txtLastName.Text?.Trim();
            string phoneNumber = txtPhoneNumber.Text?.Trim();
            string email = txtEmail.Text?.Trim();
            string dni = txtDni.Text?.Trim();
            string birthDate = txtBirthDate.Text?.Trim();
            Turn turn = pickerTurn.SelectedItem as Turn;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(dni)) 
                
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Error", "El formato del correo electrónico no es válido.", "OK");
                return;
            }

            // Actualizar los datos del usuario
            _userToEdit.Name = name;
            _userToEdit.LastName = lastName;
            _userToEdit.PhoneNumber = phoneNumber;
            _userToEdit.Direction = email;
            _userToEdit.DNI = dni;
            if (turn != null)
            {
                _userToEdit.TurnId = turn.TurnId;
            }
            
            // Actualizar usuario en el almacén de datos
            UserDataStore.Instance.UpdateUser(_userToEdit);

            await DisplayAlert("Éxito", "Empleado actualizado correctamente.", "OK");
            await Navigation.PopAsync();
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void BtnLogOut_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Cerrar sesión", "¿Estás seguro de que deseas cerrar sesión?", "Sí", "No");
            if (confirm)
            {
                AuthService.Instance.Logout();
                await Navigation.PopToRootAsync();
            }
        }

        private async void BtnEmployee_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}