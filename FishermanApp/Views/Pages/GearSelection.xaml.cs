using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class GearSelection : ContentPage
{
	GearSelectionViewModel viewModel;
	public GearSelection()
	{
		InitializeComponent();
		BindingContext = viewModel = new GearSelectionViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.InitializeAsync();
    }
}