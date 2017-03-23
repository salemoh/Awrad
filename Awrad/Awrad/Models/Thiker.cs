using SQLite;

namespace Awrad.Models
{
    public class Thiker
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
                
        public int WirdType { get; set; } // Type of wird this thiker belongs to        
        public int Type { get; set; } // Is this an Introduction, Quran, Sunnah, Summary thiker 
        public int Order { get; set; } // The order in which the thiker appears in a wird
        public int Time { get; set; } // When do we recite this thiker Any, Morning, Evening
        public int Iterations { get; set; } // Average interations
        public int MinIerations { get; set; } // Minimum number of iterations
        public int MaxIterations { get; set; } // Maximum iterations
        
        public string Content { get; set; } // Content of the thiker
        public string Reference { get; set; } // The reference to a thiker
        public string Virtue { get; set; } // Virtue gained by reciting this thiker
    }
}

