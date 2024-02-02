using FishermanApp.ViewModels;
using FishermanApp.Views.Pages;

namespace FishermanApp;

public partial class App : Application
{
	public App(LoginPageViewModel loginPageViewModel)
	{
		InitializeComponent();

		MainPage = new LoginPage(loginPageViewModel);
	}
}
