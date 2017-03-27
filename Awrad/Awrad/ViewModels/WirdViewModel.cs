using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Awrad.Helpers;
using Awrad.Models;
using Awrad.Views;

using Xamarin.Forms;

namespace Awrad.ViewModels
{
    public class WirdViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Wird> Wirds { get; set; }
        public Command LoadItemsCommand { get; set; }

        public WirdViewModel()
        {
            Title = "أوراد اليوم والليلة";
            Wirds = new ObservableRangeCollection<Wird>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
                //var _item = item as Item;
                //Wirds.Add(_item);
                //await DataStore.AddItemAsync(_item);
            //});
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Wirds.Clear();
                var wirds = await App.Database.GetAwradAsync();
                Wirds.ReplaceRange(wirds);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load wirds.",
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