using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            InitializeTestData();
        }

        public void AddAttendanceDataStore(string monthYear, string date, Attendance attendance)
        {
            if (!_attendancesDataStore.ContainsKey(monthYear) || !_attendancesDataStore[monthYear].ContainsKey(date))
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
            if (!_attendancesDataStore.ContainsKey(monthYear))
            {
                _attendancesDataStore[monthYear] = new Dictionary<string, AttendanceRecord>();
            }

            if (!_attendancesDataStore[monthYear].ContainsKey(date))
            {
                _attendancesDataStore[monthYear][date] = new AttendanceRecord { Date = date };
            }
        }

        public Dictionary<string, Dictionary<string, AttendanceRecord>> GetAttendancesData()
        {
            return _attendancesDataStore;
        }

        public string GetAttendanceSummary()
        {
            var summary = new StringBuilder();

            foreach (var monthYear in _attendancesDataStore.Keys)
            {
                summary.AppendLine($"Mes/Año: {monthYear}");
                foreach (var date in _attendancesDataStore[monthYear].Keys)
                {
                    var record = _attendancesDataStore[monthYear][date];
                    summary.AppendLine($"  Fecha: {date}");
                    summary.AppendLine($"    Temprano: {record.Early.Count} registros");
                    summary.AppendLine($"    Tarde: {record.Late.Count} registros");
                    summary.AppendLine($"    Ausente: {record.Absent.Count} registros");
                }
            }

            return summary.ToString();
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

        private void InitializeTestData()
        {
            var users = new List<string> { "1", "2" };

            string monthYear = "09/2024";

            for (int i = 1; i <= 4; i++)
            {
                string date = $"{i:D2}/09/2024";

                foreach (var user in users)
                {
                    AddAttendanceDataStore(monthYear, date, new Attendance
                    {
                        UserId = user,
                        DateTime = new DateTime(2024, 9, i),
                        AttendanceId = Util.GenerateUniqueId(),
                        Type = (AttendanceType)(i % 3)
                    });
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
}