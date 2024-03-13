using FishermanApp.ViewModels.Modals;

namespace FishermanApp.Views.Modals;

public partial class EffortModal : ContentPage
{
	private EffortModalViewModel _viewModel;
    private int _tripId;
    public EffortModal(int tripId)
	{
		InitializeComponent();
		BindingContext = _viewModel = new EffortModalViewModel();
        _tripId = tripId;

    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        SetPicker.ItemsSource = await _viewModel.PopulatePickerAsync(_tripId);
        if (SetPicker.ItemsSource != null) 
        {
            SetPicker.SelectedIndex = 0;
        }

        
    }

    private async void SetPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        await _viewModel.InitializeAsync(_tripId, SetPicker.SelectedIndex);
    }
}