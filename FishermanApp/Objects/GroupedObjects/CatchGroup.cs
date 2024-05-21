using FishermanApp.Objects.DbObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.GroupedObjects
{
    public class CatchGroup : ObservableCollection<DBCatchObject>
    {
        public string Name { get; private set; }

        public CatchGroup(string name, ObservableCollection<DBCatchObject> list) : base(list)
        {
            Name = name;
        }
    }
}
