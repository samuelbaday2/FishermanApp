using FishermanApp.Objects;
using FishermanApp.Resources.Localization;

namespace FishermanApp.Views.Modals;

public partial class CustomSpecie : ContentPage
{
	public CustomSpecie()
	{
		InitializeComponent();

        CustomSpeciesEntry.Text = string.Empty;
	}
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(500);
        CustomSpeciesEntry.Text = string.Empty;
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();
        Shell.Current.Navigation.PopAsync();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        
        MessagingCenter.Send("custom_species", "custom_species", new SelectionObject {
            SelectionTitle = CustomSpeciesEntry.Text
        });
        //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();

        Shell.Current.Navigation.PopAsync();
        Shell.Current.Navigation.PopAsync();
    }
}