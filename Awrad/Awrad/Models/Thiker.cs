using SQLite;

namespace Awrad.Models
{
    public class Thiker
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
                
        public int WirdType { get; set; } // Type of wird this thiker belongs to        
        public int Type { get; set; } // Is this a Quran or Sunnah thiker 
        public int Time { get; set; } // When do we recite this thiker Any, Morning, Evening
        public int Iterations { get; set; } // Average interations
        public int MinIerations { get; set; } // Minimum number of iterations
        public int MaxIterations { get; set; } // Maximum iterations
        public int Size { get; set; } // Shirt size of the thiker. Small, Med, Large
        
        public string Content { get; set; } // Content of the thiker
        public string Referrence { get; set; } // The reference to a thiker
        public string Virtue { get; set; } // Virtue gained by reciting this thiker
    }
}

