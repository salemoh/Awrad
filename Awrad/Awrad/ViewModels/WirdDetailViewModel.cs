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
        public Wird Wird { get; set; }
        public Command LoadThikerCommand { get; set; }

        public WirdDetailViewModel(Wird wird = null)
        {
            Wird = wird;
            Title = wird.Description;

            // Populate the thiker for this Wird
            try
            {
                Wird.Thiker = App.Database.GetThikerAsync(Wird.Id).Result;
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

        // Populate the thiker for a specific wird
        async Task ExecuteLoadThikerCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

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
            finally
            {
                IsBusy = false;
            }
        }
    }
}