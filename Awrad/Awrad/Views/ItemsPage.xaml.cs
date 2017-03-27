using System;

using Awrad.Models;
using Awrad.ViewModels;

using Xamarin.Forms;

namespace Awrad.Views
{
    public partial class ItemsPage : ContentPage
    {
        WirdViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new WirdViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Wird;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new WirdDetailViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        //async void AddItem_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new NewItemPage());
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Wirds.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
