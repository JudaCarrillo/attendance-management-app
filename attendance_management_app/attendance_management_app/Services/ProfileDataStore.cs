using System;
using System.Collections.Generic;
using System.Text;
using attendance_management_app.Models;

namespace attendance_management_app.Services
{
    public class ProfileDataStore
    {
        private static readonly Lazy<ProfileDataStore> instance = new Lazy<ProfileDataStore>(() => new ProfileDataStore());
        public static ProfileDataStore Instance => instance.Value;
        public List<Profile> ProfilesDataStore { get; private set; }

        private ProfileDataStore()
        {
            ProfilesDataStore = new List<Profile>
            {
                new Profile{ ProfileId = 1, Name = "Administrator"},
                new Profile{ ProfileId = 2, Name = "Employee"}
            };
        }

        public void AddProfileDataStore(Profile profile)
        {
            ProfilesDataStore.Add(profile);
        }

        public List<Profile> GetProfilesDataStore()
        {
            return ProfilesDataStore;
        }
    }
}
