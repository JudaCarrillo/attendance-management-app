using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using attendance_management_app.Services;
using attendance_management_app.Utils;
using Microcharts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace attendance_management_app.Views.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadChart(DateTime.Today);
        }
        private void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            AuthService.Instance.Logout();
            Navigation.PopToRootAsync();
        }

        private void LoadChart(DateTime date)
        {
            var currentUserId = AuthService.Instance.currentUser.UserId;
            var entries = Util.CreateEntriesForMonthChartToUser(currentUserId, date);
            monthlyChart.Chart = new DonutChart { Entries = entries };
        }

        private void OnHomeView(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}