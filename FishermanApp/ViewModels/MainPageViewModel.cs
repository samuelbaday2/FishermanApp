﻿
using FishermanApp.Constants.LocalDatabase.Tables;
using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
using FishermanApp.Services;
using FishermanApp.Services.ConnectivityService;
using FishermanApp.Services.LocationService;
using FishermanApp.Services.PopupMessageService;
using FishermanApp.Views.Modals;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using FishermanApp.Views.Selection;
using FishermanApp.Views.Pages;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

#if ANDROID

using Android.OS;
using Android.Content;
using AndroidX.Arch.Core;
using AndroidX.Annotations;
#endif

namespace FishermanApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        EnterSetDetailPageViewModel _enterSetDetailPageViewModel;

        private IDisplayAlertService _displayAlertService;
        private IConnectionHandlerService _connectionHandlerService;
        private ILocationFeatureService _locationFeatureService;

        private string _vesselName;
        private string _homePort;
        private string _captainName;
        private string _gpsStatus;
        private string _connectionStatus;
        private bool _hasPendingTrip;
        private bool _isAddCatchVisible;
        private bool _isLastSetEnded;
        private string _fuelCost;
        private string _foodCost;
        private string _fuelAmount;
        private bool _isGPSTrackingOn;
        private Task _gpsTask;
        private string _gPSSaved;
        private string _country;
      

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public string GPSSaved { get { return _gPSSaved; } set { SetProperty(ref _gPSSaved, value); } }
        public string FuelCost { get { return _fuelCost; } set { SetProperty(ref _fuelCost, value); } }
        public string FoodCost { get { return _foodCost; } set { SetProperty(ref _foodCost, value); } }
        public string FuelAmount { get { return _fuelAmount; } set { SetProperty(ref _fuelAmount, value); } }
        public string VesselName { get { return _vesselName; } set { SetProperty(ref _vesselName, value); } }
        public string HomePort { get { return _homePort; } set { SetProperty(ref _homePort, value); } }
        public string Country { get { return _country; } set { SetProperty(ref _country, value); } }
        public string CaptainName { get { return _captainName; } set { SetProperty(ref _captainName, value); } }
        public string GpsStatus { get { return _gpsStatus; } set { SetProperty(ref _gpsStatus, value); } }
        public string ConnectionStatus { get { return _connectionStatus; } set { SetProperty(ref _connectionStatus, value); } }
        public bool HasPendingTrip { get { return _hasPendingTrip; } set { SetProperty(ref _hasPendingTrip, value); } }
        public bool IsAddCatchVisible { get { return _isAddCatchVisible; } set { SetProperty(ref _isAddCatchVisible, value); } }
        public bool IsLastSetNotEnded { get { return _isLastSetEnded; } set { SetProperty(ref _isLastSetEnded, value); } }

        public bool _isReleaseVisible;
        public bool IsReleaseVisible { get { return _isReleaseVisible; } set { SetProperty(ref _isReleaseVisible, value); } }
        public ICommand StartTripCommand { private set; get; }
        public ICommand EndTripCommand { private set; get; }
        public ICommand StartSetCommand { private set; get; }
        public ICommand AddCatchCommand { private set; get; }
        public ICommand ReleaseCommand { private set; get; }

        public MainPageViewModel(IDisplayAlertService displayAlertService, IConnectionHandlerService connectionHandlerService, ILocationFeatureService locationFeatureService, EnterSetDetailPageViewModel enterSetDetailPageViewModel) : base()
        {
            StartTripCommand = new Command(DoStartTrip);
            EndTripCommand = new Command(DoEndTrip);
            StartSetCommand = new Command(DoStartSet);
            AddCatchCommand = new Command(DoAddMoreCatch);
            ReleaseCommand =  new Command(DoRelease);

            _displayAlertService = displayAlertService;
            _connectionHandlerService = connectionHandlerService;

            _connectionHandlerService.ConnectionStatusChanged += connectionStatusChanged;
            _connectionHandlerService.StartConnectionListenerAsync();

            _locationFeatureService = locationFeatureService;
            _locationFeatureService.StartConnectionListenerAsync();

            _enterSetDetailPageViewModel = enterSetDetailPageViewModel;
            GetGpsStatusAsync();


            GetGpsCount();
        }
        public async Task GetGpsCount() {
            var count = await _trackingTable.GetItemsAsync();
            GPSSaved = count.Count().ToString();
        }
      
        private async void DoAddMoreCatch(object obj)
        {
            InitializeConfig.InitializeFunction = true;

            Shell.Current.Navigation.PushModalAsync(new SetSelect());

            //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();
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
            _tripTable.RunQuery();
            _isGPSTrackingOn = false;
            try
            {
                VesselName = UserDataObject.UserObject.VesName;
                HomePort = UserDataObject.UserObject.VesHomePort;
                CaptainName = $"{UserDataObject.UserObject.CapFName} {UserDataObject.UserObject.CapLName}";


                if (CaptainName.Trim().Equals(string.Empty))
                {
                    CaptainName = $"{UserDataObject.UserObject.CaptainName}";
                }
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
                    IsLastSetNotEnded = true;
                    //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.SetDetails)).FirstOrDefault();
                    return;
                }
                else 
                {
                    IsLastSetNotEnded = false;
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
                    //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();
                }

                if (!lastTripData.LastOrDefault().IsTripEnded )
                {
                    IsAddCatchVisible = true;
                    IsReleaseVisible = await _catchTable.GetEtpItemsAsync(lastTripData.LastOrDefault().Id);
                }
                else {
                    IsAddCatchVisible = false;
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

        public async Task GPSTracker() 
        {
            if (HasPendingTrip)
            {
                try
                {
                    var gps = await GetCurrentLocation();
                    var TripLatitude = gps == null ? AppResources.GpsOff : gps.Latitude.ToString();
                    var TripLongitude = gps == null ? AppResources.GpsOff : gps.Longitude.ToString();
                    var tripList = await _tripTable.GetItemsAsync();
                    DbTripObject tripObject = tripList.LastOrDefault();
                    await _trackingTable.SaveItemAsync(new DBTrackingTable
                    {
                        Lat = TripLatitude,
                        Long = TripLongitude,
                        RecordedOn = DateTime.Now,
                        TripId = tripObject.Id,
                    });

                    var count = await _trackingTable.GetItemsAsync();
                    Console.WriteLine($"GPS COUNT :  {count.Count()} {TripLatitude} {TripLongitude}");

                    GPSSaved = count.Count().ToString();
                }
                catch(Exception ee) 
                { 
                
                }
             
            }
        }
        private async void DoStartTrip(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new CardHomePage(this));          
            Shell.Current.Navigation.PushModalAsync(new HACCP());
            SetBusyStatusAsync(false);
            if(sephamoreSlim.CurrentCount == 0)
            {
                return;
            }

            try
            {
                sephamoreSlim.Wait();
                var gps = await GetCurrentLocation();
                var crew = await _crewTable.GetItemsAsync();
                await _tripTable.SaveItemAsync(new DbTripObject
                {
                    RecordedOn = DateTime.Now,
                    VesselId = int.Parse(UserDataObject.UserObject.VesId),
                    StartTripLatitude = gps == null? AppResources.GpsOff : gps.Latitude.ToString(),
                    StartTripLongitude = gps == null ? AppResources.GpsOff : gps.Longitude.ToString(),
                    IsTripEnded = false,
                    IsActive = true,
                    TripStartedOn = DateTime.Now,
                    Captain = CaptainName,
                    CrewNumber = crew.Where(x => x.IsChecked).Count().ToString(),
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
            var result = await _displayAlertService.DisplayALertAsync("End Trip", "Are you sure you want to end the current trip?");

            if (result) 
            {
                SetBusyStatusAsync(false);
               

                try
                {
                    //sephamoreSlim.Wait();
                    var gps = await GetCurrentLocation();
                    var crew = await _crewTable.GetItemsAsync();

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
                        FuelAmount = FuelAmount,
                        FoodCost = FoodCost,
                        FuelCost = FuelCost,
                        CrewNumber = crew.Where(x => x.IsChecked).Count().ToString(),
                        TripStartedOn = tripObject.TripStartedOn,
                        HomePort = Preferences.Get("HomePortEntry",string.Empty),
                        Country = Preferences.Get("CountryEntry", string.Empty),
                    });

                    try
                    {
                        var setList = await _tripSetTable.GetItemsAsync();
                        DBSetObject lastSetObject = setList.LastOrDefault();
                        lastSetObject.SetEnded = true;
                        await _tripSetTable.SaveItemAsync(lastSetObject);
                    }
                    catch { }

                    try
                    {
                        var availableCrew = await _crewTable.GetAvailableCrew();

                        foreach (DBCrewObject availCrew in availableCrew) 
                        {
                            await _tripCrewTable.SaveItemAsync(new DBTripCrewObject
                            {
                                TripId = tripObject.Id,
                                CrewFirstname = availCrew.Firstname,
                                CrewLastname = availCrew.Lastname,
                                RecordedOn = DateTime.Now,
                            });
                        }
                    }
                    catch { }

                    await Shell.Current.Navigation.PushModalAsync(new CatchModal(tripObject.Id));
                    await InitializeAsync();

                    

                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);

                }
                finally
                {
                    //sephamoreSlim.Release();
                    SetBusyStatusAsync(true);
                }
            }
           
        }

        private async void DoStartSet(object obj)
        {
        
            if (IsLastSetNotEnded)
            {
                SetBusyStatusAsync(false);
                

                try
                {
                    var lastTripData = await _tripTable.GetItemsAsync();
                    var existingSets = await _tripSetTable.GetItemsAsync();
                    DBSetObject currentSet = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).LastOrDefault();

                   

                    currentSet.SetEnded = true;
                    
                    currentSet.SetEndedOn = DateTime.Now;
                  
                    await _tripSetTable.SaveItemAsync(currentSet);
                    IsLastSetNotEnded = false;
                    InitializeAsync();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
                finally 
                {
                   // sephamoreSlim.Release();
                    SetBusyStatusAsync(true);
                }
            }
            else {
                SetBusyStatusAsync(false);
               

                try
                {
                    //sephamoreSlim.Wait();
                    var gps = await GetCurrentLocation();

                    var tripList = await _tripTable.GetItemsAsync();

                    await _tripSetTable.SaveItemAsync(new DBSetObject
                    {
                        TripId = tripList.LastOrDefault().Id,
                        RecordedOn = DateTime.Now,
                        SetStartedOn = DateTime.Now,
                        StartSetLatitude = gps == null ? AppResources.GpsOff : gps.Latitude.ToString(),
                        StartSetLongitude = gps == null ? AppResources.GpsOff : gps.Longitude.ToString(),

                        IsActive = true,
                    });

                    try
                    {
                        _enterSetDetailPageViewModel.AddLastSetAsync();
                    }
                    catch { }

                    //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.SetDetails)).FirstOrDefault();
                    await Shell.Current.Navigation.PushAsync(new EnterSetDetailPage(new()));
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);

                }
                finally
                {
                    //sephamoreSlim.Release();
                    SetBusyStatusAsync(true);
                }
            }

            
        }

        private async void DoRelease(object obj)
        {
            try
            {
                var lastTripData = await _tripTable.GetItemsAsync();

                var list = await _catchTable.GetEtpItemsListAsync(lastTripData.LastOrDefault().Id);

                foreach (var item in list)
                {
                    item.IsReleased = true;
                    item.ReleaseTransactionDateTime = DateTime.Now;
                    await _catchTable.SaveItemAsync(item);
                }

                list = await _catchTable.GetEtpItemsListAsync(lastTripData.LastOrDefault().Id);

                IsReleaseVisible = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    Toast.Make("Records Updated to ETP Released", ToastDuration.Long).Show();
                  
                });
            }
            catch 
            {

            }
        }
    }
}
