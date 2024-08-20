using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class AddCrewViewModel : BaseViewModel
    {
        private string _firstname;
        private string _lastname;

        public string Firstname { get { return _firstname; } set { SetProperty(ref _firstname, value); } }
        public string Lastname { get { return _lastname; } set { SetProperty(ref _lastname, value); } }
        public ICommand AddCrewCommand { private set; get; }
        public AddCrewViewModel() 
        {
            AddCrewCommand = new Command(DoAddCrew);
            Firstname = string.Empty;
            Lastname = string.Empty;
        }

        private async void DoAddCrew(object obj)
        {
            if (Firstname.Length > 0) 
            {
                await AddCrew();
            }

            await Shell.Current.Navigation.PopAsync();
        }
        public async Task AddCrew() {
            await _crewTable.SaveItemAsync(new Objects.DbObjects.DBCrewObject
            {
                Firstname = Firstname,
                Lastname = Lastname,
                IsChecked = true,
            });
        }
    }
}
