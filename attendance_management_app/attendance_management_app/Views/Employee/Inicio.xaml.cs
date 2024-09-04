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
		public Inicio ()
		{
			InitializeComponent ();
			LoadData();
        }

		private void LoadData()
		{
            User currentUser = AuthService.Instance.currentUser;
			Turn userTurn = TurnDataStore.Instance.GetTurnsDataStore().Find(turn => turn.TurnId == currentUser.TurnId);
			BindingContext = userTurn;
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}