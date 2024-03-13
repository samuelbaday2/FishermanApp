using FishermanApp.ViewModels.Selection;

namespace FishermanApp.Views.Selection;

public partial class CatchSpeciesSelection : ContentPage
{
	private CatchSpeciesSelectionViewModel _viewModel;
	public CatchSpeciesSelection(CatchSpeciesSelectionViewModel catchSpeciesSelectionViewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = catchSpeciesSelectionViewModel;
	}
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        Thread.Sleep(500);
        _viewModel.InitializeAsync();
    }
}