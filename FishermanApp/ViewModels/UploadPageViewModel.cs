using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
using FishermanApp.Services.ConnectivityService;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class UploadPageViewModel : BaseViewModel
    {
        private readonly IConnectionHandlerService _connectionHandlerService;
        private string _pendingString;
        private bool _uploadEnabled;

        public string PendingString { get { return _pendingString; } set { SetProperty(ref _pendingString, value); } }
        public bool UploadEnabled { get { return _uploadEnabled; } set { SetProperty(ref _uploadEnabled, value); } }
        public ICommand UploadCommand { private set; get; }
        public UploadPageViewModel(IConnectionHandlerService connectionHandlerService) 
        {
            UploadCommand = new Command(DoUpload);
            _connectionHandlerService = connectionHandlerService;
        }
        public async Task Initialize()
        {
            UploadEnabled = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            var pendingTrips = await _tripTable.GetUnuploadedData();

            if (pendingTrips.Count > 0)
            {
                PendingString = $"You have {pendingTrips.Count} pending data";
            }
            else {
                PendingString = $"{AppResources.NoDataToUpload}";
                UploadEnabled = false;
            }
          
        }
        public async Task SetBusyStatusAsync(bool status)
        {
            UploadEnabled = !status;
            IsNotBusy = status;
        }
        private async void DoUpload(object obj)
        {
            SetBusyStatusAsync(false);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () => {
                string hostname = "http://" + AppResources.Hostname;
                var client2 = new RestClient(hostname);

                var username = "Samuel";
                var password = "Baday1234";
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                               .GetBytes(username + ":" + password));

                client2.AddDefaultHeader("Authorization", "Basic " + encoded);

                var pendingTrips = await _tripTable.GetUnuploadedData();

                if (pendingTrips.Count > 0)
                {
                    try
                    {
                        foreach (TripHistoryObject tripObject in pendingTrips)
                        {
                            var request2 = new RestRequest("nextt.asmx/{function}", Method.GET);
                            request2.AddParameter("VesselId", tripObject.VesselId);
                            request2.AddParameter("StartTripLatitude", tripObject.StartTripLatitude);
                            request2.AddParameter("StartTripLongitude", tripObject.StartTripLongitude);
                            request2.AddParameter("EndTripLatitude", tripObject.EndTripLatitude);
                            request2.AddParameter("EndTripLongitude", tripObject.EndTripLongitude);
                            request2.AddParameter("Captain", tripObject.Captain);
                            request2.AddParameter("IsTripEnded", tripObject.IsTripEnded);
                            request2.AddParameter("RecordedOn", tripObject.RecordedOn);
                            request2.AddParameter("TripEndedOn", tripObject.TripEndedOn);
                            request2.AddParameter("IsActive", tripObject.IsActive);
                            request2.AddParameter("EditedOn", DateTime.Now);

                            request2.AddUrlSegment("function", "InsertTripRecord");

                            IRestResponse response2 = client2.Execute(request2);
                            string resposeString = response2.Content;
                            string json = response2.Content;

                            var returnValue = JsonConvert.DeserializeObject<List<DbTripObject>>(json);

                            Console.WriteLine($"Cloud ID: {returnValue.FirstOrDefault().Id}");

                            tripObject.IsUploaded = true;
                            tripObject.UploadedId = returnValue.FirstOrDefault().Id;
                            _tripTable.SaveItemAsync(tripObject as DbTripObject);

                            //UPLOAD TRIP EFFORT CONNECTED TO TRIP UPLOADED
                            UploadTripEffort(tripObject.Id, tripObject.UploadedId, client2);

                        }

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Toast.Make(AppResources.UploadComplete, ToastDuration.Long).Show();
                            Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.Home)).FirstOrDefault();
                        });

                    }
                    catch (Exception ee)
                    {

                    }
                    finally
                    {
                        SetBusyStatusAsync(true);
                    }
                }
                else 
                {
                    SetBusyStatusAsync(true);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Toast.Make(AppResources.NoDataToUpload, ToastDuration.Long).Show();                      
                        Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.Home)).FirstOrDefault();
                    });                           
                }
                
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


        }

        private async Task UploadTripEffort(int tripId,int dbTripId, RestClient client2)
        {
            var tripSets = await _tripSetTable.GetItesmByTripIdAsync(tripId);

            foreach (DBSetObject setObject in tripSets)
            {
                var request2 = new RestRequest("nextt.asmx/{function}", Method.GET);
                request2.AddParameter("TripId", dbTripId);
                request2.AddParameter("StartSetLatitude", setObject.StartSetLatitude);
                request2.AddParameter("StartSetLongitude", setObject.StartSetLongitude);
                request2.AddParameter("EndSetLatitude", setObject.EndSetLatitude);
                request2.AddParameter("EndSetpLongitude", setObject.EndSetpLongitude);
                request2.AddParameter("RecordedOn", setObject.RecordedOn);
                request2.AddParameter("SetEndedOn", setObject.SetEndedOn);
                request2.AddParameter("SetEnded", setObject.SetEnded);
                request2.AddParameter("IsActive", setObject.IsActive);
                request2.AddParameter("LengthOfLongLine", setObject.LengthOfLongLine);
                request2.AddParameter("NumberOfBaskets", setObject.NumberOfBaskets);
                request2.AddParameter("HooksPerBasket", setObject.HooksPerBasket);
                request2.AddParameter("TypeOfHook", setObject.TypeOfHook);
                request2.AddParameter("GangionLength", setObject.GangionLength);
                request2.AddParameter("BaitType", setObject.BaitType);
                request2.AddParameter("BaitSpecie", setObject.BaitSpecie);
                request2.AddParameter("HasCatchData", setObject.HasCatchData);
                request2.AddParameter("EditedOn", DateTime.Now);

                request2.AddUrlSegment("function", "InsertTripEffortRecord");

                IRestResponse response2 = client2.Execute(request2);
                string resposeString = response2.Content;
                string json = response2.Content;

                var returnValue = JsonConvert.DeserializeObject<List<DBSetObject>>(json);

                setObject.IsUploaded = true;
                setObject.UploadedId = returnValue.FirstOrDefault().Id;
                await _tripSetTable.SaveItemAsync(setObject);

                await UploadCatch(setObject.Id, setObject.UploadedId, dbTripId, client2);
            }
        }

        private async Task UploadCatch(int setId, int dbSetId, int dbTripId, RestClient client2)
        {
            var catchData = await _catchTable.GetItemsBySetIdAsync(setId);

            foreach (DBCatchObject catchObject in catchData)
            {
                var request2 = new RestRequest("nextt.asmx/{function}", Method.GET);
                request2.AddParameter("SetId", dbSetId);
                request2.AddParameter("TripId", dbTripId);
                request2.AddParameter("IsActive", catchObject.IsActive);
                request2.AddParameter("Species", catchObject.Species);
                request2.AddParameter("ScientificName", catchObject.ScientificName);
                request2.AddParameter("Quantity", catchObject.Quantity);
                request2.AddParameter("RecordedOn", catchObject.RecordedOn);
                request2.AddParameter("EditedOn", DateTime.Now);

                request2.AddUrlSegment("function", "InsertCatchRecord");

                IRestResponse response2 = client2.Execute(request2);
                string resposeString = response2.Content;
                string json = response2.Content;

                var returnValue = JsonConvert.DeserializeObject<List<DBSetObject>>(json);

                catchObject.IsUploaded = true;
                catchObject.UploadedId = returnValue.FirstOrDefault().Id;
                await _catchTable.SaveItemAsync(catchObject);
            }
        }
    }
}
