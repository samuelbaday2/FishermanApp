using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Objects.DbObjects
{
    public class DBCaptainObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CaptainName { get; set; }
        public DateTime RecordedOn { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
