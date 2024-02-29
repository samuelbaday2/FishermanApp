using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.DbObjects
{
    public class DbTripObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int VesselId { get; set; }
        public string StartTripLatitude { get; set; }
        public string StartTripLongitude { get; set; }
        public string EndTripLatitude { get; set; }
        public string EndTripLongitude { get; set; }
        public bool IsTripEnded { get; set; }
        public DateTime RecordedOn { get; set; }
        public DateTime TripEndedOn { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime EditedOn { get; set; }
    }
}
