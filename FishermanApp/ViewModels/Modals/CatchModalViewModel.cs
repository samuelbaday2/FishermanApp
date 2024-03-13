using FishermanApp.Objects.DbObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels.Modals
{
    public class CatchModalViewModel : BaseViewModel
    {
        private ObservableCollection<DBCatchObject> _catchDataCollection;

        public ObservableCollection<DBCatchObject> CatchDataCollection { get { return _catchDataCollection; } set { SetProperty(ref _catchDataCollection, value); } }
   
        public ICommand CloseCommand { private set; get; }
        public CatchModalViewModel() 
        {
            CloseCommand = new Command(DoClose);
    
            CatchDataCollection = new ObservableCollection<DBCatchObject>();
        }
        public async Task Initialize(int tripId) 
        {
            CatchDataCollection = new ObservableCollection<DBCatchObject>();
            List<DBCatchObject> list = new List<DBCatchObject>();

            list = await _catchTable.GetItemsAsync();
            CatchDataCollection = new ObservableCollection<DBCatchObject>(list.Where(x => x.TripId == tripId).ToList());
        }
        private void DoClose(object obj)
        {
            Shell.Current.Navigation.PopModalAsync();
        }
    }
}
