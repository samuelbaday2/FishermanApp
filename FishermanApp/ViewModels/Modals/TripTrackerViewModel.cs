using FishermanApp.Objects.DbObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels.Modals
{
    public class TripTrackerViewModel : BaseViewModel
    {
        private string _tripLat;
        private string _tripLong;
        private string _triEndpLat;
        private string _tripEndLong;
        private string _tripStartTime;
        private string _tripEndTime;
        private string _tripDuration;

        public string TripLat { get { return _tripLat; } set { SetProperty(ref _tripLat, value); } }
        public string TripLong { get { return _tripLong; } set { SetProperty(ref _tripLong, value); } }
        public string TripEndLat { get { return _triEndpLat; } set { SetProperty(ref _triEndpLat, value); } }
        public string TripEndLong { get { return _tripEndLong; } set { SetProperty(ref _tripEndLong, value); } }
        public string TripStartTime { get { return _tripStartTime; } set { SetProperty(ref _tripStartTime, value); } }
        public string TripEndTime { get { return _tripEndTime; } set { SetProperty(ref _tripEndTime, value); } }
        public string TripDuration { get { return _tripDuration; } set { SetProperty(ref _tripDuration, value); } }
        public ICommand CloseCommand { private set; get; }
        public TripTrackerViewModel(int tripId)
        {
            CloseCommand = new Command(DoClose);

            Initialize(tripId);
        }
        public async Task Initialize(int tripId)
        {
            DbTripObject tripObj = await _tripTable.GetItemAsync(tripId);

            TripLat =  $"Lat: {tripObj.StartTripLatitude}";
            TripLong = $"Long: {tripObj.StartTripLongitude}"; 

            TripEndLat = $"Lat: {tripObj.EndTripLatitude}"; 
            TripEndLong = $"Long: {tripObj.EndTripLongitude}"; 

            TripStartTime = $"{tripObj.RecordedOn.ToString("MMM-dd-yyyy")} {tripObj.RecordedOn.ToString("hh:mm tt")}";
            TripEndTime = $"{tripObj.TripEndedOn.ToString("MMM-dd-yyyy")} {tripObj.TripEndedOn.ToString("hh:mm tt")}";

            long tickDiff = tripObj.TripEndedOn.Ticks - tripObj.RecordedOn.Ticks;

            TimeSpan duration = new TimeSpan(tickDiff);
            double minutes = duration.TotalMinutes;
            Console.WriteLine($"Trip Duration : {minutes}");

            TripDuration = $"{minutes.ToString("N2")} " + (minutes > 1 ? "minutes" : "minute");
        }
        private void DoClose(object obj)
        {
            Shell.Current.Navigation.PopModalAsync();
        }
    }
}
