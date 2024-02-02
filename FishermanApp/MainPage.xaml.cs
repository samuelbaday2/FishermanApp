using FishermanApp.ViewModels;

namespace FishermanApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = mainPageViewModel;

    }

    
}

