using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class ConnectivityPage : ContentPage
{
	private MainPageViewModel _vm;
	public ConnectivityPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = _vm = mainPageViewModel;
	}
}