using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
using FishermanApp.ViewModels.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CatchObject = FishermanApp.Objects.DbObjects.CatchObject;

namespace FishermanApp.ViewModels
{
    public class EnterCatchDetailsPageViewModel : BaseViewModel
    {
        private ObservableCollection<CatchObject> _catchDataCollection;
        private string _catchQuanity;

        private List<string> _pickerItems;
        private int counter = 0;

        public ObservableCollection<CatchObject> CatchDataCollection { get { return _catchDataCollection; } set { SetProperty(ref _catchDataCollection, value); } }
        public string CatchQuanity { get { return _catchQuanity; } set { SetProperty(ref _catchQuanity, value); } }
        public List<string> PickerItems { get { return _pickerItems; } set { SetProperty(ref _pickerItems, value); } }

        public ICommand AddRowCommand { private set; get; }
        public ICommand AddCatchCommand { private set; get; }
        public EnterCatchDetailsPageViewModel() 
        {
            AddRowCommand = new Command(DoAddRow);
            AddCatchCommand = new Command(DoAddCatch);

            Initialize();
            InitializeMessaging();
        }
        public void InitializeMessaging()
        {
           
       
        }
        public async Task Initialize() {
            counter = 0;
            CatchDataCollection = new ObservableCollection<CatchObject>();

            PickerItems = new List<string>();

            var lastTripData = await _tripTable.GetItemsAsync();
            var existingSets = await _tripSetTable.GetItemsAsync();
            var catchData = await _catchTable.GetItemsBySetIdAsync(CurrentSetObjectStatic.SetNumberObject.SetId);

            var lastSet = existingSets.Where(x => x.Id == (existingSets.LastOrDefault().Id)).LastOrDefault();
            int currentSetCount = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).Count();

            for (int x = 0; x < currentSetCount; x++) 
            {
                PickerItems.Add($"Set {x + 1}");
            }

            foreach (DBCatchObject obj in catchData) 
            {
                CatchDataCollection.Add(new CatchObject
                {
                    Index = counter++,
                    Quantity = obj.Quantity,
                    ScientificName = obj.ScientificName,
                    Species = obj.Species,
                    Id = obj.Id,
                    Weight = obj.Weight, 
                    ProcessingType = obj.ProcessingType,
                });
            }

            CatchDataCollection.Add(new CatchObject { Index = counter++ });
        }
        public async Task UpdateCatchRow(int itemIndex,string species, string scientificName, string weight, string processgingType) {
            for(int x = 0;  x < CatchDataCollection.Count; x++)
            {
                if (CatchDataCollection[x].Index == itemIndex)
                {
                    CatchObject catchObject = CatchDataCollection[x];
                    catchObject.Species = species;
                    catchObject.ScientificName = scientificName;
                    catchObject.Weight = weight;
                    catchObject.ProcessingType = processgingType;
                    CatchDataCollection[x] = catchObject;
                }
            }

            if (CatchDataCollection[CatchDataCollection.Count - 1].Species != null) 
            {
                CatchDataCollection.Add(new CatchObject { Index = counter++ });
            }
         
        }

        public async Task UpdateCatchRowDelete(int itemIndex)
        {
            for (int x = 0; x < CatchDataCollection.Count; x++)
            {
                if (CatchDataCollection[x].Index == itemIndex)
                {
                    CatchObject catchObject = CatchDataCollection[x];
                    if (catchObject.Id != null) 
                    {
                        await _catchTable.DeleteItemAsync(await _catchTable.GetItemAsync((int)catchObject.Id));
                    }

                    CatchDataCollection.Remove(CatchDataCollection.Where(o => o.Index == itemIndex).FirstOrDefault());

                    return;
                }
            }

            if (CatchDataCollection[CatchDataCollection.Count - 1].Species != null)
            {
                CatchDataCollection.Add(new CatchObject { Index = counter++ });
            }

        }

        private async void DoAddCatch(object obj)
        {
            if (false)//(CatchDataCollection.Count == 1 && CatchDataCollection.FirstOrDefault().Species == null)
            {
                await Toast.Make(AppResources.CatchDataCannotBeEmpty,ToastDuration.Long).Show();
            }
            else {
                SetBusyStatusAsync(false);
                var lastTripData = await _tripTable.GetItemsAsync();
                var existingSets = await _tripSetTable.GetItemsAsync();


                var lastSet = existingSets.Where(x => x.Id == (existingSets.LastOrDefault().Id)).LastOrDefault();
                int currentSetCount = existingSets.Where(x => x.TripId == lastTripData.LastOrDefault().Id).Count();

                try
                {
                    foreach (CatchObject catchObject in CatchDataCollection)
                    {
                        if (catchObject.Species != null && catchObject.Species.Length > 0)
                        {
                            DBCatchObject dbCatchObject = new DBCatchObject
                            {
                                IsActive = true,
                                Quantity = catchObject.Quantity,
                                SetId = CurrentSetObjectStatic.SetNumberObject.SetId,//lastSet.Id,
                                TripId = lastTripData.LastOrDefault().Id,
                                Species = catchObject.Species,
                                RecordedOn = DateTime.Now,
                                ScientificName = catchObject.ScientificName,
                                SetNumber = currentSetCount,
                                Weight = catchObject.Weight,
                                ProcessingType = catchObject.ProcessingType,
                            };

                            if (catchObject.Id != null) 
                            {
                                dbCatchObject.Id = (int)catchObject.Id;
                            }

                            await _catchTable.SaveItemAsync(dbCatchObject);
                        }

                    }

                    lastSet.HasCatchData = true;
                    await _tripSetTable.SaveItemAsync(lastSet);

                    Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.Home)).FirstOrDefault();
                }
                catch (Exception ee)
                {

                }
                finally
                {
                    SetBusyStatusAsync(true);
                }
            }
                               
        }

        private async void DoAddRow(object obj)
        {
            CatchDataCollection.Add(new CatchObject { Index = counter++ });
        }
    }
}
