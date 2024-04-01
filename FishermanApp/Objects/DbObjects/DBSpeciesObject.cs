using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.DbObjects
{
    public class DBSpeciesObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Species { get; set; }
        public string ScientificName { get; set; }
        public DateTime EditedOn { get; set; }
        public bool IsUploaded { get; set; } = false;
    }
}
