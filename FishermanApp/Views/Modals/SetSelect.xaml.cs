using FishermanApp.Objects;
using FishermanApp.Resources.Localization;
using FishermanApp.ViewModels.Modals;
using FishermanApp.Views.Pages;

namespace FishermanApp.Views.Modals;

public partial class SetSelect : ContentPage
{
	private SetSelectViewModel _viewModel;
	public SetSelect()
	{
		InitializeComponent();

		BindingContext = _viewModel = new SetSelectViewModel();

		_viewModel.LoadData();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.Navigation.PopModalAsync();
        
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Button button = sender as Button;

        SetNumberObject item = button.CommandParameter as SetNumberObject;

        CurrentSetObjectStatic.SetNumberObject = item;

        //Shell.Current.Navigation.PopModalAsync();
        //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();

        Shell.Current.Navigation.PushAsync(new EnterCatchDetailsPage());
    }
}