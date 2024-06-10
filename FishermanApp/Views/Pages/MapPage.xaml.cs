using FishermanApp.Constants.LocalDatabase.Tables;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Views.Modals;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace FishermanApp.Views.Pages;

public partial class MapPage : ContentPage
{
    public const double AngleConvert = 180;
    public const double MinLatValue = -90;
    public const double MaxLatValue = 90;
    public const double MinLonValue = -180;
    public const double MaxLonValue = 180;
    public const double MaxWorldLength = 360;
    private DbTripObject DbTripObject;
    public MapPage(DbTripObject tripObject)
	{
		InitializeComponent();

        FishingMap.MapType = Microsoft.Maui.Maps.MapType.Street;
      
        FishingMap.Loaded += async (s, e) => 
        {
            await Task.Delay(1000);
            MoveMap(tripObject);

        };

        DbTripObject = tripObject;
    }

    public async Task MoveMap(DbTripObject tripObject) 
    {
        try
        {
            Location location = new Location(double.Parse(tripObject.StartTripLatitude), double.Parse(tripObject.StartTripLongitude));
            MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
            Map map = new Map(mapSpan);

            double zoomLevel = 0.1;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (FishingMap.VisibleRegion != null)
            {
                FishingMap.MoveToRegion(mapSpan);
            }

            //Pin pin = new Pin
            //{
            //    Label = "Fishing Trip Start Location",
            //    Address = $"{double.Parse(tripObject.StartTripLatitude)},{double.Parse(tripObject.StartTripLongitude)}",
            //    Type = PinType.Place,
            //    Location = new Location(double.Parse(tripObject.StartTripLatitude), double.Parse(tripObject.StartTripLongitude))
            //};
            //FishingMap.Pins.Add(pin);

            List<DBSetObject> setDetails = await new TripSetTable().GetItemsAsync();
            setDetails = new List<DBSetObject>(setDetails.Where(x => x.TripId == tripObject.Id));
            List<Location> locations = new List<Location>();

            int setCounter = 1;
            foreach (DBSetObject setObj in setDetails) 
            {
                Pin pin = new Pin
                {
                    Label = $"Fishing Set {setCounter++}",
                    Address = $"{double.Parse(setObj.StartSetLatitude)},{double.Parse(setObj.StartSetLongitude)}",
                    Type = PinType.Place,
                    Location = new Location(double.Parse(setObj.StartSetLatitude), double.Parse(setObj.StartSetLongitude))
                };
                FishingMap.Pins.Add(pin);
                locations.Add(new Location(double.Parse(setObj.StartSetLatitude), double.Parse(setObj.StartSetLongitude)));
            }

            FishingMap.MoveToRegion(await GetSpan(locations,default));
        }
        catch (Exception ee)
        {

        }
    }

    public async Task<MapSpan> GetSpan(List<Location> locations, double safeAreaLevel = 1.1)
    {
        var north = MinLatValue;
        var south = MaxLatValue;
        var west = MaxLonValue;
        var east = MinLonValue;

        foreach (var loc in locations)
        {
            north = Math.Max(north, loc.Latitude);
            south = Math.Min(south, loc.Latitude);
            west = Math.Min(west, loc.Longitude);
            east = Math.Max(east, loc.Longitude);
        }

        if (west > east)
        {
            east += MaxWorldLength;
        }

        var halfHeight = (north - south) / 2.0;
        var halfWidth = Math.Abs(east - west) / 2.0;
        var centre = new Location((south + north) / 2.0, NormalizeLongitude((west + east) / 2.0));
        var northLocation = new Location(centre.Latitude + halfHeight, centre.Longitude);
        var southLocation = new Location(centre.Latitude - halfHeight, centre.Longitude);
        var eastLocation = new Location(centre.Latitude, halfWidth != MaxLonValue ? NormalizeLongitude(centre.Longitude + halfWidth) : MaxLonValue);
        var westLocation = new Location(centre.Latitude, !halfWidth.Equals(MaxLonValue) ? NormalizeLongitude(centre.Longitude - halfWidth) : MinLonValue);

        north = ToDegrees(northLocation.Latitude);
        south = ToDegrees(southLocation.Latitude);
        east = ToDegrees(eastLocation.Longitude);
        west = ToDegrees(westLocation.Longitude);

        var latDeg = Math.Abs(north - south) * safeAreaLevel;
        var lonDeg = Math.Abs(east - west) * safeAreaLevel;

        MapSpan span = new(centre, latDeg, lonDeg);
        return span;
    }

    private double NormalizeLongitude(double longitude)
    {
        if (longitude is < MinLonValue or > MaxLonValue)
        {
            return longitude - Math.Floor((longitude + MaxLonValue) / MaxWorldLength) * MaxWorldLength;
        }

        return longitude;
    }

    private double ToDegrees(double coordinate)
    {
        double degree = 0;

        if (coordinate is >= MinLonValue and <= MaxLonValue)
        {
            degree = (coordinate + AngleConvert + MaxWorldLength) % MaxWorldLength;
        }

        return degree;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushModalAsync(new TripTracker(DbTripObject.Id));
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }
}