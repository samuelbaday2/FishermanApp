using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.DbObjects
{
    public class CatchObject
    {
        [AutoIncrement]
        public int Index {  get; set; }
        public string Species { get; set; }
        public string Quantity { get; set; }
        public string ScientificName { get; set; }
    }
}
