using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.DbObjects
{
    public class DBHACCPObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TripId { get; set; }
        public int UploadedId { get; set; }
        public bool Water { get; set; }
        public bool Cleaning { get; set; }
        public bool Container { get; set; }
        public bool Ice { get; set; }
    }
}
