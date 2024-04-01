using FishermanApp.Services.AppUpdateService;
using FishermanApp.ViewModels;

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

        if (await _updateService.CheckIfFileExistsOnServerAsync())
        {
            var result = await DisplayAlert("App Update", "A new version of the app is available, would you like to download and update?", "Download Update", "Cancel");

            if (result)
            {
                _viewModel.SetBusyStatusAsync(false);
                _updateService.DownloadApk(default);
                _updateService.DownloadPercentageChanged += downloadPercentChanged;
            }
        }
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
}

