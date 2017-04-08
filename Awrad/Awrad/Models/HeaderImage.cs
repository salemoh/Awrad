using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
