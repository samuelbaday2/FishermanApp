using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
using FishermanApp.ViewModels;
using FishermanApp.ViewModels.Selection;

namespace FishermanApp.Views.Pages;

public partial class EnterCatchDetailsPage : ContentPage
{
	private EnterCatchDetailsPageViewModel viewModel;
    private Object CurrentObject; 
	public EnterCatchDetailsPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new EnterCatchDetailsPageViewModel();

    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (InitializeConfig.InitializeFunction)
        {
            await viewModel.Initialize();
            SetPicker.ItemsSource = viewModel.PickerItems;
            InitializeConfig.InitializeFunction = false;
        }
        
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        Entry Entry = (Entry)sender;

        CurrentObject = Entry.ReturnCommandParameter;
        MessagingCenter.Subscribe<CatchSpeciesSelectionViewModel, SelectionObject>(this, AppResources.CatchSpeciesSelection, async (sender, arg) =>
        {
            CatchObject catchObject = CurrentObject as CatchObject;
            viewModel.UpdateCatchRow(catchObject.Index,arg.SelectionTitle, catchObject.ScientificName);
            MessagingCenter.Unsubscribe<CatchSpeciesSelectionViewModel, SelectionObject>(this, AppResources.CatchSpeciesSelection);
        });

        MessagingCenter.Subscribe<string, SelectionObject>("custom_species", "custom_species", async (sender, arg) =>
        {
            CatchObject catchObject = CurrentObject as CatchObject;
            //MessagingCenter.Send(this, AppResources.CatchSpeciesSelection, arg);
            viewModel.UpdateCatchRow(catchObject.Index, arg.SelectionTitle, arg.SubTitle);
            MessagingCenter.Unsubscribe<CatchSpeciesSelectionViewModel, SelectionObject>(this, "custom_species");
        });

        Entry.IsEnabled = false;
        Entry.IsEnabled = true;

        Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchSpeciesSelection)).FirstOrDefault();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        CatchObject catchObject = btn.CommandParameter as CatchObject;
        await viewModel.UpdateCatchRowDelete(catchObject.Index);
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.Home)).FirstOrDefault();
    }
}