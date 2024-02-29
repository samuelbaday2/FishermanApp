using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class EnterCatchDetailsPage : ContentPage
{
	private EnterCatchDetailsPageViewModel viewModel;
	public EnterCatchDetailsPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new EnterCatchDetailsPageViewModel();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.Initialize();
    }
}