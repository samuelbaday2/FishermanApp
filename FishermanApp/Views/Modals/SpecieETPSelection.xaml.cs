using FishermanApp.Views.Selection;

namespace FishermanApp.Views.Modals;

public partial class SpecieETPSelection : ContentPage
{
	public SpecieETPSelection()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
        await Shell.Current.Navigation.PushAsync(new CatchSpeciesSelection(new(true)));      
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
        await Shell.Current.Navigation.PushAsync(new CatchSpeciesSelection(new(false)));
    }
}