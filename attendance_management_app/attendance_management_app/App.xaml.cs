﻿using attendance_management_app.Views;
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

            MainPage = new NavigationPage(new splash_screen());
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
