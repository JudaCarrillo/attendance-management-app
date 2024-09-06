using System;
using System.Collections.Generic;
using System.Text;

namespace attendance_management_app.Models
{
    public class Turn
    {
        public int TurnId { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<string> WorkingDays { get; set; }
        public List<string> WorkingDaysFormat { get; set; }
    }
}
