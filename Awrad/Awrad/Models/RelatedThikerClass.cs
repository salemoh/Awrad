using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Awrad.Models
{
    [Table("RelatedThiker")]
    public class RelatedThikerClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Wird { get; set; } // The wird we are looking for related thiker for
        public int RelatedWird { get; set; } // The wird from which we want to get extra thiker
        public int ThikerSize { get; set; } // Either select 0=small thiker, 1=med thiker (includes 0), or 2=large (includes 0,1)
        public int CurrentThiker { get; set; } // The current thiker count we want to select
        public String Description { get; set; } // Description of what this related wird is about
    }
}
