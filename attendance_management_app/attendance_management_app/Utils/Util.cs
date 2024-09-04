using System;
using System.Collections.Generic;
using System.Text;

namespace attendance_management_app.Utils
{
    public class Util
    {

        public static string GetDayOfWeek(int day)
        {
            switch (day)
            {
                case 0:
                    return "Domingo";
                case 1:
                    return "Lunes";
                case 2:
                    return "Martes";
                case 3:
                    return "Miércoles";
                case 4:
                    return "Jueves";
                case 5:
                    return "Viernes";
                case 6:
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

    }
}
