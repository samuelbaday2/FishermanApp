using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects
{
    public class CrewObject
    {
        public String CrewFirstName { get; set; }
        public String CrewLastName { get; set; }
        public String CrewGender { get; set; }
        public String CrewNationality { get; set; }
        public String CrewJobTitle { get; set; }
        public String CrewDateOfBirth { get; set; }
        public String VesCrewId { get; set; }
        public String CrewUniqueId { get; set; }
        public String VesId { get; set; }
        public String FullName
        {
            get
            {
                return "Name : " + CrewFirstName + " " + CrewLastName;
            }
        }
    }
}
