using Foundation;

namespace FishermanApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp(RegisterServices);

    private static void RegisterServices(MauiAppBuilder builder)
    {
        //builder.Services.AddScoped<ILocationFeatureService, LocationFeatureService>();
    }
}
