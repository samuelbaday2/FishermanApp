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
        public string Species { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string ScientificName { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public string ProcessingType { get; set; } = string.Empty;
        public DateTime RecordedOn { get; set; }
        public DateTime EditedOn { get; set; }
        public bool IsUploaded { get; set; } = false;
        public int SetNumber { get; set; }
        public int VesselId { get; set; }
        public string VesselName { get; set; } = string.Empty;
    }
}
