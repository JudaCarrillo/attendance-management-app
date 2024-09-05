using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using ChartEntry = Microcharts.ChartEntry;
using SkiaSharp;
using attendance_management_app.Services;
using Microcharts.Forms;

namespace attendance_management_app.Utils
{
    public class Util
    {

        public static int GetDayOfWeek(string day)
        {
            switch (day)
            {
                case "Domingo":
                case "Sunday":
                    return 0;
                case "Lunes":
                case "Monday":
                    return 1;
                case "Martes":
                case "Tuesday":
                    return 2;
                case "Miércoles":
                case "Wednesday":
                    return 3;
                case "Jueves":
                case "Thursday":
                    return 4;
                case "Viernes":
                case "Friday":
                    return 5;
                case "Sábado":
                case "Saturday":
                    return 6;
                default:
                    return 0;
            }
        }

        public static string GetDayOfWeekInSpanish(string day)
        {
            switch (day)
            {

                case "Sunday":
                    return "Domingo";
                case "Monday":
                    return "Lunes";
                case "Tuesday":
                    return "Martes";
                case "Wednesday":
                    return "Miércoles";
                case "Thursday":
                    return "Jueves";
                case "Friday":
                    return "Viernes";
                case "Saturday":
                    return "Sábado";
                default:
                    return "";
            }
        }

        public static string GenerateUniqueId()
        {

            DateTime now = DateTime.Now;
            string timestamp = now.ToString("yyyyMMddHHmmssffff");
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            string uniqueId = $"{timestamp}-{randomNumber}";
            return uniqueId;
        }

        public static DateTime GetClosestDateForDayOfWeek(DateTime dateReference, int dayOfWeek)
        {
            int daysDifference = dayOfWeek - (int)dateReference.DayOfWeek;
            DateTime targetDate = dateReference.AddDays(daysDifference);
            return targetDate;
        }

        public static List<ChartEntry> CreateEntriesForDailyChart(DateTime date)
        {
            string monthYear = date.ToString("MM/yyyy");
            string dayDate = date.ToString("dd/MM/yyyy");

            var attendanceData = AttendanceDataStore.Instance.GetAttendancesData();

            if (!attendanceData.ContainsKey(monthYear) || !attendanceData[monthYear].ContainsKey(dayDate))
            {
                AttendanceDataStore.Instance.InitializedAttendanceDataStore(monthYear, dayDate);
            }

            var record = attendanceData[monthYear][dayDate];

            int earlyCount = record.Early.Count;
            int lateCount = record.Late.Count;
            int absentCount = record.Absent.Count;

            var entries = new List<ChartEntry>
            {
                new ChartEntry(earlyCount)
                {
                    Label = "Asistencias",
                    ValueLabel = earlyCount.ToString(),
                    ValueLabelColor = SKColor.Parse("#2ecc71"),
                    Color = SKColor.Parse("#2ecc71")
                },
                new ChartEntry(lateCount)
                {
                    Label = "Tardanzas",
                    ValueLabel = lateCount.ToString(),
                    ValueLabelColor = SKColor.Parse("#f1c40f"),
                    Color = SKColor.Parse("#f1c40f")
                },
                new ChartEntry(absentCount)
                {
                    Label = "Faltas",
                    ValueLabel = absentCount.ToString(),
                    ValueLabelColor = SKColor.Parse("#e74c3c"),
                    Color = SKColor.Parse("#e74c3c")
                }
            };

            return entries;
        }

        public static List<ChartEntry> CreateEntriesForMonthChart(DateTime date)
        {
            string monthYear = date.ToString("MM/yyyy");
            string day = date.ToString("dd/MM/yyyy");

            var attendanceData = AttendanceDataStore.Instance.GetAttendancesData();

            if (!attendanceData.ContainsKey(monthYear))
            {
                AttendanceDataStore.Instance.InitializedAttendanceDataStore(monthYear, day);
            }

            var record = attendanceData[monthYear];

            int earlyCount = 0;
            int lateCount = 0;
            int absentCount = 0;

            foreach (var dailyRecord in record)
            {
                earlyCount += dailyRecord.Value.Early.Count;
                lateCount += dailyRecord.Value.Late.Count;
                absentCount += dailyRecord.Value.Absent.Count;
            }

            var entries = new List<ChartEntry>
            {
                new ChartEntry(earlyCount)
                {
                    Label = "Asistencias",
                    ValueLabel = earlyCount.ToString(),
                    ValueLabelColor = SKColor.Parse("#2ecc71"),
                    Color = SKColor.Parse("#2ecc71")
                },
                new ChartEntry(lateCount)
                {
                    Label = "Tardanzas",
                    ValueLabel = lateCount.ToString(),
                    ValueLabelColor = SKColor.Parse("#f1c40f"),
                    Color = SKColor.Parse("#f1c40f")
                },
                new ChartEntry(absentCount)
                {
                    Label = "Faltas",
                    ValueLabel = absentCount.ToString(),
                    ValueLabelColor = SKColor.Parse("#e74c3c"),
                    Color = SKColor.Parse("#e74c3c")
                }
            };

            return entries;
        }

        public static List<ChartEntry> CreateEntriesForMonthChartToUser(string userId, DateTime date)
        {
            string monthYear = date.ToString("MM/yyyy");
            string day = date.ToString("dd/MM/yyyy");

            var attendanceData = AttendanceDataStore.Instance.GetAttendancesData();
            if (!attendanceData.ContainsKey(monthYear))
            {
                AttendanceDataStore.Instance.InitializedAttendanceDataStore(monthYear, day);
            }

            var record = attendanceData[monthYear];
            int earlyCount = 0;
            int lateCount = 0;
            int absentCount = 0;

            foreach (var dailyRecord in record)
            {
                earlyCount += dailyRecord.Value.Early.FindAll(attendance => attendance.UserId == userId).Count;
                lateCount += dailyRecord.Value.Late.FindAll(attendance => attendance.UserId == userId).Count;
                absentCount += dailyRecord.Value.Absent.FindAll(attendance => attendance.UserId == userId).Count;
            }

            var entries = new List<ChartEntry>
            {
                new ChartEntry(earlyCount)
                {
                    Label = "Asistencias",
                    ValueLabelColor = SKColor.Parse("#2ecc71"),
                    ValueLabel = earlyCount.ToString(),
                    Color = SKColor.Parse("#2ecc71")
                },
                new ChartEntry(lateCount)
                {
                    Label = "Tardanzas",
                    ValueLabel = lateCount.ToString(),
                    ValueLabelColor = SKColor.Parse("#f1c40f"),
                    Color = SKColor.Parse("#f1c40f")
                },
                new ChartEntry(absentCount)
                {
                    Label = "Faltas",
                    ValueLabel = absentCount.ToString(),
                    ValueLabelColor = SKColor.Parse("#e74c3c"),
                    Color = SKColor.Parse("#e74c3c")
                }
            };

            return entries;
        }

    }
}
