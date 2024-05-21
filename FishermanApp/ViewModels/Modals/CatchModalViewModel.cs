using FishermanApp.Objects.DbObjects;
using FishermanApp.Objects.GroupedObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels.Modals
{
    public class CatchModalViewModel : BaseViewModel
    {
        private ObservableCollection<CatchGroup> _catchDataCollection;

        public ObservableCollection<CatchGroup> CatchDataCollection { get { return _catchDataCollection; } set { SetProperty(ref _catchDataCollection, value); } }
   
        public ICommand CloseCommand { private set; get; }
        public CatchModalViewModel() 
        {
            CloseCommand = new Command(DoClose);
    
            CatchDataCollection = new ObservableCollection<CatchGroup>();
        }
        public async Task Initialize(int tripId) 
        {
            CatchDataCollection = new ObservableCollection<CatchGroup>();
            List<DBCatchObject> list = new List<DBCatchObject>();

            list = await _catchTable.GetItemsAsync();

            var lastTripData = await _tripTable.GetItemsAsync();

            var existingSets = await _tripSetTable.GetItemsAsync();
            int currentSetCount = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).Count();

            for (int counter = 1; counter <= currentSetCount; counter++) 
            {
                CatchDataCollection.Add(new CatchGroup($"Set {counter}", new ObservableCollection<DBCatchObject>(list.Where(x => x.TripId == tripId && x.SetNumber == counter).ToList())));
            }
         
        }
        private void DoClose(object obj)
        {
            Shell.Current.Navigation.PopModalAsync();
        }
    }
}
