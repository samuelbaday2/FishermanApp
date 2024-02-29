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

namespace FishermanApp.ViewModels.Selection
{
    public class SelectionBaseViewModel : BaseViewModel
    {
        private ObservableCollection<SelectionObject> _selectionCollection;

        public ObservableCollection<SelectionObject> SelectionCollection { get { return _selectionCollection; } set { SetProperty(ref _selectionCollection, value); } }
        public List<SelectionObject> SelectionList;

        public ICommand OnGearSelectCommand { private set; get; }
        public SelectionBaseViewModel()
        {
            SelectionList = new List<SelectionObject>();

            OnGearSelectCommand = new Command(DoGearSelect);


            SelectionCollection = new ObservableCollection<SelectionObject>();
        }
        //public async Task InitializeAsync()
        //{

        //    SelectionCollection = new ObservableCollection<SelectionObject>();

        //    //foreach (string gear in GearList.Where(x => x.GearType.Trim() != string.Empty).Select(x => x.GearType.Trim()).Distinct())
        //    //{
        //    //    SelectionCollection.Add(new SelectionObject
        //    //    {
        //    //        SelectionTitle = gear.Trim(),
        //    //        SelectionImage = ImageSource,
        //    //        BackgroundColor = BackgroundColor,
        //    //    });
        //    //}
        //}

        private void DoGearSelect(object obj)
        {
            GearSelectionObject SelectedGear = obj as GearSelectionObject;
            List<GearObject> GearList = JsonConvert.DeserializeObject<List<GearObject>>(Preferences.Get(Pref.GEAR_LIST, string.Empty));

            SelectionCollection = new ObservableCollection<SelectionObject>();

            try
            {
                foreach (string gear in GearList.Where(x => x.GearType.Trim() != string.Empty && x.GearType.Trim() == SelectedGear.SelectionTitle).Select(x => x.GearName.Trim()))
                {
                    string ImageSource = string.Empty;
                    string BackgroundColor = null;


                    SelectionCollection.Add(new SelectionObject
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


            GearObject ObjectToInvoke = GearList.Where(x => x.GearName.Trim() == SelectedGear.SelectionTitle).FirstOrDefault();

            Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.FlyoutStartTrip)).FirstOrDefault();

            WeakReferenceMessenger.Default.Send(ObjectToInvoke);
        }
    }
}
