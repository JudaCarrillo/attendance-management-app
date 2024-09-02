using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace attendance_management_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class splash_screen : ContentPage
    {
        public splash_screen()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var fadeIn = new Animation(v => SplashImage.Opacity = v, 0, 1);
            var fadeOut = new Animation(v => SplashImage.Opacity = v, 1, 0);

            uint totalDuration = 5000; 


            var animation = new Animation();
            animation.Add(0, 0.5, fadeIn);
            animation.Add(0.5, 1, fadeOut);


            animation.Commit(this, "SplashAnimation", length: totalDuration, easing: Easing.CubicInOut, finished: async (v, c) =>
            {
 
                await Navigation.PushAsync(new Login(), true);
            });
        }
    }
}