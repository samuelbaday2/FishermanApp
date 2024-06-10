using FishermanApp.Objects;

namespace FishermanApp.Views.Modals;

public partial class CustomSpecie : ContentPage
{
	public CustomSpecie()
	{
		InitializeComponent();

        CustomSpeciesEntry.Text = string.Empty;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopModalAsync();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        
        MessagingCenter.Send(this, "custom_species", new SelectionObject {
            SelectionTitle = CustomSpeciesEntry.Text
        });
        await Shell.Current.Navigation.PopModalAsync();
    }
}