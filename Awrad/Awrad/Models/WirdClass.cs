using System;
using System.Collections.Generic;
using SQLite;

namespace Awrad.Models
{
    [Table("Wird")]
    public class WirdClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Description { get; set; } // Description of the wird
        public string Introduction { get; set; } // WirdClass intro
        public string Summary { get; set; } // Summary of the wird
        public string Accent { get; set; } // Location of image accents to display as border for wird
        [Ignore]
        public List<ThikerClass> Thiker { get; set; } // The list of thiker as part of this WirdClass       
        public DateTime LastAccessTimestamp { get; set; } // Last time this Wird was accessed
    }
}

