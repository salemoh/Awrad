using System;
using System.Collections.Generic;

using Awrad.Models;
using Awrad.ViewModels;

using Xamarin.Forms;

namespace Awrad.Views
{
    public partial class ItemsPage : ContentPage
    {
        // Give the user up to 60 minutes before resetting the thiker page
        private const int WirdTimespanIncrement = 60;

        // Used to load all wirds
        private readonly WirdViewModel _wirdViewModel;

        // Saves the main page bar color
        public Color? MainPageBarColor { get; set; }
        public Dictionary<int, WirdDetailPage> WirdPagesDictionary { get; private set; }

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _wirdViewModel = new WirdViewModel();

            // Set the color to NULL
            MainPageBarColor = null;

            // Create pages dictionary
            WirdPagesDictionary = new Dictionary<int, WirdDetailPage>();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var wird = args.SelectedItem as WirdClass;
            if (wird == null)
                return;

            // Push the page on the navigation stack
            WirdDetailPage wirdDetailPage = null;

            // Compare last access time with now
            int result = DateTime.Compare(DateTime.UtcNow, wird.LastAccessTimestamp.AddMinutes(WirdTimespanIncrement));

            // Keep a list of wirds that were created earlier
            if (!WirdPagesDictionary.ContainsKey(wird.Id) || result > 0)
            {
                wirdDetailPage = new WirdDetailPage(new WirdDetailViewModel(wird));
                WirdPagesDictionary[wird.Id] = wirdDetailPage;
                wird.LastAccessTimestamp = DateTime.UtcNow;
                await App.Database.SaveWirdAsync(wird);

            }
            else if (WirdPagesDictionary.ContainsKey(wird.Id))
            {
                wirdDetailPage = WirdPagesDictionary[wird.Id];
                wird.LastAccessTimestamp = DateTime.UtcNow;
                await App.Database.SaveWirdAsync(wird);
            }

            await Navigation.PushAsync(wirdDetailPage);

            // Set the navigation bar color
            var mainPage = Application.Current.MainPage as NavigationPage;
            if (mainPage != null)
            {
                MainPageBarColor = mainPage.BarBackgroundColor;
                mainPage.BarBackgroundColor = Color.FromHex(wird.Accent.Substring(1));
            }

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_wirdViewModel.Wirds.Count == 0)
                _wirdViewModel.LoadItemsCommand.Execute(null);

            // Reset the color of bar to blue
            var mainPage = Application.Current.MainPage as NavigationPage;
            if (mainPage != null)
            {
                if (MainPageBarColor == null)
                {
                    // We have not yet set the main page color so copy it
                    MainPageBarColor = mainPage.BarBackgroundColor;
                }
                mainPage.BarBackgroundColor = MainPageBarColor.Value;
            }

        }
    }
}
