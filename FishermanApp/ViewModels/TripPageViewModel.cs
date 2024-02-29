using CommunityToolkit.Mvvm.Messaging;
using FishermanApp.Objects;
using FishermanApp.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class TripPageViewModel : BaseViewModel
    {
        GearSelectionViewModel _gearSelectionViewModel;

        private bool _hasBycatch;
        private bool _hasBycatchNo;
        private string _gearText;
        private string _quantityText;
        public bool HasBycatch { get { return _hasBycatch; } set { SetProperty(ref _hasBycatch, value); } }
        public bool HasBycatchNo { get { return _hasBycatch; } set { SetProperty(ref _hasBycatch, value); } }
        public string GearText { get { return _gearText; } set { SetProperty(ref _gearText, value); } }
        public string QuantityText { get { return _quantityText; } set { SetProperty(ref _quantityText, value); } }


        public ICommand ByCatchYesCommand { private set; get; }
        public ICommand ByCatchNoCommand { private set; get; }
        public ICommand EndTripCommand { private set; get; }
        public TripPageViewModel(GearSelectionViewModel gearSelectionViewModel) 
        {
            ByCatchYesCommand = new Command(DoYesClick);
            ByCatchNoCommand = new Command(DoNoClick);
            EndTripCommand = new Command(DoEndTrip);


            _gearSelectionViewModel = gearSelectionViewModel;
            //HasBycatch = true;
            //HasBycatchNo = !HasBycatch;

            WeakReferenceMessenger.Default.Register<GearObject>(this, (r, m) =>
            {
                GearText = m.GearName;
            });
        }

        public async Task InitializeAsync() {
            GearText = string.Empty;
            QuantityText = string.Empty;
        }

        private void onGearSelectedCommand(object sender, GearObject e)
        {
            GearText = e.GearName;
        }

        private void DoEndTrip(object obj)
        {
            Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.Home)).FirstOrDefault();
        }

        private void DoNoClick(object obj)
        {
            HasBycatch = false;
            HasBycatchNo = !HasBycatch;
        }

        private void DoYesClick(object obj)
        {
            HasBycatch = true;
            HasBycatchNo = !HasBycatch;
        }
    }
}
