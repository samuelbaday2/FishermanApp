using FishermanApp.Objects;
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
    public class GenericSelectionViewModel : SelectionBaseViewModel
    {
        public ICommand OnSelectCommand { private set; get; }
        public GenericSelectionViewModel() 
        {
            SelectionList = new List<SelectionObject>();

            OnSelectCommand = new Command(DoSelect);

            SelectionCollection = new ObservableCollection<SelectionObject>();
        }

        public async Task InitializeAsync()
        {
            SelectionCollection = new ObservableCollection<SelectionObject>();
            
            SelectionCollection.Add(new SelectionObject { 
                 SelectionTitle = "shadhasjhdhas",
            });
            SelectionCollection.Add(new SelectionObject
            {
                SelectionTitle = "shadhasjhdhas",
            });
            SelectionCollection.Add(new SelectionObject
            {
                SelectionTitle = "shadhasjhdhas",
            });
        }

        private void DoSelect(object obj)
        {
            SelectionObject SelectedObject = obj as SelectionObject;

            //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.CatchDetails)).FirstOrDefault();

          
        }
    }
}
