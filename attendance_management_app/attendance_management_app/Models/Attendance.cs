using System;
using System.Collections.Generic;
using System.Text;

namespace attendance_management_app.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public AttendanceType Type { get; set; }

        public enum AttendanceType
        {
            Early,
            Late,
            Absent
        }
    }
}
