using FishermanApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand StartTripCommand { private set; get; }
        public MainPageViewModel(IPermissionService permissionService) {
            StartTripCommand = new Command(DoStartTrip);
        }

        private async void DoStartTrip(object obj)
        {
            GetCurrentLocation();
        }
    }
}
