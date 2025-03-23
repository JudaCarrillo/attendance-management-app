using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using attendance_management_app.Services;
using attendance_management_app.Utils;
using attendance_management_app.Views.Administrador;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;

namespace attendance_management_app.Views.Administrator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Graficos : ContentPage
    {
        private Button _selectedButton;
        public Graficos()
        {
            InitializeComponent();
            OnButtonSelected(DiarioButton);
            GraficoMensual.IsVisible = false;

            var entries = Util.CreateEntriesForDailyChart(DateTime.Today);
            dailyChart.Chart = new DonutChart { Entries = entries };
        }
        private void OnButtonSelected(Button button)
        {
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Color.White;
                _selectedButton.TextColor = Color.Black; // Restaurar color de texto
            }

            // Aplicamos el estilo de "seleccionado" al botón que fue presionado
            button.BackgroundColor = Color.FromHex("#6A83FF");
            button.TextColor = Color.White; // Cambiar el color del texto

            // Actualizamos el botón seleccionado
            _selectedButton = button;
        }


        private void DiarioButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            OnButtonSelected(button);

            GraficoDiario.IsVisible = true;
            GraficoMensual.IsVisible = false;

            var entries = Util.CreateEntriesForDailyChart(DateTime.Today);
            dailyChart.Chart = new DonutChart { Entries = entries };
        }

        private void MensualButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            OnButtonSelected(button);

            GraficoDiario.IsVisible = false;
            GraficoMensual.IsVisible = true;
            var entries = Util.CreateEntriesForMonthChart(DateTime.Today);
            monthlyChart.Chart = new DonutChart { Entries = entries };
        }

        private void BtnLogOut_Clicked(object sender, EventArgs e)
        {
            AuthService.Instance.Logout();
            Navigation.PopAsync();
        }

        private async void BtnEmployee_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrarEmpleado());
        }
    }
}