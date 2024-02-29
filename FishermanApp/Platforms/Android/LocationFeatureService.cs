using Android.Content;
using Android.Locations;
using Android.Runtime;
using FishermanApp.Services.LocationService;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp
{
    public class LocationFeatureService : ILocationFeatureService
    {
        public bool IsConnected { get; set ; }

        public event EventHandler<bool> ConnectionStatusChanged;

        public Task StartConnectionListenerAsync()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);


            //locationManager.AddGpsStatusListener(new GpsListener());

            IsConnected = locationManager.IsProviderEnabled(LocationManager.GpsProvider);

            return Task.CompletedTask;
        }

       
    }
}
