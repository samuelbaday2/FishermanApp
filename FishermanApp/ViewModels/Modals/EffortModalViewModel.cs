using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels.Modals
{
    public class EffortModalViewModel : EnterSetDetailPageViewModel
    {
        private List<string> _setNumberArray;

        public List<string> SetNumberArray { get { return _setNumberArray; } set { SetProperty(ref _setNumberArray, value); } }
        public ICommand CloseCommand { private set; get; }
        public EffortModalViewModel() 
        {
            CloseCommand = new Command(DoClose);

 
        }
        public async Task<List<string>> PopulatePickerAsync(int tripId) {
            SetNumberArray = new List<string>();
            var existingSets = await _tripSetTable.GetItemsAsync();
            int currentSetCount = existingSets.Where(x => x.TripId == tripId).Count();
         
            int counter = 1;
            for(int x = 0; x < currentSetCount; x++)
            {
                SetNumberArray.Add($"{counter++}");
            }

            return SetNumberArray;
        }
        private void DoClose(object obj)
        {
            Shell.Current.Navigation.PopModalAsync();
        }

        public async Task InitializeAsync(int tripId ,int index)
        {
            List<DBSetObject> existingSets = await _tripSetTable.GetItemsAsync();
            List<DBSetObject> currentSetCount = existingSets.Where(x => x.TripId == tripId).ToList();
            try
            {
                var lastSet = await _tripSetTable.GetItemAsync(currentSetCount[index].Id);

                if (lastSet != null)
                {
                    try
                    {
                        LongLineLength = lastSet.LengthOfLongLine;
                    }
                    catch { }

                    try
                    {
                        LiveBait = lastSet.BaitType.ToLower().Contains(AppResources.Live.ToLower());
                    }
                    catch { }

                    try
                    {
                        DeadBait = lastSet.BaitType.ToLower().Contains(AppResources.Dead.ToLower());
                    }
                    catch { }

                    try
                    {
                        GangionLength = lastSet.GangionLength;
                    }
                    catch { }

                    try
                    {
                        BaitSpecie = lastSet.BaitSpecie;
                    }
                    catch { }

                    try
                    {
                        HookType = lastSet.TypeOfHook;
                    }
                    catch { }

                    try
                    {
                        BasketCount = lastSet.NumberOfBaskets;
                    }
                    catch { }

                    try
                    {
                        HooksPerBasket = lastSet.HooksPerBasket;
                    }
                    catch { }

                }
            }
            catch (Exception ee)
            {

            }

        }
    }
}
