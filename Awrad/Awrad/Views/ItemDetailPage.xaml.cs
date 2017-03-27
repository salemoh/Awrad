﻿
using Awrad.ViewModels;

using Xamarin.Forms;

namespace Awrad.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        WirdDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ItemDetailPage()
        {
            InitializeComponent();
        }

        public ItemDetailPage(WirdDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
    }
}
