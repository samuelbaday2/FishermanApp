using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
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
    public class CatchSpeciesSelectionViewModel : SelectionBaseViewModel
    {
        public ICommand OnSelectCommand { private set; get; }
        public CatchSpeciesSelectionViewModel() {
            SelectionList = new List<SelectionObject>();

            OnSelectCommand = new Command(DoSelect);

            SelectionCollection = new ObservableCollection<SelectionObject>();
        }
        public async Task InitializeAsync()
        {
            SelectionCollection = new ObservableCollection<SelectionObject>();
            var SpeciesList = JsonConvert.DeserializeObject<List<DBSpeciesObject>>(Preferences.Get(Pref.CATCH_SPECIES_LIST, string.Empty));
            foreach (var Species in SpeciesList)
            {
                SelectionCollection.Add(new SelectionObject
                {
                    SelectionTitle = Species.Species,
                    SubTitle = $"({Species.ScientificName})",
                });
            }
        }

        private void DoSelect(object obj)
        {
            SelectionObject SelectedObject = obj as SelectionObject;

            Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();

            MessagingCenter.Send(this, AppResources.CatchSpeciesSelection, SelectedObject);

        }
    }
}
