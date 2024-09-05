using attendance_management_app.Models;
using attendance_management_app.Services;
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
    }
}