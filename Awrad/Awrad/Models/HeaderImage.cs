using System.Windows.Input;

namespace Awrad.Models
{
    public class HeaderImage
    {
        public string ImageUrl { get; set; }
        public string Notes { get; set; }
        public ICommand TapShare { get; set; }
    }
}
