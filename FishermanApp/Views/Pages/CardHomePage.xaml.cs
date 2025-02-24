using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class CardHomePage : ContentPage
{
	private MainPageViewModel _vm;
	public CardHomePage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = _vm = mainPageViewModel;

    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InitializeAsync();
    }
}