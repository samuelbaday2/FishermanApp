using FishermanApp.Resources.Localization;
using FishermanApp.ViewModels;
using FishermanApp.ViewModels.Selection;

namespace FishermanApp.Views.Pages;

public partial class EnterSetDetailPage : ContentPage
{
    private EnterSetDetailPageViewModel viewModel;
    private HookTypeSelectionViewModel _hookTypeViewModel;
    public EnterSetDetailPage(HookTypeSelectionViewModel hookTypeSelectionView)
	{
		InitializeComponent();
		BindingContext = viewModel = new EnterSetDetailPageViewModel();
        _hookTypeViewModel = hookTypeSelectionView;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
    }

    private async void HookTypeEntry_Focused(object sender, FocusEventArgs e)
    {
        Entry Entry = (Entry)sender;
 
        Entry.IsEnabled = false;
        Entry.IsEnabled = true;

        Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.HookType)).FirstOrDefault();
    }

    private async void BaitSpeciesEntry_Focused(object sender, FocusEventArgs e)
    {
        Entry Entry = (Entry)sender;

        Entry.IsEnabled = false;
        Entry.IsEnabled = true;

        Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.BaitSpecie)).FirstOrDefault();
    }
}