using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;
using attendance_management_app.Models;

namespace attendance_management_app.ViewModel
{
    public class CalendarViewModel : BindableObject
    {
        public EventCollection Events { get; set; }

        public CalendarViewModel()
        {
            Events = new EventCollection
            {
                // Día con evento rojo (Falta)
                [DateTime.Now.AddDays(-1)] = new DayEventCollection<EventModel>(Color.Red, Color.Red)
                {
                    new EventModel { Name = "Falta", Description = "El usuario no asistió" }
                },

                // Día con evento naranja (Tardanza)
                [DateTime.Now] = new DayEventCollection<EventModel>(Color.Orange, Color.Orange)
                {
                    new EventModel { Name = "Tardanza", Description = "El usuario llegó tarde" }
                },

                // Día con evento verde (Presente)
                [DateTime.Now.AddDays(1)] = new DayEventCollection<EventModel>(Color.Green, Color.Green)
                {
                    new EventModel { Name = "Asistencia", Description = "El usuario asistió a tiempo" }
                }
            };
        }
    }
}
