using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

//USAR LOS PAQUETES MICROCHARTS
using Microcharts;
using Entry = Microcharts.ChartEntry;

//USAMOS SKIASHARP PARA COLORES DE GRÁFICOS
using SkiaSharp;



namespace attendance_management_app
{
    public partial class MainPage : ContentPage
    {
        List<Entry> entryList;

        public MainPage()
        {
            InitializeComponent();
            entryList = new List<Entry>();
            // Cargar Lista de Entradas
            LoadEntries();
            // Asignar los Datos a los gráficos en la Vista
            barChart.Chart = new BarChart()
            {
                Entries = entryList
            };
            pieChart.Chart = new PieChart()
            {
                Entries = entryList
            };
            donutChart.Chart = new DonutChart()
            {
                Entries = entryList
            };
            linesChart.Chart = new LineChart()
            {
                Entries = entryList
            };
        }

        private void LoadEntries ()
        {
            Entry e1 = new Entry(60)
            {
                Label = "A",
                ValueLabel = "60",
                Color = SKColor.Parse("#00bcd4")
            };

            Entry e2 = new Entry(40)
            {
                Label = "A",
                ValueLabel = "40",
                Color = SKColor.Parse("#000000")
            };

            Entry e3 = new Entry(50)
            {
                Label = "A",
                ValueLabel = "50",
                Color = SKColor.Parse("#6DC36D")
            };

            Entry e4 = new Entry(70)
            {
                Label = "A",
                ValueLabel = "70",
                Color = SKColor.Parse("#024a86")
            };
            entryList.Add(e1);
            entryList.Add(e2);
            entryList.Add(e3);
            entryList.Add(e4);
        }
    }
}
