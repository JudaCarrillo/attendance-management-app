using System;
using System.Collections.Generic;
using System.Text;
using attendance_management_app.Models;

namespace attendance_management_app.Services
{
    public class AuthService
    {
        private static readonly Lazy<AuthService> instance = new Lazy<AuthService>(() => new AuthService());
        public static AuthService Instance => instance.Value;
        public User currentUser { get; private set; }

        private AuthService()
        {
            currentUser = null;
        }

        public void Login(User updateUser)
        {
            currentUser = updateUser;
        }

        public void Logout()
        {
            currentUser = null;
        }
    }
}
