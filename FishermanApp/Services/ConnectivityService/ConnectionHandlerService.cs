using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.ConnectivityService
{
    public class ConnectionHandlerService : IConnectionHandlerService
    {
        public bool IsConnected { get ; set; }
        public event EventHandler<bool> ConnectionStatusChanged;

        public async Task StartConnectionListenerAsync()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            ConnectionStatusChanged.Invoke(this, IsConnected);
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = true;
            if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {
                IsConnected = false;
                Console.WriteLine("Internet access is available but is limited.");
           
            }

            else if (e.NetworkAccess != NetworkAccess.Internet)
            {
                IsConnected = false;
                Console.WriteLine("Internet access has been lost.");
              
            }          

            ConnectionStatusChanged.Invoke(this, IsConnected);
            return;         
        }
    }
}
