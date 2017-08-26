using SQLite;

namespace Awrad.Models
{
    [Table("Thiker")]
    public class ThikerClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
                
        public int WirdType { get; set; } // Type of wird this thiker belongs to        
        public int Type { get; set; } // Is this a 0=Quran or 1=Sunnah thiker 
        public int Time { get; set; } // When do we recite this thiker 0=Any, 1=Morning, 2=Evening
        public int Iterations { get; set; } // Average iterations
        public int MinIerations { get; set; } // Minimum number of iterations
        public int MaxIterations { get; set; } // Maximum iterations
        public int Size { get; set; } // Shirt size of the thiker. 0=Small, 1=Med, 2=Large
        
        public string Content { get; set; } // Content of the thiker
        public string Referrence { get; set; } // The reference to a thiker
        public string Virtue { get; set; } // Virtue gained by reciting this thiker
        public string Book { get; set; } // The book where this Thiker appeared
        public string Author { get; set; } // The authore of the book
        public string AlternateContent { get; set; } // If there is an alternate to the original content
    }
}