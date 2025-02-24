using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class VesselnfoPage : ContentPage
{
	private MainPageViewModel _vm;
	public VesselnfoPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = _vm = mainPageViewModel;

        try
        {
            CountryEntry.Text = Preferences.Get($"{nameof(CountryEntry)}", string.Empty);
            HomePortEntry.Text = Preferences.Get($"{nameof(HomePortEntry)}", string.Empty);
        }
        catch { }
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();    
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
		
		Preferences.Set($"{nameof(CountryEntry)}", CountryEntry.Text);
        Preferences.Set($"{nameof(HomePortEntry)}", HomePortEntry.Text);
    }
}