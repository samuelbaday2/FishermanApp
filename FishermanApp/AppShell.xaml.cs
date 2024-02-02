using FishermanApp.ViewModels;
using FishermanApp.Views.Pages;

namespace FishermanApp;

public partial class AppShell : Shell
{
    private LoginPageViewModel viewModel;
	public AppShell(LoginPageViewModel loginPageViewModel)
	{
		InitializeComponent();
        viewModel = loginPageViewModel;
	}

    private async void Shell_Navigating(object sender, ShellNavigatingEventArgs e)
    {
		
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Application.Current.MainPage = new LoginPage(viewModel);
    }
}
