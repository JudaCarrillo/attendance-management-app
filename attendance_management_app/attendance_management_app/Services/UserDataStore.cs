using System;
using System.Collections.Generic;
using System.Text;
using attendance_management_app.Models;
using attendance_management_app.Utils;

namespace attendance_management_app.Services
{
    public class UserDataStore
    {
        private static readonly Lazy<UserDataStore> instance = new Lazy<UserDataStore>(() => new UserDataStore());
        public static UserDataStore Instance => instance.Value;
        public List<User> UsersDataStore { get; private set; }

        private UserDataStore()
        {
            UsersDataStore = new List<User>
            {
                new User {
                    UserId = "1",
                    Name = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "123456789",
                    Direction= "SENATI",
                    DNI = "701565",
                    Password = "admin",
                    Enabled = true,
                    FirstLogin = true,
                    TurnId = 1,
                    ProfileId = 1,
                },

                new User
                {
                    UserId = "2",
                    Name = "User",
                    LastName = "User",
                    PhoneNumber = "987654321",
                    Direction = "SENATI",
                    DNI = "701566",
                    Password = "user",
                    Enabled = true,
                    FirstLogin = false,
                    TurnId = 1,
                    ProfileId = 2,
                }

            };
        }

        public void AddUsersDataStore(User user)
        {
            UsersDataStore.Add(user);
        }

        public List<User> GetUsersDataStore()
        {
            return UsersDataStore;
        }
    }
}
