using FishermanApp.Constants.LocalDatabase.Tables;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit;

namespace FishermanApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        private bool isBusy;
        private bool isBusyReversed = true;
        private string title = string.Empty;

        public readonly SemaphoreSlim sephamoreSlim = new SemaphoreSlim(1,1);

        public TripTable _tripTable;
        public TripSetTable _tripSetTable;
        public BaitSpeciesTable _baitSpeciesTable;
        public CatchTable _catchTable;
        public CatchSpeciesTable _catchSpeciesTable;
        public BaseViewModel()
        {
            _tripTable = new TripTable();
            _tripSetTable = new TripSetTable();
            _baitSpeciesTable = new BaitSpeciesTable();
            _catchTable = new CatchTable();
            _catchSpeciesTable = new CatchSpeciesTable();

            _catchSpeciesTable.AddSpeciesAsync();
            _baitSpeciesTable.AddSpeciesAsync();
        }
      
        public async Task<Location> GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                return location;
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                return null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public bool IsBusyReversed
        {
            get { return isBusyReversed; }
            set
            {
                isBusyReversed = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
      
     
    }
}
