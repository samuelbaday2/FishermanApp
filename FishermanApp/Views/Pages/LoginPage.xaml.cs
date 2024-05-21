using FishermanApp.Services.AppUpdateService;
using FishermanApp.ViewModels;
using FishermanApp.Views.Modals;

namespace FishermanApp.Views.Pages;

public partial class LoginPage : ContentPage
{
	private LoginPageViewModel _viewModel;
    private IUpdateService _updateService;

    public LoginPage(LoginPageViewModel loginPageViewModel, IUpdateService updateService)
	{
		InitializeComponent();
		BindingContext = _viewModel = loginPageViewModel;
        _updateService = updateService;

    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            //if(_updateService != null)
            //if (await _updateService.CheckIfFileExistsOnServerAsync())
            //{
            //    var result = await DisplayAlert("App Update", "A new version of the app is available, would you like to download and update?", "Download Update", "Cancel");

            //    if (result)
            //    {
            //        _viewModel.SetBusyStatusAsync(false);
            //        _updateService.DownloadApk(default);
            //        _updateService.DownloadPercentageChanged += downloadPercentChanged;
            //    }
            //}
        }
        catch { }
        
    }
    public void downloadPercentChanged(object? sender, int percent)
    {
        _viewModel.UpdatePercent($"{percent}%");
        if (percent == 100) 
        {
            _viewModel.UpdatePercent(string.Empty);
            _viewModel.SetBusyStatusAsync(true);
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (_viewModel == null) 
        {
            _viewModel = DependencyService.Get<LoginPageViewModel>();
        }
        await Navigation.PushModalAsync(new RegisterVessel(_updateService, _viewModel));
    }
}