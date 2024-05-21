using FishermanApp.Services.SosFeature;
using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class SosPage : ContentPage
{
	private readonly SosPageViewModel _viewModel;
	public SosPage(ISosService sosService)
	{
		InitializeComponent();
		BindingContext = _viewModel = new SosPageViewModel(sosService);
	}
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
		_viewModel.TurnOffSignal();
    }
}