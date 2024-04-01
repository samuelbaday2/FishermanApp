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
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (InitializeConfig.InitializeFunction)
        {
            viewModel.Initialize();
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

        Entry.IsEnabled = false;
        Entry.IsEnabled = true;

        Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchSpeciesSelection)).FirstOrDefault();
    }
}