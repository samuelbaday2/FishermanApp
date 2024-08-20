using FishermanApp.Objects.DbObjects;
using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class ManageCrewPage : ContentPage
{
    private ManageCrewViewModel _viewModel;
	public ManageCrewPage()
	{
		InitializeComponent();
        BindingContext = _viewModel = new ManageCrewViewModel();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.GetAllCrew();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new AddCrewPage());
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        DBCrewObject dBCrewObject = btn.CommandParameter as DBCrewObject;

        _viewModel.DeleteCrew(dBCrewObject);
    }
}