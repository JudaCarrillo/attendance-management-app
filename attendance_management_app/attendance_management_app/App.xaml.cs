using attendance_management_app.Views;
using attendance_management_app.Views.Shared;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace attendance_management_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new UpdatePassword())
            {
                BarBackgroundColor = Color.Transparent,
                BackgroundColor = Color.FromHex("#0D0D0D"),
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
