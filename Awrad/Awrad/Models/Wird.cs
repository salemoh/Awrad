using SQLite;

namespace Awrad.Models
{
    public class Wird
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Description { get; set; } // Description of the wird
        public string Introduction { get; set; } // Wird intro
        public string Summary { get; set; } // Summary of the wird
        public string Accent { get; set; } // Location of image accents to display as border for wird
    }
}

