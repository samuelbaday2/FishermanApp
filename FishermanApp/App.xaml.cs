using FishermanApp.Objects;
using FishermanApp.Services.AppUpdateService;
using FishermanApp.ViewModels;
using FishermanApp.Views.Pages;
using Newtonsoft.Json;

namespace FishermanApp;

public partial class App : Application
{
	public App(LoginPageViewModel loginPageViewModel, IUpdateService updateService)
	{
		InitializeComponent();

		if (Preferences.Get(Pref.LOGGED_USER, string.Empty) != string.Empty)
		{
			string json = Preferences.Get(Pref.LOGGED_USER, string.Empty);
            var returnValue = JsonConvert.DeserializeObject<List<RegistrationObject>>(json);

            UserDataObject.UserObject = returnValue[0];

            MainPage = new AppShell(loginPageViewModel, updateService);
        }
		else {
            MainPage = new NavigationPage(new LoginPage(loginPageViewModel, updateService));
        }
        RequestPermissionAsync();
    }


    public async Task RequestPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.StorageWrite>();
        }

        status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.StorageRead>();
        }
    }
}
