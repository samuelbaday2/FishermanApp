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
        private DateTime _minimumPickerDate;
        private bool IsTaskRunning = false;
        private bool _isRefreshing;
        public DateTime SelectedDate { get { return _selectedDate; } set { SetProperty(ref _selectedDate, value); } }
        public DateTime MinimumPickerDate { get { return _minimumPickerDate; } set { SetProperty(ref _minimumPickerDate, value); } }
        public bool IsRefreshing { get { return _isRefreshing; } set { SetProperty(ref _isRefreshing, value); } }

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

            IsTaskRunning = true;
            TripCollection = new ObservableCollection<TripHistoryObject>();
            List<DbTripObject> tripList = await _tripTable.GetItemsAsync(SelectedDate);

            foreach (DbTripObject obj in tripList)
            {
                TripHistoryObject newTripObject = obj;

                var sets = await _tripSetTable.GetItemsAsync();
                newTripObject.NumberOfSets = $"Times Fished: {sets.Where(x => x.TripId == newTripObject.Id).Count()}";

                TripCollection.Add(newTripObject);
            }
            IsTaskRunning = false;
        }
        public async Task InitializeAsync()
        {
            //SelectedDate = DateTime.Now;

            //await FilterTripsAsync();
        }

        public async Task SetMinimumDateAsync()
        {
            DbTripObject firstTripObject = await _tripTable.GetItemAsync(0);
            MinimumPickerDate = firstTripObject.RecordedOn;

        }
    }
}
