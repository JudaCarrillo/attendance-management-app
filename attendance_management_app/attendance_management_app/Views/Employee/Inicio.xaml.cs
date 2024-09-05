using attendance_management_app.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using attendance_management_app.Models;
using attendance_management_app.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace attendance_management_app.Views.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : ContentPage
    {
        private Button _selectedButton;
        public Inicio()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            User currentUser = AuthService.Instance.currentUser;
            Turn userTurn = TurnDataStore.Instance.GetTurnsDataStore().Find(turn => turn.TurnId == currentUser.TurnId);
            BindingContext = userTurn;

            //DateTime today = DateTime.Today;
            //OnSelectDay(today);
        }


        private void OnButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            string clickedDay = button.CommandParameter as string;

            var dayOfWeek = Util.GetDayOfWeek(clickedDay);
            DateTime today = DateTime.Today;
            var targetDate = Util.GetClosestDateForDayOfWeek(today, dayOfWeek);
            OnSelectDay(targetDate);

            ButtonSelectedStyle(button);
        }

        private void OnSelectDay(DateTime date)
        {
            string targetMonht = date.ToString("MM/yyyy");
            string targetDate = date.ToString("dd/MM/yyyy");

            var currentUser = AuthService.Instance.currentUser;

            var attendanceData = AttendanceDataStore.Instance.GetAttendancesData();

            if (!attendanceData.ContainsKey(targetMonht) || !attendanceData[targetMonht].ContainsKey(targetDate))
            {
                AttendanceDataStore.Instance.InitializedAttendanceDataStore(targetMonht, targetDate);
            }

            var record = attendanceData[targetMonht][targetDate];
            var attendanceRecord = record.Early.Find(attendance => attendance.UserId == currentUser.UserId) ??
                                   record.Late.Find(attendance => attendance.UserId == currentUser.UserId) ??
                                   record.Absent.Find(attendance => attendance.UserId == currentUser.UserId);

            if (attendanceRecord == null)
            {
                AttendanceFrame.BackgroundColor = Color.White;
                return;
            }

            switch (attendanceRecord.Type)
            {
                case Attendance.AttendanceType.Early:
                    AttendanceFrame.BackgroundColor = Color.FromHex("#801CDE3B");
                    break;
                case Attendance.AttendanceType.Late:
                    AttendanceFrame.BackgroundColor = Color.FromHex("#80FFAE00");
                    break;
                case Attendance.AttendanceType.Absent:
                    AttendanceFrame.BackgroundColor = Color.FromHex("#80FF0000");
                    break;
            }
        }

        private void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            AuthService.Instance.Logout();
            Navigation.PopToRootAsync();
        }

        private void ButtonSelectedStyle(Button clickedButton)
        {
            // Restaurar el estado del botón previamente seleccionado
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Color.Black;
                _selectedButton.TextColor = Color.White;
            }

            // Establecer el estado del botón actualmente seleccionado
            clickedButton.BackgroundColor = Color.Gray;
            clickedButton.TextColor = Color.Black;

            // Guardar la referencia del botón seleccionado
            _selectedButton = clickedButton;
        }
    }
}