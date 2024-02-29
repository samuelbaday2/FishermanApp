using FishermanApp.Resources.Localization;
using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class TripPage : ContentPage
{
	TripPageViewModel viewModel;
    GearSelectionViewModel _gearSelectionViewModel;
	
	public TripPage(GearSelectionViewModel gearSelectionViewModel)
	{
		InitializeComponent();
		BindingContext = viewModel = new TripPageViewModel(gearSelectionViewModel);
        _gearSelectionViewModel = gearSelectionViewModel;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        viewModel.InitializeAsync();
    }

    private async void Entry_Focused(object sender, FocusEventArgs e)
    {
        Entry GearEntry = (Entry)sender;
        await _gearSelectionViewModel.InitializeAsync();

        GearEntry.IsEnabled = false;
        GearEntry.IsEnabled = true;

        Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.GearSelection)).FirstOrDefault();
    }
}