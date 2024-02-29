using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.LocationService
{
    public interface ILocationFeatureService
    {
        bool IsConnected { get; set; }
        event EventHandler<bool> ConnectionStatusChanged;
        Task StartConnectionListenerAsync();
    }
}
