using CommunityToolkit.Mvvm.Messaging;
using FishermanApp.Objects;
using FishermanApp.Services.AppUpdateService;
using FishermanApp.ViewModels;
using FishermanApp.Views.Pages;
using Microsoft.Maui.ApplicationModel.Communication;

namespace FishermanApp;

public partial class MainPage : ContentPage
{
	int count = 0;
	private MainPageViewModel _viewModel;
    private IUpdateService _updateService;

    public MainPage(MainPageViewModel mainPageViewModel, IUpdateService updateService)
	{
		InitializeComponent();
		BindingContext = mainPageViewModel;
        _viewModel = mainPageViewModel;
        _updateService = updateService;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();

        if (Preferences.Get(ConfigClass.PERMISSION_LOC, true)) 
        {
            bool answer = await DisplayAlert("Location Permission", "Fisherman app collects location data to enable tracking even if the app is on background to get fishing data to determine where fishermen get more fish", "Allow", "Decline");

            if (answer) 
            {
                WeakReferenceMessenger.Default.Send(ConfigClass.PERMISSION_LOC);
            }
           
        }
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
    public void downloadPercentChanged(object? sender, int percent)
    {
        _viewModel.UpdatePercent($"{percent}%");
        if (percent == 100)
        {
            _viewModel.UpdatePercent(string.Empty);
            _viewModel.SetBusyStatusAsync(true);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new ManageCrewPage());
    }
}

