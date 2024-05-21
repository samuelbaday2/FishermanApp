using FishermanApp.ViewModels.Modals;

namespace FishermanApp.Views.Modals;

public partial class TripTracker : ContentPage
{
	private TripTrackerViewModel _viewModel;
	public TripTracker(int tripId)
	{
		InitializeComponent();
		BindingContext = _viewModel = new TripTrackerViewModel(tripId);


	}
}