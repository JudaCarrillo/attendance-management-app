using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using attendance_management_app.Services;
using attendance_management_app.Models;
using System.Globalization;
using attendance_management_app.Utils;
using System.Linq;
using System.Threading.Tasks;

//USAR LOS PAQUETES MICROCHARTS
using Microcharts;
using Entry = Microcharts.ChartEntry;

//USAMOS SKIASHARP PARA COLORES DE GRÁFICOS
using SkiaSharp;



namespace attendance_management_app
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }


    }
}
