using FishermanApp.ViewModels.Selection;

namespace FishermanApp.Views.Selection;

public partial class HookTypeSelection : ContentPage
{
    private HookTypeSelectionViewModel viewModel;
	public HookTypeSelection()
	{
		InitializeComponent();
		BindingContext = viewModel = new HookTypeSelectionViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.InitializeAsync();
    }
}