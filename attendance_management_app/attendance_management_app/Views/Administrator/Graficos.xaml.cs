using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        private void DiarioButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            OnButtonSelected(button);

            GraficoDiario.IsVisible = true;
            GraficoMensual.IsVisible = false;
        }

        private void MensualButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            OnButtonSelected(button);

            GraficoDiario.IsVisible = false;
            GraficoMensual.IsVisible = true;

        }
    }
}