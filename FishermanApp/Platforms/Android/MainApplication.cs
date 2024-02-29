using Android.App;
using Android.Runtime;
using AndroidX.AppCompat.App;
using FishermanApp.Services.LocationService;

namespace FishermanApp;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
        //Forced Light Theme
        AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
    }

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp(RegisterServices);

    private static void RegisterServices(MauiAppBuilder builder)
    {
        builder.Services.AddScoped<ILocationFeatureService, LocationFeatureService>();
    }
}
