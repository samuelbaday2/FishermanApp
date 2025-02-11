using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.DbObjects
{
    public class DBTripCrewObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int TripId { get; set; }
        public string CrewFirstname { get; set; } = string.Empty;
        public string CrewLastname { get; set; } = string.Empty;
        public DateTime RecordedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
