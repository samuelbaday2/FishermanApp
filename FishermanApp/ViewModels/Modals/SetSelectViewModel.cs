using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.ViewModels.Modals
{
    public class SetSelectViewModel : BaseViewModel
    {
        private ObservableCollection<SetNumberObject> _setCollection;

        public ObservableCollection<SetNumberObject> SetCollection { get { return _setCollection; } set { SetProperty(ref _setCollection, value); } }
        public SetSelectViewModel() 
        {
            
        }

        public async Task LoadData()
        {
            SetCollection = new ObservableCollection<SetNumberObject>();

            var lastTripData = await _tripTable.GetItemsAsync();
            var existingSets = await _tripSetTable.GetItemsAsync();

            List<DBSetObject> currentSetCount = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).ToList();

            for (int i = 0; i < currentSetCount.Count(); i++) 
            {
                SetCollection.Add(new SetNumberObject { 
                    Index = i,
                    SetNumber = $"Set {i+1}",
                    SetId = currentSetCount[i].Id,
                });
            }
        }
    }
}
