using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using AndroidX.AppCompat.App;
using FishermanApp.Resources.Localization;

namespace FishermanApp;

[Activity(Theme = "@style/Maui.SplashTheme", LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public const int REQUEST_CHECK_SETTINGS = 0x1;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        //if (!Android.OS.Environment.IsExternalStorageManager)
        //{
        //    Intent intent = new Intent();
        //    intent.SetAction(Android.Provider.Settings.ActionManageAppAllFilesAccessPermission);
        //    Android.Net.Uri uri = Android.Net.Uri.FromParts("package", this.PackageName, null);
        //    intent.SetData(uri);
        //    StartActivity(intent);
        //}

        //Preferences.Set("package_install", PackageManager.CanRequestPackageInstalls());
         

        DisplayLocationSettingsRequest();
    }
    protected override void OnNewIntent(Intent intent)
    {      
        Bundle extras = intent.Extras;
        if (1.Equals(intent.Action))
        {
            var status = extras.GetInt(PackageInstaller.ExtraStatus);
            var message = extras.GetString(PackageInstaller.ExtraStatusMessage);
            switch (status)
            {
                case (int)PackageInstallStatus.PendingUserAction:
                    // Ask user to confirm the installation
                    var confirmIntent = (Intent)extras.Get(Intent.ExtraIntent);
                    StartActivity(confirmIntent);
                    break;
                case (int)PackageInstallStatus.Success:
                    //TODO: Handle success
                    break;
                case (int)PackageInstallStatus.Failure:
                case (int)PackageInstallStatus.FailureAborted:
                case (int)PackageInstallStatus.FailureBlocked:
                case (int)PackageInstallStatus.FailureConflict:
                case (int)PackageInstallStatus.FailureIncompatible:
                case (int)PackageInstallStatus.FailureInvalid:
                case (int)PackageInstallStatus.FailureStorage:
                    //TODO: Handle failures
                    break;
            }
        }
    }

    private void DisplayLocationSettingsRequest()
    {
        var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API).Build();
        googleApiClient.Connect();

        var locationRequest = Android.Gms.Location.LocationRequest.Create();
        locationRequest.SetPriority(Android.Gms.Location.LocationRequest.PriorityHighAccuracy);
        locationRequest.SetInterval(10000);
        locationRequest.SetFastestInterval(10000 / 2);

        var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
        builder.SetAlwaysShow(true);

        var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
        result.SetResultCallback((LocationSettingsResult callback) =>
        {
            switch (callback.Status.StatusCode)
            {
                case LocationSettingsStatusCodes.Success:
                    {
                        //DoStuffWithLocation();
                        break;
                    }
                case LocationSettingsStatusCodes.ResolutionRequired:
                    {
                        try
                        {
                            // Show the dialog by calling startResolutionForResult(), and check the result
                            // in onActivityResult().
                            callback.Status.StartResolutionForResult(this, REQUEST_CHECK_SETTINGS);
                        }
                        catch (IntentSender.SendIntentException e)
                        {
                        }

                        break;
                    }
                default:
                    {
                        // If all else fails, take the user to the android location settings
                        StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                        break;
                    }
            }
        });
    }

}
