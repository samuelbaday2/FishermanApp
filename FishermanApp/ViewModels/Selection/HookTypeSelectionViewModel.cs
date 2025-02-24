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
    public class HookTypeSelectionViewModel : SelectionBaseViewModel
    {
        public ICommand OnSelectCommand { private set; get; }
        public HookTypeSelectionViewModel() 
        {
            SelectionList = new List<SelectionObject>();
         
            OnSelectCommand = new Command(DoSelect);


            SelectionCollection = new ObservableCollection<SelectionObject>();
      
        }
        public async Task InitializeAsync()
        {

            SelectionCollection = new ObservableCollection<SelectionObject>
            {
                new SelectionObject
                {
                    SelectionTitle = "J Hook",

                },
                new SelectionObject
                {
                    SelectionTitle = "J Hook (Modified)",

                },
                new SelectionObject
                {
                    SelectionTitle = "Circle Hook (in line)",

                },
                new SelectionObject
                {
                    SelectionTitle = "Circle Hook (off set)",

                }
            };

        }

        private async void DoSelect(object obj)
        {
            SelectionObject SelectedObject = obj as SelectionObject;


            //Shell.Current.CurrentItem = Shell.Current.Items.Where(x => x.Title.Contains(AppResources.SetDetails)).FirstOrDefault();
            await Shell.Current.Navigation.PopAsync();

            MessagingCenter.Send<HookTypeSelectionViewModel, SelectionObject>(this, AppResources.HookType, SelectedObject);
        }
    }
}
