using System;
using System.Collections.Generic;
using System.Text;

namespace attendance_management_app.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Direction { get; set; }
        public string Password { get; set; }
        public string DNI { get; set; }
        public bool FirstLogin { get; set; }
        public bool Enabled { get; set; }
        public int ProfileId { get; set; }
        public int TurnId { get; set; }
    }
}
