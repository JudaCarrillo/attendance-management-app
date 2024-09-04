using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using attendance_management_app.Droid;

namespace product_catalog_xamarin.Droid
{
    [Activity(Label = "Guille Library", Icon = "@mipmap/icon",
        Theme = "@style/ScreenPageTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize)]

    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            Finish();
        }
    }
}