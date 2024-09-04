using System;
using attendance_management_app.Droid.Services;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.App.Job;
using Android.Content;
using System.Runtime.Remoting.Contexts;
using Android.Util;
using Java.Util;
namespace attendance_management_app.Droid
{
    [Activity(Label = "attendance_management_app", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Log.Info("MainActivity", "Init App");
            LoadApplication(new App());
            SetDailyAlarm(21, 09);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetDailyAlarm(int hour, int minute)
        {
            AlarmManager alarmManager = (AlarmManager)GetSystemService(AlarmService);

            Intent intent = new Intent(this, typeof(AttendanceJobService));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            Calendar calendar = Calendar.Instance;
            calendar.Set(CalendarField.HourOfDay, hour);
            calendar.Set(CalendarField.Minute, minute);
            calendar.Set(CalendarField.Second, 0);
            calendar.Set(CalendarField.Millisecond, 0);

            long triggerTime = calendar.TimeInMillis;
            alarmManager.SetRepeating(AlarmType.RtcWakeup, triggerTime, AlarmManager.IntervalDay, pendingIntent);
        }

    }
}