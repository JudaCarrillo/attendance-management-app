using attendance_management_app.Models;
using attendance_management_app.Services;
using attendance_management_app.Utils;
using System;
using System.Collections.Generic;
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
            ToggleFrames(true);
        }

        private void OnButtonSelected(Button button)
        {
            // Reset previous selected button styles
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Color.White;
                _selectedButton.TextColor = Color.Black;
                _selectedButton.BorderColor = Color.FromHex("#5E7BFD");
                _selectedButton.BorderWidth = 1;
            }

            // Apply selected styles
            button.BackgroundColor = Color.FromHex("#5E7BFD");
            button.TextColor = Color.White;
            button.BorderWidth = 0;
            _selectedButton = button;
        }

        private void ToggleFrames(bool showEmployeeTable)
        {
            employeeTableFrame.IsVisible = showEmployeeTable;
            registerFrame.IsVisible = !showEmployeeTable;
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
            OnButtonSelected(GestionButton);
            ToggleFrames(true);
        }

        private void RegistroButton_Clicked(object sender, EventArgs e)
        {
            OnButtonSelected(RegistroButton);
            ToggleFrames(false);
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

        private void ClearEntries()
        {
            txtName.Text = "";
            txtLastName.Text = "";
            txtPhoneNumber.Text = "";
            txtEmail.Text = "";
            txtDni.Text = "";
            txtBirthDate.Text = "";
            pickerTurn.SelectedItem = null;
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            ClearEntries();
        }

        private async void BtnNewUser_Clicked(object sender, EventArgs e)
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
                string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(birthDate))

            {
                await DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Error", "El formato del correo electrónico no es válido.", "OK");
                return;
            }

            if (!IsValidDate(birthDate))
            {
                await DisplayAlert("Error", "La fecha de nacimiento no es válida (DD/MM/AAAA).", "OK");
                return;
            }

            User newUser = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Name = name,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Direction = email, // Corregido para almacenar en el campo correcto
                DNI = dni,
                Enabled = true,
                FirstLogin = true,
                ProfileId = 2,
                TurnId = turn.TurnId,
            };

            UserDataStore.Instance.AddUsersDataStore(newUser);
            ClearEntries();
            RefreshUserList();

            await DisplayAlert("Éxito", "Empleado registrado correctamente.", "OK");
            GestionButton_Clicked(GestionButton, new EventArgs());
        }

        private void RefreshUserList()
        {
            listEmployees.ItemsSource = null;
            listEmployees.ItemsSource = new List<User>(UserDataStore.Instance.GetUsersDataStore());
        }

        private void OnUserStateChanged(object sender, ToggledEventArgs e)
        {
            if (sender is Switch switchControl)
            {
                // Obtener el contexto de binding (User)
                var user = switchControl.BindingContext as User;
                if (user == null)
                {
                    // Si no hay binding context, intentar usar el parámetro del comando
                    var userId = SwitchExtensions.GetCommandParameter(switchControl) as string;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        UserDataStore.Instance.UpdateUserState(userId);
                        RefreshUserList();
                    }
                }
                else
                {
                    // Usar directamente el UserId del User
                    UserDataStore.Instance.UpdateUserState(user.UserId);
                    RefreshUserList();
                }
            }
        }

        public static class SwitchExtensions
        {
            public static readonly BindableProperty CommandParameterProperty =
                BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(SwitchExtensions), null);

            public static void SetCommandParameter(BindableObject view, object value)
            {
                view.SetValue(CommandParameterProperty, value);
            }

            public static object GetCommandParameter(BindableObject view)
            {
                return view.GetValue(CommandParameterProperty);
            }
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

        private void OnButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            OnButtonSelected(button);

            if (button == GestionButton)
            {
                ToggleFrames(true); // Mostrar tabla de empleados
            }
            else if (button == RegistroButton)
            {
                ToggleFrames(false); // Mostrar formulario de registro
            }
        }

        private bool IsValidDate(string date)
        {
            return DateTime.TryParseExact(date, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out _);
        }

        private async void BtnEmployee_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrarEmpleado());
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

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            if (button == null) return;

            var userId = button.CommandParameter as string;
            if (string.IsNullOrEmpty(userId)) return;

            // Buscar el usuario seleccionado
            var selectedUser = UserDataStore.Instance.GetUserById(userId);
            if (selectedUser == null) return;

            // Navegar a la página de edición con el usuario como parámetro
            await Navigation.PushAsync(new EditarEmpleado(selectedUser));
        }
    }
}