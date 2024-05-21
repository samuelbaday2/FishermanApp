using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using FishermanApp.Services;
using FishermanApp.Services.AndroidEnvironmentService;
using FishermanApp.Services.AppUpdateService;
using FishermanApp.Services.ConnectivityService;
using FishermanApp.Services.GeoLocation;
using FishermanApp.Services.LocationService;
using FishermanApp.Services.PopupMessageService;
using FishermanApp.Services.SosFeature;
using FishermanApp.Services.Weather;
using FishermanApp.ViewModels;
using FishermanApp.ViewModels.Modals;
using FishermanApp.ViewModels.Selection;
using FishermanApp.Views.Modals;
using FishermanApp.Views.Pages;
using FishermanApp.Views.Selection;
#if ANDROID
using Maui.Android.InAppUpdates;
#endif
using Microsoft.Extensions.Logging;

namespace FishermanApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp(Action<MauiAppBuilder> registerPlatformSpecificServices)
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiMaps()
#if ANDROID
             .UseAndroidInAppUpdates()
#endif
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		
		registerPlatformSpecificServices(builder);

		builder.Services.AddTransient<IPermissionService, PermissionService>();
        builder.Services.AddTransient<IConnectionHandlerService, ConnectionHandlerService>();
        builder.Services.AddTransient<IUpdateService, UpdateService>();
        builder.Services.AddTransient<IAndroidEnvironmentService, AndroidEnvironmentService>();
        builder.Services.AddTransient<ISosService, SosService>();
        builder.Services.AddTransient<IDisplayAlertService, DisplayAlertService>();
        builder.Services.AddTransient<IWeatherDataService, WeatherDataService>();

        //Viewmodels
        builder.Services.AddSingleton<GearSelectionViewModel>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<TripPageViewModel>();
        builder.Services.AddSingleton<TripPageViewModel>();
        builder.Services.AddSingleton<EnterSetDetailPageViewModel>();
        builder.Services.AddSingleton<HookTypeSelectionViewModel>();
        builder.Services.AddSingleton<SpeciesSelectionViewModel>();
        builder.Services.AddSingleton<EnterCatchDetailsPageViewModel>();
        builder.Services.AddSingleton<TripHistoryPageViewModel>();
        builder.Services.AddSingleton<CatchSpeciesSelectionViewModel>();
        builder.Services.AddSingleton<UploadPageViewModel>();
        builder.Services.AddSingleton<SosPageViewModel>();
        builder.Services.AddSingleton<WeatherForecastPageViewModel>();

        //Views
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<TripPage>();
        builder.Services.AddSingleton<GearSelection>();
        builder.Services.AddSingleton<EnterSetDetailPage>();
        builder.Services.AddSingleton<HookTypeSelection>();
        builder.Services.AddSingleton<SpeciesSelectionViewModel>();
        builder.Services.AddSingleton<EnterCatchDetailsPage>();
        builder.Services.AddSingleton<TripHistoryPage>();
        builder.Services.AddSingleton<CatchSpeciesSelection>();
        builder.Services.AddSingleton<UploadPage>();
        builder.Services.AddSingleton<SosPage>();
        builder.Services.AddSingleton<MapPage>();
        builder.Services.AddSingleton<WeatherForecastPage>();
      

        //Modals
        builder.Services.AddSingleton<CatchSpeciesSelectionViewModel>();
        builder.Services.AddSingleton<CatchModal>();
        builder.Services.AddSingleton<RegisterVesselModalViewModel>();
        builder.Services.AddSingleton<RegisterVessel>();

        builder.Services.AddSingleton<EffortModalViewModel>();
        builder.Services.AddSingleton<EffortModal>();

        builder.Services.AddSingleton<TripTrackerViewModel>();
        builder.Services.AddSingleton<TripTracker>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
