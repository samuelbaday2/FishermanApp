using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects
{
    public class CatchObject
    {
        public string VesId { get; set; }
        public string TripId { get; set; }
        public string BatchLotNum { get; set; }
        public string Bycatch { get; set; }
        public string ItemType { get; set; }
        public string Quantity { get; set; }
        public string WeightOfBatchLot { get; set; }
        public string WeightItem { get; set; }
        public string FirstFreezeDate { get; set; }
        public string EventLocation { get; set; }
        public string Origin { get; set; }
        public string EventDateTime { get; set; }
        public string EventMethod { get; set; }
        public string Temperature { get; set; }
    }
}
