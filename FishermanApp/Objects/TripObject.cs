using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects
{
    public class TripObject
    {
        public string VesId { get; set; }
        public string FADLocation { get; set; }
        public string FADUse { get; set; }
        public string ActivityId { get; set; }
        public string ActivityType { get; set; }
        public string EventNum { get; set; }
        public string EventType { get; set; }
        public string ItemCode { get; set; }
        public string ItemNum { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public string ProductDestination { get; set; }
        public string TripCaptain { get; set; }
        public string TripCrew { get; set; }
        public List<CatchObject2> Catches { get; set; }
    }
}
