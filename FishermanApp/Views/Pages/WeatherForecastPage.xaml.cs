using CommunityToolkit.Maui.Layouts;
using FishermanApp.Services.Weather;
using FishermanApp.ViewModels;

namespace FishermanApp.Views.Pages;

public partial class WeatherForecastPage : ContentPage
{
    readonly WeatherForecastPageViewModel vm = new(new WeatherDataService());

    public WeatherForecastPage()
	{
		InitializeComponent();
		BindingContext = vm;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
    }
    private void HomePage_Orientation(object sender, EventArgs e)
    {
        string visualStateOrientation = Height > Width ? "Portrait" : "Landscape";

        VisualStateManager.GoToState(gridParent, visualStateOrientation);
        VisualStateManager.GoToState(topLayerLower1, visualStateOrientation);
        VisualStateManager.GoToState(topLayerHigher2, visualStateOrientation);
        VisualStateManager.GoToState(gridAdminTopLayerHigher2, visualStateOrientation);
        VisualStateManager.GoToState(iconAndTextLocation, visualStateOrientation);
        VisualStateManager.GoToState(iconPicker, visualStateOrientation);
        VisualStateManager.GoToState(updating, visualStateOrientation);
        VisualStateManager.GoToState(lookTemperature, visualStateOrientation);
        VisualStateManager.GoToState(lookWeatherAndDt, visualStateOrientation);
        VisualStateManager.GoToState(dividingLine, visualStateOrientation);
        VisualStateManager.GoToState(gridComplexDescriptionClimate, visualStateOrientation);
        VisualStateManager.GoToState(stackTodayAnd7Days, visualStateOrientation);
        VisualStateManager.GoToState(scrollForecast24Hours, visualStateOrientation);
        VisualStateManager.GoToState(uniformItemsLayoutForecast24Hours, visualStateOrientation);
        VisualStateManager.GoToState(borderLoading, visualStateOrientation);
    }
    private void SelectedWeather_Tapped(object sender, TappedEventArgs e)
    {
        VisualElement surfaceWeather = sender as VisualElement;
        UniformItemsLayout parent = surfaceWeather.Parent as UniformItemsLayout;

        foreach (View child in parent.Children.Cast<View>())
        {
            VisualStateManager.GoToState(child, "Normal");
            ChangeSurfaceControls(child, false);
        }
        VisualStateManager.GoToState(surfaceWeather, "Selected");
        ChangeSurfaceControls(surfaceWeather, true);
    }

    private static void ChangeSurfaceControls(VisualElement child, bool isSelected)
    {
        Label labelTemperature = child.FindByName<Label>("labelTemperature");
        Image imageSmallWeather = child.FindByName<Image>("imageSmallWeather");
        Label labelHour = child.FindByName<Label>("labelHour");

        string visualStateControl = isSelected ? "Selected" : "Normal";
        VisualStateManager.GoToState(labelTemperature, visualStateControl);
        VisualStateManager.GoToState(imageSmallWeather, visualStateControl);
        VisualStateManager.GoToState(labelHour, visualStateControl);
    }
}