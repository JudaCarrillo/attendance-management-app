using System;
using System.Linq;
using System.Xml;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Util;
using attendance_management_app.Services;
using Java.Util;

namespace attendance_management_app.Droid.Services
{

    [BroadcastReceiver(Enabled = true)]
    public class AttendanceJobService : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {

            DateTime dateTime = DateTime.Now;
            Log.Info("AttendanceJobService", $"Ejecutando cron job {dateTime.ToString()}");
            string month = dateTime.ToString("M/yyyy");
            string date = dateTime.ToString("d/M/yyyy");
            AttendanceDataStore.Instance.UpdateAttendanceAbsentHistory(month, date, dateTime);
        }
    }
}