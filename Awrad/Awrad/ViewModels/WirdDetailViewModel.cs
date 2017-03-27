using Awrad.Models;

namespace Awrad.ViewModels
{
    public class WirdDetailViewModel : BaseViewModel
    {
        public Wird Wird { get; set; }
        public WirdDetailViewModel(Wird wird = null)
        {
            Title = wird.Description;
            Wird = wird;
        }
    }
}