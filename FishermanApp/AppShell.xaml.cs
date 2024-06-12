using FishermanApp.Objects;
using FishermanApp.Services.AppUpdateService;
using FishermanApp.ViewModels;
using FishermanApp.Views.Modals;
using FishermanApp.Views.Pages;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FishermanApp;

public partial class AppShell : Shell
{
    private LoginPageViewModel viewModel;
    private IUpdateService _updateService;
	public AppShell(LoginPageViewModel loginPageViewModel, IUpdateService updateService)
	{
		InitializeComponent();
        viewModel = loginPageViewModel;
        _updateService = updateService;

        Routing.RegisterRoute(nameof(CustomSpecie), typeof(CustomSpecie));
    }

    private async void Shell_Navigating(object sender, ShellNavigatingEventArgs e)
    {
		
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            Preferences.Set(Pref.LOGGED_USER, string.Empty);
            Preferences.Set(Pref.LOGGED_USER_PASSWORD, string.Empty);

            Preferences.Clear();         
        }
        catch
        {

        }
        finally 
        {
            if(_updateService == null) 
            {
                _updateService = DependencyService.Get<IUpdateService>();
            }

            Application.Current.MainPage = new NavigationPage(new LoginPage(viewModel, _updateService));
        }
       
    }
}
