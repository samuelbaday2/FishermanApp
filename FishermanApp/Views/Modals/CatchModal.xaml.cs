using FishermanApp.ViewModels.Modals;

namespace FishermanApp.Views.Modals;

public partial class CatchModal : ContentPage
{
	private CatchModalViewModel _viewModel;
    private int _tripId;
	public CatchModal(int tripId)
	{
		InitializeComponent();
		BindingContext = _viewModel = new CatchModalViewModel();
        _tripId = tripId;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Initialize(_tripId);
    }
}