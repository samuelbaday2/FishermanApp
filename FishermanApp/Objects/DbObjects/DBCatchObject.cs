using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.DbObjects
{
    public class DBCatchObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int SetId { get; set; }
        public int TripId { get; set; }
        public int UploadedId { get; set; }
        public bool IsActive { get; set; } = true;
        public string Species { get; set; }
        public string ScientificName { get; set; }
        public string Quantity { get; set; }
        public DateTime RecordedOn { get; set; }
        public DateTime EditedOn { get; set; }
        public bool IsUploaded { get; set; } = false;
        public int SetNumber { get; set; }
    }
}
