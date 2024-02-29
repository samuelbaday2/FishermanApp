using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class LoginPage : ContentPage
{
	private LoginPageViewModel viewModel;
	public LoginPage(LoginPageViewModel loginPageViewModel)
	{
		InitializeComponent();
		BindingContext = viewModel = loginPageViewModel;

	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        
    }
}