using FishermanApp.Objects.DbObjects;
using FishermanApp.ViewModels;
using FishermanApp.Views.Modals;

namespace FishermanApp.Views.Pages;

public partial class TripHistoryPage : ContentPage
{
	private TripHistoryPageViewModel _viewModel;
    private bool InitialSetup = true;
	public TripHistoryPage(TripHistoryPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = _viewModel = viewModel;

        _viewModel.SetMinimumDateAsync();
	}
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        Thread.Sleep(500);
        try
        {
            DbTripObject firstTripObject = await _viewModel._tripTable.GetFirstItemAsync();
            HistoryDatePicker.MinimumDate = firstTripObject.RecordedOn; 
            HistoryDatePicker.MaximumDate = DateTime.Today;
        }
        catch { }

        if (InitialSetup) 
        {
            HistoryDatePicker.Date = DateTime.Today;
            _viewModel.FilterTripsAsync();
            InitialSetup = false;
        }
       
    }

    private async void HistoryDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (!InitialSetup)
        {
            _viewModel.FilterTripsAsync();
        }
       
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        Button ViewCatchButton = (Button)sender;
        DbTripObject tripObject = ViewCatchButton.CommandParameter as DbTripObject;

        Console.WriteLine(tripObject);
        await Shell.Current.Navigation.PushModalAsync(new CatchModal(tripObject.Id));
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        Button ViewEffortButton = (Button)sender;
        DbTripObject tripObject = ViewEffortButton.CommandParameter as DbTripObject;

        Console.WriteLine(tripObject);

        await Shell.Current.Navigation.PushModalAsync(new EffortModal(tripObject.Id));
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        Button ViewEffortButton = (Button)sender;
        DbTripObject tripObject = ViewEffortButton.CommandParameter as DbTripObject;

        Console.WriteLine(tripObject);

        await Shell.Current.Navigation.PushAsync(new MapPage(tripObject));
    }

    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        Button TripTrackerButton = (Button)sender;
        DbTripObject tripObject = TripTrackerButton.CommandParameter as DbTripObject;

        Console.WriteLine(tripObject);

        await Shell.Current.Navigation.PushModalAsync(new TripTracker(tripObject.Id));
    }
}