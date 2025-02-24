using FishermanApp.ViewModels.Selection;
using FishermanApp.Views.Modals;
using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;

namespace FishermanApp.Views.Selection;

public partial class CatchSpeciesSelection : ContentPage
{
	private CatchSpeciesSelectionViewModel _viewModel;
	public CatchSpeciesSelection(CatchSpeciesSelectionViewModel catchSpeciesSelectionViewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = catchSpeciesSelectionViewModel;

   
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        Thread.Sleep(500);
        _viewModel.InitializeAsync();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();
        //Shell.Current.Navigation.PopAsync();
        //InitializeConfig.InitializeFunction = false;
        //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains("CustomSpecie")).FirstOrDefault();
        Shell.Current.Navigation.PushAsync(new CustomSpecie());
    }
}