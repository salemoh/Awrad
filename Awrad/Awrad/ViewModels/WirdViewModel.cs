using System;
using System.Collections.ObjectModel;
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
        public ObservableRangeCollection<WirdClass> Wirds { get; set; }
        public ObservableCollection<HeaderImage> HeaderImages { get; set; }


        public Command LoadItemsCommand { get; set; }

        public Command TapShare { get; set; }

        public WirdViewModel()
        {
            Title = "أوراد اليوم والليلة";
            Wirds = new ObservableRangeCollection<WirdClass>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            TapShare = new Command(OnTappedShare);

            HeaderImages = new ObservableCollection<HeaderImage>
            {
                new HeaderImage
                {
                    ImageUrl = "http://www.sqorebda3.com/vb/Photo/new_1423722305_127.png",
                    Notes = "اللهم صلي وسلم على نبينا محمد",
                    TapShare = TapShare
                },
                new HeaderImage
                {
                    ImageUrl = "http://g.abunawaf.com/2009/8/21/mvib64wcx68u.png",
                    Notes = "رمضان شهر الخير",
                    TapShare = TapShare
                }
            };

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

        void OnTappedShare(object imageUrl)
        {
            DependencyService.Get<IShareHelper>().Share(imageUrl as string);
        }
    }
}