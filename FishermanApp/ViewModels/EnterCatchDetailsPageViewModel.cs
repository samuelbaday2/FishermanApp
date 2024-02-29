using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using FishermanApp.Resources.Localization;
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

        public ObservableCollection<CatchObject> CatchDataCollection { get { return _catchDataCollection; } set { SetProperty(ref _catchDataCollection, value); } }
        public string CatchQuanity { get { return _catchQuanity; } set { SetProperty(ref _catchQuanity, value); } }

        public ICommand AddRowCommand { private set; get; }
        public ICommand AddCatchCommand { private set; get; }
        public EnterCatchDetailsPageViewModel() 
        {
            AddRowCommand = new Command(DoAddRow);
            AddCatchCommand = new Command(DoAddCatch);

            Initialize();
        }
        public async Task Initialize() {
            CatchDataCollection = new ObservableCollection<CatchObject>
            {
                new CatchObject(),
            };
        }

        private async void DoAddCatch(object obj)
        {
            var lastTripData = await _tripTable.GetItemsAsync();
            var existingSets = await _tripSetTable.GetItemsAsync();


            var lastSet = existingSets.Where(x => x.Id == (existingSets.LastOrDefault().Id)).LastOrDefault();

            try
            {
                foreach (CatchObject catchObject in CatchDataCollection)
                {
                    if (catchObject.Species != null && catchObject.Species.Length > 0)
                    {
                        await _catchTable.SaveItemAsync(new DBCatchObject
                        {
                            IsActive = true,
                            Quantity = catchObject.Quantity,
                            SetId = lastSet.Id,
                            Species = catchObject.Species,
                            RecordedOn = DateTime.Now,
                        });
                    }

                }

                lastSet.HasCatchData = true;
                await _tripSetTable.SaveItemAsync(lastSet);

                Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.Home)).FirstOrDefault();
            }
            catch (Exception ee)
            { 
            
            }
                       
        }

        private async void DoAddRow(object obj)
        {
            CatchDataCollection.Add(new CatchObject());
        }
    }
}
