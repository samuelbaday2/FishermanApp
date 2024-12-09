using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using FishermanApp.Constants.LocalDatabase.Tables;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Platforms.Android
{
    [Service]
    public class LocForegroundService : Service
    {
        private string NOTIFICATION_CHANNEL_ID = "1000";
        private int NOTIFICATION_ID = 1;
        private string NOTIFICATION_CHANNEL_NAME = "notification";
        private CancellationTokenSource _cancelTokenSource;

        private async Task startForegroundService()
        {
            BackgroundWorker backgroundWorker = new();

            backgroundWorker.DoWork += new DoWorkEventHandler(
                async delegate (object sender, DoWorkEventArgs e) {
                    bool returnValue = true;
                    int counter = 1;
                    
                    await GPSTracker();
                                                     
                    e.Result = returnValue;
                }
            );

            backgroundWorker.RunWorkerCompleted += async (s, e) => {
                if (Preferences.Get("GpsTracking", 0) == 0)
                {
                    await Task.Delay(300000);

                }
                else if (Preferences.Get("GpsTracking", 0) == 1)
                {
                    await Task.Delay(600000);

                }
                else if (Preferences.Get("GpsTracking", 0) == 2)
                {
                    await Task.Delay(900000);

                }
                else if (Preferences.Get("GpsTracking", 0) == 3)
                {
                    await Task.Delay(60000);
                }
                backgroundWorker.RunWorkerAsync();
            };


            backgroundWorker.RunWorkerAsync();

            var notifcationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                createNotificationChannel(notifcationManager);
            }

            var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
            notification.SetAutoCancel(false);
            notification.SetOngoing(true);
            notification.SetSmallIcon(Resource.Mipmap.appicon);
            notification.SetContentTitle("Fisherman App");
            notification.SetContentText("Tracking Location");
            StartForeground(NOTIFICATION_ID, notification.Build());
        }
        public async Task GPSTracker()
        {
            try
            {
                var gps = await GetCurrentLocation();
                var TripLatitude = gps == null ? AppResources.GpsOff : gps.Latitude.ToString();
                var TripLongitude = gps == null ? AppResources.GpsOff : gps.Longitude.ToString();
                var tripList = await new TripTable().GetItemsAsync();
                DbTripObject tripObject = tripList.LastOrDefault();


                if (!tripObject.IsTripEnded)
                {
                    await new TrackingTable().SaveItemAsync(new DBTrackingTable
                    {
                        Lat = TripLatitude,
                        Long = TripLongitude,
                        RecordedOn = DateTime.Now,
                        TripId = tripObject.Id,
                    });
                }
                var count = await new TrackingTable().GetItemsAsync();
                Console.WriteLine($"GPS COUNT :  {count.Count()} {TripLatitude} {TripLongitude}");

                //GPSSaved = count.Count().ToString();
            }
            catch (Exception ee)
            {

            }
        }
        public async Task<Location> GetCurrentLocation()
        {
            try
            {
                Location location = new();
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(15));

                _cancelTokenSource = new CancellationTokenSource();
                location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);


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
   
            }
        }

        private void createNotificationChannel(NotificationManager notificationMnaManager)
        {
            var channel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME,
            NotificationImportance.Low);
            notificationMnaManager.CreateNotificationChannel(channel);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            startForegroundService();
            return StartCommandResult.Sticky;
        }
    }

}
