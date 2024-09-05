using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
