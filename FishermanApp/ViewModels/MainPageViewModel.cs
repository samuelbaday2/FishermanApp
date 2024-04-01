using FishermanApp.Constants.LocalDatabase.Tables;
using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
using FishermanApp.Services;
using FishermanApp.Services.ConnectivityService;
using FishermanApp.Services.LocationService;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        EnterSetDetailPageViewModel _enterSetDetailPageViewModel;

        IPermissionService _permissionService;
        IConnectionHandlerService _connectionHandlerService;
        ILocationFeatureService _locationFeatureService;

        private string _vesselName;
        private string _homePort;
        private string _captainName;
        private string _gpsStatus;
        private string _connectionStatus;
        private bool _hasPendingTrip;

        public string VesselName { get { return _vesselName; } set { SetProperty(ref _vesselName, value); } }
        public string HomePort { get { return _homePort; } set { SetProperty(ref _homePort, value); } }
        public string CaptainName { get { return _captainName; } set { SetProperty(ref _captainName, value); } }
        public string GpsStatus { get { return _gpsStatus; } set { SetProperty(ref _gpsStatus, value); } }
        public string ConnectionStatus { get { return _connectionStatus; } set { SetProperty(ref _connectionStatus, value); } }
        public bool HasPendingTrip { get { return _hasPendingTrip; } set { SetProperty(ref _hasPendingTrip, value); } }

        public ICommand StartTripCommand { private set; get; }
        public ICommand EndTripCommand { private set; get; }
        public ICommand StartSetCommand { private set; get; }

        public MainPageViewModel(IPermissionService permissionService, IConnectionHandlerService connectionHandlerService, ILocationFeatureService locationFeatureService, EnterSetDetailPageViewModel enterSetDetailPageViewModel) {
            StartTripCommand = new Command(DoStartTrip);
            EndTripCommand = new Command(DoEndTrip);
            StartSetCommand = new Command(DoStartSet);

            VesselName = UserDataObject.UserObject.VesName;
            HomePort = UserDataObject.UserObject.VesHomePort;
            CaptainName = $"{UserDataObject.UserObject.CapFName} {UserDataObject.UserObject.CapLName}";

            if (CaptainName.Trim().Equals(string.Empty))
            {
                CaptainName = $"{UserDataObject.UserObject.CaptainName}";
            }

            _permissionService = permissionService;
            _connectionHandlerService = connectionHandlerService;

            _connectionHandlerService.ConnectionStatusChanged += connectionStatusChanged;
            _connectionHandlerService.StartConnectionListenerAsync();

            _locationFeatureService = locationFeatureService;
            _locationFeatureService.StartConnectionListenerAsync();
          
            _enterSetDetailPageViewModel = enterSetDetailPageViewModel;
            GetGpsStatusAsync();
        }
        public async Task UpdatePercent(string percent)
        {
            try
            {
                Percentage = percent;
            }
            catch { }
        }
        public async Task InitializeAsync() 
        {
            try
            {
                var tripList = await _tripTable.GetItemsAsync();

                HasPendingTrip = !tripList.LastOrDefault().IsTripEnded;
            }
            catch { }

            try
            {
                var lastTripData = await _tripTable.GetItemsAsync();
                var existingSets = await _tripSetTable.GetItemsAsync();

                int currentSetCount = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).Count();

                bool hasSetEnded = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).LastOrDefault().SetEnded;

                if (currentSetCount > 0 && !hasSetEnded)
                {
                    try
                    {
                        await _enterSetDetailPageViewModel.AddLastSetAsync();
                    }
                    catch { }
                    Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.SetDetails)).FirstOrDefault();
                    return;
                }

                bool hasCatchData = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).LastOrDefault().HasCatchData;
                if (currentSetCount > 0 && hasSetEnded && !hasCatchData)
                {
                    try
                    {
                        await _enterSetDetailPageViewModel.AddLastSetAsync();
                    }
                    catch { }
                    InitializeConfig.InitializeFunction = true;
                    Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();
                }
            }
            catch (Exception ee)
            {
            
            }
        }
        public void connectionStatusChanged(object? sender,bool isConnected) 
        {
            ConnectionStatus = isConnected ? AppResources.Online : AppResources.Offline;
        }

        public async Task GetGpsStatusAsync()
        {
            await _locationFeatureService.StartConnectionListenerAsync();
            GpsStatus = (_locationFeatureService.IsConnected ? AppResources.Online : AppResources.Offline);
            await Task.Delay(10000);
            GetGpsStatusAsync();
        }
        private async void DoStartTrip(object obj)
        {
            SetBusyStatusAsync(false);
            if(sephamoreSlim.CurrentCount == 0)
            {
                return;
            }

            try
            {
                sephamoreSlim.Wait();
                var gps = await GetCurrentLocation();

                await _tripTable.SaveItemAsync(new DbTripObject
                {
                    RecordedOn = DateTime.Now,
                    VesselId = int.Parse(UserDataObject.UserObject.VesId),
                    StartTripLatitude = gps == null? AppResources.GpsOff : gps.Latitude.ToString(),
                    StartTripLongitude = gps == null ? AppResources.GpsOff : gps.Longitude.ToString(),
                    IsTripEnded = false,
                    IsActive = true,
                    Captain = CaptainName,
                });

                var tripList = await _tripTable.GetItemsAsync();

                HasPendingTrip = !tripList.LastOrDefault().IsTripEnded;
             
            }
            catch(Exception ee) 
            {
                Console.WriteLine(ee.Message);
              
            }
            finally {
                sephamoreSlim.Release();
                SetBusyStatusAsync(true);
            }
           
        }

        private async void DoEndTrip(object obj)
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

                var tripList = await _tripTable.GetItemsAsync();

                DbTripObject tripObject = tripList.LastOrDefault();

                await _tripTable.SaveItemAsync(new DbTripObject
                {
                    Id = tripObject.Id,
                    RecordedOn = tripObject.RecordedOn,
                    VesselId = tripObject.VesselId,
                    StartTripLatitude = tripObject.StartTripLatitude,
                    StartTripLongitude = tripObject.StartTripLongitude,
                    EndTripLatitude = gps == null ? AppResources.GpsOff : gps.Latitude.ToString(),
                    EndTripLongitude = gps == null ? AppResources.GpsOff : gps.Longitude.ToString(),
                    IsTripEnded = true,
                    TripEndedOn = DateTime.Now,
                    IsActive = tripObject.IsActive,
                    Captain = tripObject.Captain,
                });

                await InitializeAsync();
            }
            catch(Exception ee) 
            {
                Console.WriteLine(ee.Message);
               
            }
            finally {
                sephamoreSlim.Release();
                SetBusyStatusAsync(true);
            }
        }

        private async void DoStartSet(object obj)
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

                var tripList = await _tripTable.GetItemsAsync();

                await _tripSetTable.SaveItemAsync(new DBSetObject
                {
                    TripId = tripList.LastOrDefault().Id,
                    RecordedOn = DateTime.Now,

                    StartSetLatitude = gps == null ? AppResources.GpsOff : gps.Latitude.ToString(),
                    StartSetLongitude = gps == null ? AppResources.GpsOff : gps.Longitude.ToString(),
                    
                    IsActive = true,
                });

                try
                {
                    _enterSetDetailPageViewModel.AddLastSetAsync();
                }
                catch { }
                
                Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.SetDetails)).FirstOrDefault();
                
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
    }
}
