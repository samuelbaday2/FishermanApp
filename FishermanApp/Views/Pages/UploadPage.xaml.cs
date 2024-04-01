using FishermanApp.Services.ConnectivityService;
using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class UploadPage : ContentPage
{
	private readonly UploadPageViewModel _uploadPageViewModel;
	public UploadPage(UploadPageViewModel uploadPageViewModel, IConnectionHandlerService connectionHandlerService)
	{
		InitializeComponent();
		BindingContext = _uploadPageViewModel = new UploadPageViewModel(connectionHandlerService);

    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        _uploadPageViewModel.Initialize();
    }
}