using FishermanApp.ViewModels.Selection;

namespace FishermanApp.Views.Selection;

public partial class GenericSelectionModal : ContentPage
{
	private GenericSelectionViewModel _viewModel;
	public GenericSelectionModal()
	{
		InitializeComponent();
		BindingContext = _viewModel = new GenericSelectionViewModel();

    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        Thread.Sleep(500);
        _viewModel.InitializeAsync();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
      
    }
}