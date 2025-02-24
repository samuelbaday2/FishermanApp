using FishermanApp.Objects.DbObjects;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects
{
    public class TripHistoryObject 
    {
        [Ignore]
        public string TripStartDate 
        {
            get {
                return $"Depart Date: {RecordedOn.ToString("MMM-dd-yyyy")}";
            }
            
        }

        [Ignore]
        public string TripStartTime
        {
            get
            {
                return $"Depart Time: {RecordedOn.ToString("hh:mm tt")}";
            }

        }

        [Ignore]
        public string TripCaptain
        {
            get
            {
                return $"Captain: {Captain}";
            }

        }

        [Ignore]
        public string NumberOfSets
        {
            get;set;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UploadedId { get; set; }
        public int VesselId { get; set; }
        public string StartTripLatitude { get; set; }
        public string StartTripLongitude { get; set; }
        public string EndTripLatitude { get; set; }
        public string EndTripLongitude { get; set; }
        public string Captain { get; set; }
        public bool IsTripEnded { get; set; }
        public DateTime RecordedOn { get; set; }
        public DateTime TripEndedOn { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime EditedOn { get; set; }
        public bool IsUploaded { get; set; } = false;
        public bool IsOnline { get; set; } = false;
        public string FuelCost { get; set; }
        public string FuelAmount { get; set; }
        public string FoodCost { get; set; }
        public string CrewNumber { get; set; }
        public string HomePort { get; set; }
        public string Country { get; set; }
        public DateTime TripStartedOn { get; set; }
    }
}
