using System;
using System.Collections.Generic;
using System.Text;
using attendance_management_app.Models;
using attendance_management_app.Utils;
using static attendance_management_app.Models.Attendance;

namespace attendance_management_app.Services
{
    public class AttendanceDataStore
    {
        private static readonly Lazy<AttendanceDataStore> instance = new Lazy<AttendanceDataStore>(() => new AttendanceDataStore());
        public static AttendanceDataStore Instance => instance.Value;

        private Dictionary<string, Dictionary<string, AttendanceRecord>> _attendancesDataStore;


        private AttendanceDataStore()
        {
            _attendancesDataStore = new Dictionary<string, Dictionary<string, AttendanceRecord>>();
        }

        public void AddAttendanceDataStore(string monthYear, string date, Attendance attendance)
        {
            if (!_attendancesDataStore.ContainsKey(monthYear) || !!_attendancesDataStore[monthYear].ContainsKey(date))
            {
                InitializedAttendanceDataStore(monthYear, date);
            }

            var record = _attendancesDataStore[monthYear][date];

            switch (attendance.Type)
            {
                case AttendanceType.Early:
                    record.Early.Add(attendance);
                    break;
                case AttendanceType.Late:
                    record.Late.Add(attendance);
                    break;
                case AttendanceType.Absent:
                    record.Absent.Add(attendance);
                    break;
            }
        }

        public void InitializedAttendanceDataStore(string monthYear, string date)
        {
            _attendancesDataStore[monthYear] = new Dictionary<string, AttendanceRecord>();
            _attendancesDataStore[monthYear][date] = new AttendanceRecord { Date = date };
        }

        public Dictionary<string, Dictionary<string, AttendanceRecord>> GetAttendancesData()
        {
            return _attendancesDataStore;
        }

        public void UpdateAttendanceAbsentHistory(string month, string date, DateTime dateTime)
        {
            var users = UserDataStore.Instance.GetUsersDataStore();
            var attendanceHistory = AttendanceDataStore.Instance.GetAttendancesData();

            if (!attendanceHistory.ContainsKey(month) || !attendanceHistory[month].ContainsKey(date))
            {
                AttendanceDataStore.Instance.InitializedAttendanceDataStore(month, date);
            }

            var earlyAttendanceRecords = attendanceHistory[month][date].Early;
            var lateAttendanceRecords = attendanceHistory[month][date].Late;

            foreach (var user in users)
            {
                bool isUserMarkedAsEarly = earlyAttendanceRecords.Exists(attendance => attendance.UserId == user.UserId);
                bool isUserMarkedAsLate = lateAttendanceRecords.Exists(attendance => attendance.UserId == user.UserId);

                if (isUserMarkedAsEarly && isUserMarkedAsLate) continue;

                AttendanceDataStore.Instance.AddAttendanceDataStore(
                    month,
                    date,
                    new Attendance
                    {
                        UserId = user.UserId,
                        DateTime = dateTime,
                        AttendanceId = Util.GenerateUniqueId(),
                        Type = Attendance.AttendanceType.Absent
                    }
                );
            }
        }

    }

    public class AttendanceRecord
    {
        public string Date { get; set; }
        public List<Attendance> Early { get; set; } = new List<Attendance>();
        public List<Attendance> Late { get; set; } = new List<Attendance>();
        public List<Attendance> Absent { get; set; } = new List<Attendance>();
    }

}
