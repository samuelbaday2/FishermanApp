using CommunityToolkit.Mvvm.Messaging;
using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
using FishermanApp.ViewModels.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class EnterSetDetailPageViewModel : BaseViewModel
    {
        private string _setNumber;
        private string _basketCount;
        private string _hooksPerBasket;
        private string _gangionLength;
        private string _longLineLength;
        private string _baitSpecie;
        private string _hookType;
        private bool _liveBait;
        private bool _deadBait;

        public bool LiveBait { get { return _liveBait; } set { SetProperty(ref _liveBait, value); } }
        public bool DeadBait { get { return _deadBait; } set { SetProperty(ref _deadBait, value); } }
        public string GangionLength { get { return _gangionLength; } set { SetProperty(ref _gangionLength, value); } }
        public string LongLineLength { get { return _longLineLength; } set { SetProperty(ref _longLineLength, value); } }
        public string BaitSpecie { get { return _baitSpecie; } set { SetProperty(ref _baitSpecie, value); } }
        public string HookType { get { return _hookType; } set { SetProperty(ref _hookType, value); } }
        public string SetNumber { get { return _setNumber; } set { SetProperty(ref _setNumber, value); } }
        public string BasketCount { get { return _basketCount; } set { SetProperty(ref _basketCount, value); } } 
        public string HooksPerBasket { get { return _hooksPerBasket; } set { SetProperty(ref _hooksPerBasket, value); } }

        public ICommand LiveButtonCommand { private set; get; }
        public ICommand DeadButtonCommand { private set; get; }
        public ICommand EndSetCommand { private set; get; }
        private bool AddRecentSet = false;

        public EnterSetDetailPageViewModel() 
        {
            LiveButtonCommand = new Command(LiveButtonClicked);
            DeadButtonCommand = new Command(DeadButtonClicked);
            EndSetCommand = new Command(DoEndSet);

            InitializeWeakReferences();

            
        }
        public async Task AddLastSetAsync() 
        {
            ConfigClass.GetRecentSet = true;         
        }
        public void InitializeWeakReferences() 
        {
            MessagingCenter.Subscribe<HookTypeSelectionViewModel, SelectionObject>(this, AppResources.HookType, async (sender, arg) =>
            {
                HookType = arg.SelectionTitle;
            });

            MessagingCenter.Subscribe<SpeciesSelectionViewModel, SelectionObject>(this, AppResources.BaitSpecie, async (sender, arg) =>
            {
                BaitSpecie = arg.SelectionTitle;
            });
        }
        private async void DoEndSet(object obj)
        {
            SetBusyStatusAsync(false);
            if (sephamoreSlim.CurrentCount == 0)
            {
                return;
            }

            try
            {
                sephamoreSlim.Wait();
                var gps = await GetCurrentLocation();

                var lastTripData = await _tripTable.GetItemsAsync();
                var existingSets = await _tripSetTable.GetItemsAsync();
                DBSetObject currentSet = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).LastOrDefault();

                if (LiveBait) 
                {
                    currentSet.BaitType = AppResources.Live.ToLower();
                }
                if (DeadBait)
                {
                    currentSet.BaitType = AppResources.Dead.ToLower();
                }

                currentSet.EndSetLatitude = gps == null ? AppResources.GpsOff : gps.Latitude.ToString();
                currentSet.EndSetpLongitude = gps == null ? AppResources.GpsOff : gps.Longitude.ToString();
                currentSet.SetEnded = true;
                currentSet.BaitSpecie = BaitSpecie;
                
                currentSet.LengthOfLongLine = LongLineLength;
                currentSet.GangionLength = GangionLength;
                currentSet.SetEndedOn = DateTime.Now;
                currentSet.TypeOfHook = HookType;
                currentSet.HooksPerBasket = HooksPerBasket;
                currentSet.NumberOfBaskets = BasketCount;


                await _tripSetTable.SaveItemAsync(currentSet);

                InitializeConfig.InitializeFunction = true;
                Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            finally
            {
                sephamoreSlim.Release();
                SetBusyStatusAsync(true);
            }
        }

        private async void DeadButtonClicked(object obj)
        {
            LiveBait = false;
            DeadBait = !LiveBait;
        }

        private async void LiveButtonClicked(object obj)
        {
            LiveBait = true;
            DeadBait = !LiveBait;
        }

        public async Task InitializeAsync() 
        {
            try
            {
                var lastTripData = await _tripTable.GetItemsAsync();
                var existingSets = await _tripSetTable.GetItemsAsync();

                int currentSetCount = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).Count();

                if (ConfigClass.GetRecentSet == true) {
                    ConfigClass.GetRecentSet = false;
                   
                    var lastSet = existingSets.Where(x => x.Id == (existingSets.LastOrDefault().Id - 1)).LastOrDefault();

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
                SetNumber = currentSetCount.ToString();
            }
            catch(Exception ee) 
            {
            
            }
         
        }
    }
}
