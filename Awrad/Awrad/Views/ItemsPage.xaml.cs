using System;

using Awrad.Models;
using Awrad.ViewModels;

using Xamarin.Forms;

namespace Awrad.Views
{
    public partial class ItemsPage : ContentPage
    {
        WirdViewModel viewModel;
        public Color? MainPageBarColor { get; set; }

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new WirdViewModel();

            // Set the color to NULL
            MainPageBarColor = null;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as WirdClass;
            if (item == null)
                return;

            // Push the page on the navigation stack
            await Navigation.PushAsync(new WirdDetailPage(new WirdDetailViewModel(item)));

            // Set the navigation bar color
            var mainPage = App.Current.MainPage as NavigationPage;
            if (mainPage != null)
            {
                MainPageBarColor = mainPage.BarBackgroundColor;
                mainPage.BarBackgroundColor = Color.FromHex(item.Accent.Substring(1));
            }

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Wirds.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            // Reset the color of bar to blue
            var mainPage = App.Current.MainPage as NavigationPage;
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
