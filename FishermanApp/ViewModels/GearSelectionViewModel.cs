using CommunityToolkit.Mvvm.Messaging;
using FishermanApp.Objects;
using FishermanApp.Resources.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
  
    public class GearSelectionViewModel : BaseViewModel
    {
        private ObservableCollection<GearObject> _gearCollection;
        private ObservableCollection<GearSelectionObject> _gearSelectionCollection;

        public event EventHandler<GearObject> OnGearSelected = delegate { };

        private bool IsGearSelected = false;

        public ObservableCollection<GearObject> GearCollection { get { return _gearCollection; } set { SetProperty(ref _gearCollection, value); } }
        public ObservableCollection<GearSelectionObject> GearSelectionCollection { get { return _gearSelectionCollection; } set { SetProperty(ref _gearSelectionCollection, value); } }

        public ICommand OnGearSelectCommand { private set; get; }
        List<GearObject> GearList;
        public GearSelectionViewModel() 
        {
            GearList = new List<GearObject>();
            GearList = JsonConvert.DeserializeObject<List<GearObject>>(Preferences.Get(Pref.GEAR_LIST, string.Empty));

            OnGearSelectCommand = new Command(DoGearSelect);

            GearCollection = new ObservableCollection<GearObject>();
            GearSelectionCollection = new ObservableCollection<GearSelectionObject>();
            //InitializeAsync();
            OnGearSelected += onGearSelectedCommand;
        }

        private void onGearSelectedCommand(object sender, GearObject e)
        {
          
        }

        public async Task InitializeAsync()
        {
            IsGearSelected = false;
            GearSelectionCollection = new ObservableCollection<GearSelectionObject>();

            foreach (string gear in GearList.Where(x => x.GearType.Trim() != string.Empty).Select(x => x.GearType.Trim()).Distinct())
            {
                string ImageSource = string.Empty;
                string BackgroundColor = null;

                if (!IsGearSelected) 
                {
                    if (gear.Trim().Contains(AppResources.Nets))
                    {
                        ImageSource = "net_icon";
                    }

                    if (gear.Trim().Contains(AppResources.MiscGear))
                    {
                        ImageSource = "misc_icon";
                    }

                    if (gear.Trim().Contains(AppResources.TrollAndPole))
                    {
                        ImageSource = "fishingpole_icon";
                    }

                    if (gear.Trim().Contains(AppResources.Longlines))
                    {
                        ImageSource = "flag_icon";
                    }
                }
               

                GearSelectionCollection.Add(new GearSelectionObject
                {
                    SelectionTitle = gear.Trim(),
                    SelectionImage = ImageSource,
                    BackgroundColor = BackgroundColor,
                });
            }
        }

        private void DoGearSelect(object obj)
        {
            GearSelectionObject SelectedGear = obj as GearSelectionObject;
            List<GearObject> GearList = JsonConvert.DeserializeObject<List<GearObject>>(Preferences.Get(Pref.GEAR_LIST, string.Empty));

            GearSelectionCollection = new ObservableCollection<GearSelectionObject>();

            try
            {
                foreach (string gear in GearList.Where(x => x.GearType.Trim() != string.Empty && x.GearType.Trim() == SelectedGear.SelectionTitle).Select(x => x.GearName.Trim()))
                {
                    string ImageSource = string.Empty;
                    string BackgroundColor = null;
                  

                    GearSelectionCollection.Add(new GearSelectionObject
                    {
                        SelectionTitle = gear.Trim(),
                        SelectionImage = ImageSource,
                        BackgroundColor = BackgroundColor,
                    });
                }
            }
            catch 
            {
               
            }

           
            if (IsGearSelected) 
            {
                GearObject ObjectToInvoke = GearList.Where(x => x.GearName.Trim() == SelectedGear.SelectionTitle).FirstOrDefault();
               
                Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.FlyoutStartTrip)).FirstOrDefault();
                OnGearSelected.Invoke(this, ObjectToInvoke);

                WeakReferenceMessenger.Default.Send(ObjectToInvoke);
            }

            IsGearSelected = true;
        }
    }
}
