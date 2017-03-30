using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Awrad.Helpers;
using Awrad.Models;
using Xamarin.Forms;

namespace Awrad.ViewModels
{
    public class WirdDetailViewModel : BaseViewModel
    {
        public WirdClass Wird { get; set; }
        public Command LoadThikerCommand { get; set; }

        public WirdDetailViewModel(WirdClass wird = null)
        {
            Wird = wird;
            Title = wird.Description;
        }

        // Populate the thiker for a specific wird
        public async Task PopulateThiker()
        {
            // Populate the thiker for this wird
            try
            {
                Wird.Thiker = await App.Database.GetThikerAsync(Wird.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load thiker.",
                    Cancel = "OK"
                }, "message");
            }
        }
    }
}