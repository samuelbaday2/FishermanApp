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

    private void Entry_Completed(object sender, EventArgs e)
    {
        Entry entry = (Entry)sender;
        string input = entry.Text;
        int uom = Preferences.Get("UOM", 0);
        if (uom == 0) 
        {
            entry.Text = $"{input} (m)";
        }
        else if (uom == 1)
        {
            entry.Text = $"{input} (ft)";
        }
        else if (uom == 2)
        {
            entry.Text = $"{input} (in)";
        }
    }
}