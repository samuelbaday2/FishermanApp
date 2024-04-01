using FishermanApp.Objects;
using FishermanApp.Services.AppUpdateService;
using FishermanApp.ViewModels;
using FishermanApp.Views.Pages;

namespace FishermanApp;

public partial class AppShell : Shell
{
    private LoginPageViewModel viewModel;
    private readonly IUpdateService _updateService;
	public AppShell(LoginPageViewModel loginPageViewModel, IUpdateService updateService)
	{
		InitializeComponent();
        viewModel = loginPageViewModel;
        _updateService = updateService;

    }

    private async void Shell_Navigating(object sender, ShellNavigatingEventArgs e)
    {
		
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Preferences.Set(Pref.LOGGED_USER, string.Empty);
        Preferences.Set(Pref.LOGGED_USER_PASSWORD, string.Empty);
        Application.Current.MainPage = new LoginPage(viewModel, _updateService);
    }
}
