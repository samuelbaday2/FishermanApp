using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.Runtime;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using FishermanApp.Services.LocationService;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FishermanApp
{
    public class LocationFeatureService : ILocationFeatureService
    {
        public bool IsConnected { get; set; }

        public event EventHandler<bool> ConnectionStatusChanged;

        public Task StartConnectionListenerAsync()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);

            IsConnected = locationManager.IsProviderEnabled(LocationManager.GpsProvider);


            return Task.CompletedTask;
        }

        public Task InstallApk(string path) 
        {
            try
            {
                if (Preferences.Get("package_install", false))
                {
                    Toast.Make($"Launching Installer", ToastDuration.Short).Show();
                    Platform.CurrentActivity.StartActivity(new Android.Content.Intent(
                        Android.Provider.Settings.ActionManageUnknownAppSources,
                        Android.Net.Uri.Parse("package:" + path)));
                }
                else
                {
                    Toast.Make($"launching installer", ToastDuration.Short).Show();

#if ANDROID
                    var context = Android.App.Application.Context;
                    Java.IO.File apkFile = new Java.IO.File(path);
                    Intent intent = new Intent(Intent.ActionView);
                    var uri = Microsoft.Maui.Storage.FileProvider.GetUriForFile(context, "com.insitesolutions.fishermanapp" + ".fileprovider", apkFile);

                    intent.SetDataAndType(uri, "application/vnd.android.package-archive");

                    intent.AddFlags(ActivityFlags.NewTask);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    intent.AddFlags(ActivityFlags.ClearTop);
                    intent.PutExtra(Intent.ExtraNotUnknownSource, true);

                    Platform.CurrentActivity.StartActivityForResult(intent, 1);
#endif
                    Toast.Make($"opening launcher", ToastDuration.Short).Show();
                }
            }
            catch (Exception ee)
            {
                Toast.Make($"{ee.Message}", ToastDuration.Long).Show();
            }
           
            
            return Task.CompletedTask;
        }


    }
}
