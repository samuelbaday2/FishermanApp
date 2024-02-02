using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace FishermanApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public const int REQUEST_CHECK_SETTINGS = 0x1;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        //DisplayLocationSettingsRequest();
    }

    //private void DisplayLocationSettingsRequest()
    //{
    //    var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API).Build();
    //    googleApiClient.Connect();

    //    StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
    //}

}
