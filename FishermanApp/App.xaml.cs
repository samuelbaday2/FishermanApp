using FishermanApp.Objects;
using FishermanApp.ViewModels;
using FishermanApp.Views.Pages;
using Newtonsoft.Json;

namespace FishermanApp;

public partial class App : Application
{
	public App(LoginPageViewModel loginPageViewModel)
	{
		InitializeComponent();

		if (Preferences.Get(Pref.LOGGED_USER, string.Empty) != string.Empty)
		{
			string json = Preferences.Get(Pref.LOGGED_USER, string.Empty);
            var returnValue = JsonConvert.DeserializeObject<List<RegistrationObject>>(json);

            UserDataObject.UserObject = returnValue[0];

            MainPage = new AppShell(loginPageViewModel);
        }
		else {
            MainPage = new LoginPage(loginPageViewModel);
        }
		
	}
}
