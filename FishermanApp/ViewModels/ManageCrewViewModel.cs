using FishermanApp.Objects.DbObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class ManageCrewViewModel : BaseViewModel
    {
        private ObservableCollection<DBCrewObject> _crewList;
        private bool _isRefreshing;

        public bool IsRefreshing { get { return _isRefreshing; } set { SetProperty(ref _isRefreshing, value); } }
        public ObservableCollection<DBCrewObject> CrewList { get { return _crewList; } set { SetProperty(ref _crewList, value); } }

        public ICommand RefreshCommand { private set; get; }
        public ManageCrewViewModel() 
        {
            RefreshCommand = new Command(DoRefresh);
            CrewList = new ObservableCollection<DBCrewObject>();
        }    
        public async Task GetAllCrew() 
        {
            CrewList = new ObservableCollection<DBCrewObject>( await _crewTable.GetItemsAsync());
            IsRefreshing = false;
        }
        private void DoRefresh(object obj)
        {
            GetAllCrew();
        }
        public async Task DeleteCrew(DBCrewObject dBCrewObject) 
        {
            await _crewTable.DeleteItemAsync(dBCrewObject);
            GetAllCrew();
        }
    }
}
