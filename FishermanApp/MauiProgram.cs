using FishermanApp.Services;
using FishermanApp.Services.GeoLocation;
using FishermanApp.ViewModels;
using FishermanApp.Views.Pages;
using Microsoft.Extensions.Logging;

namespace FishermanApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<IPermissionService,PermissionService>();

		builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoginPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
