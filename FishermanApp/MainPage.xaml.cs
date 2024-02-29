using FishermanApp.ViewModels;

namespace FishermanApp;

public partial class MainPage : ContentPage
{
	int count = 0;
	MainPageViewModel viewModel;

	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = mainPageViewModel;
        viewModel = mainPageViewModel;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
    }


}

