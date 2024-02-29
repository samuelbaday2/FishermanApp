using FishermanApp.ViewModels.Selection;

namespace FishermanApp.Views.Selection;

public partial class SpeciesSelection : ContentPage
{
    private SpeciesSelectionViewModel viewModel;
    public SpeciesSelection()
	{
		InitializeComponent();
        BindingContext = viewModel = new SpeciesSelectionViewModel();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        Thread.Sleep(500);
        viewModel.InitializeAsync();
    }
}