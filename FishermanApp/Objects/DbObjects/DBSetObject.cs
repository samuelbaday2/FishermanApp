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
        public string StartSetLatitude { get; set; } = string.Empty;
        public string StartSetLongitude { get; set; } = string.Empty;
        public string EndSetLatitude { get; set; } = string.Empty;
        public string EndSetpLongitude { get; set; } = string.Empty;
        public DateTime RecordedOn { get; set; }
        public DateTime SetEndedOn { get; set; }
        public bool SetEnded { get; set; }
        public bool IsActive { get; set; } = true;
        public string LengthOfLongLine { get; set; } = string.Empty;
        public string NumberOfBaskets { get; set; } = string.Empty;
        public string HooksPerBasket { get; set; } = string.Empty;
        public string TypeOfHook { get; set; } = string.Empty;
        public string GangionLength { get; set; } = string.Empty;
        public string BaitType { get; set; } = string.Empty;
        public string BaitSpecie { get; set; } = string.Empty;
        public bool HasCatchData { get; set; }
        public DateTime EditedOn { get; set; }
        public bool IsUploaded { get; set; } = false;
        public string UoM { get; set; } = string.Empty;
        public string MinDepth { get; set; } = string.Empty;
        public string MaxDepth { get; set; } = string.Empty;
        public DateTime SetStartedOn { get; set; }
    }
}