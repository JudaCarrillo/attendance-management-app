using System;
using System.Collections.Generic;
using System.Text;
using attendance_management_app.Models;

namespace attendance_management_app.Services
{
    public class TurnDataStore
    {
        private static readonly Lazy<TurnDataStore> instance = new Lazy<TurnDataStore>(() => new TurnDataStore());
        public static TurnDataStore Instance => instance.Value;

        public List<Turn> TurnsDataStore { get; private set; }

        private TurnDataStore()
        {
            TurnsDataStore = new List<Turn>
            {
                new Turn {
                    TurnId = 1,
                    Name = "Part Time Morning",
                    StartTime = "08" ,
                    EndTime = "12",
                    WorkingDays = new List<string> {"1", "2", "3", "4", "5" },
                    WorkingDaysFormat = new List<string> {"Lunes", "Martes", "Miércoles", "Jueves", "Viernes" }
                },


                new Turn {
                    TurnId = 2,
                    Name = "Part Time Afternoon",
                    StartTime = "13" ,
                    EndTime = "17",
                    WorkingDays = new List<string> {"1", "2", "3", "4", "5" },
                    WorkingDaysFormat = new List<string> {"Lunes", "Martes", "Miércoles", "Jueves", "Viernes" }
                },

                new Turn {
                    TurnId = 3,
                    Name = "Full Time",
                    StartTime = "08" ,
                    EndTime = "17",
                    WorkingDays = new List<string> {"1", "2", "3", "4", "5" } ,
                    WorkingDaysFormat = new List < string > { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes" }
                }
            };
        }

        public void AddTurnDataStore(Turn turn)
        {
            TurnsDataStore.Add(turn);
        }

        public List<Turn> GetTurnsDataStore()
        {
            return TurnsDataStore;
        }
    }
}
