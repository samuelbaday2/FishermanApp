using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class AddCrewPage : ContentPage
{
	public AddCrewPage()
	{
		InitializeComponent();
		BindingContext = new AddCrewViewModel();
	}
}