using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class SettingsPage : ContentPage
{
	private SettingsPageViewModel _viewModel;
	public SettingsPage(SettingsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = _viewModel;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UoMPicker.SelectedIndex = Preferences.Get("UOM", 0);
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
		Picker UoMPicker = (Picker)sender;

		Preferences.Set("UOM", UoMPicker.SelectedIndex);
    }
}