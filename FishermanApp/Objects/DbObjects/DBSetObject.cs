using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.DbObjects
{
    public class DBSetObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UploadedId { get; set; }
        public int TripId { get; set; }
        public string StartSetLatitude { get; set; }
        public string StartSetLongitude { get; set; }
        public string EndSetLatitude { get; set; }
        public string EndSetpLongitude { get; set; }
        public DateTime RecordedOn { get; set; }
        public DateTime SetEndedOn { get; set; }
        public bool SetEnded { get; set; }
        public bool IsActive { get; set; } = true;
        public string LengthOfLongLine { get; set; }
        public string NumberOfBaskets { get; set; }
        public string HooksPerBasket { get; set; }
        public string TypeOfHook { get; set; }
        public string GangionLength { get; set; }
        public string BaitType { get; set; }
        public string BaitSpecie { get; set; }
        public bool HasCatchData { get; set; }
        public DateTime EditedOn { get; set; }
        public bool IsUploaded { get; set; } = false;
    }
}