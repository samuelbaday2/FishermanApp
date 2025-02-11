using FishermanApp.Objects;
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
    public class TripHistoryPageViewModel : BaseViewModel
    {
        private ObservableCollection<TripHistoryObject> _tripCollection;
        private DateTime _selectedDate;
        private DateTime _selectedMaxDate;
        private DateTime _minimumPickerDate;
        private DateTime _maximumPickerDate;
        private bool IsTaskRunning = false;
        private bool _isRefreshing;
        private bool _isOnline;
        public DateTime SelectedDate { get { return _selectedDate; } set { SetProperty(ref _selectedDate, value); } }
        public DateTime SelectedMaxDate { get { return _selectedMaxDate; } set { SetProperty(ref _selectedMaxDate, value); } }
        public DateTime MinimumPickerDate { get { return _minimumPickerDate; } set { SetProperty(ref _minimumPickerDate, value); } }
        public DateTime MaximumPickerDate { get { return _maximumPickerDate; } set { SetProperty(ref _maximumPickerDate, value); } }
        public bool IsRefreshing { get { return _isRefreshing; } set { SetProperty(ref _isRefreshing, value); } }
        public bool IsOnline { get { return _isOnline; } set { SetProperty(ref _isOnline, value); } }

        public ICommand ViewCatchCommand { private set; get; }
        public ICommand ViewEffortCommand { private set; get; }
        public ICommand RefreshCommand { private set; get; }
        public ObservableCollection<TripHistoryObject> TripCollection { get { return _tripCollection; } set { SetProperty(ref _tripCollection, value); } }
        public TripHistoryPageViewModel() 
        {
            TripCollection = new ObservableCollection<TripHistoryObject>();
            ViewCatchCommand = new Command(DoViewCatch);
            ViewEffortCommand = new Command(DoViewEffort);
            RefreshCommand = new Command(DoRefresh);

            IsOnline = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }

        private async void DoViewEffort(object obj)
        {
           
        }

        private async void DoRefresh(object obj)
        {
            await FilterTripsAsync();
            IsRefreshing = false;
        }

        private async void DoViewCatch(object obj)
        {
           
        }

        public async Task FilterTripsAsync() 
        {
            if(IsTaskRunning) return;

            IsOnline = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            IsTaskRunning = true;
            TripCollection = new ObservableCollection<TripHistoryObject>();

            try
            {
                List<DbTripObject> tripList = await _tripTable.GetItemsAsync(SelectedDate,SelectedMaxDate);

                if(tripList!= null)
                foreach (DbTripObject obj in tripList)
                {
                    TripHistoryObject newTripObject = obj;

                    var sets = await _tripSetTable.GetItemsAsync();
                    newTripObject.NumberOfSets = $"Times Fished: {sets.Where(x => x.TripId == newTripObject.Id).Count()}";

                        newTripObject.IsOnline = IsOnline;
                        if (newTripObject.StartTripLatitude.Equals("GPS OFF"))
                        {
                            newTripObject.IsOnline = false;
                        }

                    
                    TripCollection.Add(newTripObject);
                }
            }
            catch { }
            finally {
                IsTaskRunning = false;
            }       
        }
        public async Task InitializeAsync()
        {
            //SelectedDate = DateTime.Now;

            //await FilterTripsAsync();
        }

        public async Task SetMinimumDateAsync()
        {
            try
            {
                DbTripObject firstTripObject = await _tripTable.GetItemAsync(0);
                if (firstTripObject != null)
                {
                    MinimumPickerDate = firstTripObject.RecordedOn;
                }
                else 
                {
                    MinimumPickerDate = DateTime.Now.Date;
                }
            }
            catch {
                MinimumPickerDate = DateTime.Now.Date;
            }        

            MaximumPickerDate = DateTime.Now.Date;
        }
    }
}
