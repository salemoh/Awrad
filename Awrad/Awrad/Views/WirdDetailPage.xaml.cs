using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awrad.Helpers;
using Awrad.Models;
using Awrad.ViewModels;
using Xamarin.Forms;

namespace Awrad.Views
{
    public partial class WirdDetailPage : CarouselPage
    {
        WirdDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public WirdDetailPage()
        {
            InitializeComponent();
        }

        public WirdDetailPage(WirdDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            // Set the navigation to start from last page
            if (Children.Count > 0)
            {
                CurrentPage = Children[Children.Count - 1];
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // If not yet, load the thiker from DB on appearing
            //if (viewModel.WirdClass.ThikerClass == null || viewModel.WirdClass.ThikerClass.Count == 0)
            //    viewModel.LoadThikerCommand.Execute(null);

            // Populate the pages
            await Task.Run(async () =>
            {
                var tcs = new TaskCompletionSource<bool>();

                // Lets await to fill the thiker
                await viewModel.PopulateThiker();

                Device.BeginInvokeOnMainThread(() =>
                {
                    PopulateWirdPages();
                    tcs.SetResult(false);
                });

                return tcs.Task;
            });
        }

        private async void PopulateWirdPages()
        {
            // Border padding
            var padding = new Thickness(Device.OnPlatform(20, 20, 0), Device.OnPlatform(20, 20, 0),
                Device.OnPlatform(20, 20, 0), Device.OnPlatform(20, 20, 0));

            // Create WirdClass specific pages
            var introductionPage = GetRtlContentPage(padding, viewModel.Wird.Introduction);
            var summaryPage = GetRtlContentPage(padding, viewModel.Wird.Summary);
            var countingPage = GetRtlCountingPage(padding, viewModel.Wird.Thiker[0]);

            // Populate the thiker pages
            var thikerPages = new List<ContentPage>();
            foreach (var thiker in viewModel.Wird.Thiker)
            {
                // Populate the summary page
                var thikerPage = new ContentPage
                {
                    Padding = padding,
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new Label
                            {
                                Text = thiker.Content,
                                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HorizontalTextAlignment = TextAlignment.End
                            },
                        }
                    }
                };

                // Add page to list
                thikerPages.Add(thikerPage);
            }

            // Add pages in proper RTL order
            Children.Add(summaryPage);
            foreach (var thikerPage in Enumerable.Reverse(thikerPages))
            {
                // Add the pages in reverse order
                Children.Add(thikerPage);
            }
            Children.Add(introductionPage);
            Children.Add(countingPage);

            // Set the current page to the last page
            if (Children.Count > 0)
            {
                CurrentPage = Children[Children.Count - 1];
            }
        }


        /// <summary>
        /// Generates a counting page for a thiker
        /// </summary>
        /// <param name="padding"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private ContentPage GetRtlCountingPage(Thickness padding, ThikerClass thiker)
        {
            var grid = new Grid();

            // Define our grid columns
            grid.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)});
            grid.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(100)});

            grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});

            var contentLabel = new Label
            {
                Text = thiker.Content,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.End
            };

            var counterLabel = new Label
            {
                Text = "0",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            // Align the content and counting elements
            grid.Children.Add(contentLabel, 0, 0);
            Grid.SetColumnSpan(contentLabel, 2);
            grid.Children.Add(counterLabel, 0, 1);
            grid.Children.Add(new Image {Source = "zero.png"}, 1, 1);

            // Hook up the tap gester for the page
            var tapGesture = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command<ThikerCounter>(OnTapThiker),
                CommandParameter = new ThikerCounter(thiker, grid)
            };
            grid.GestureRecognizers.Add(tapGesture);

            return new ContentPage
            {
                Padding = padding,
                Content = grid
            };
        }

        private void OnTapThiker(ThikerCounter thikerCounter)
        {
            thikerCounter.IncrementIteration();
        }


        private void TapGesture_Tapped(object sender, System.EventArgs e)
        {
            // Get the grid from the sender
            var grid = sender as Grid;

            // Get the image and lable
            var counter = grid.Children[1] as Label;
            var hand = grid.Children[2] as Image;

            // Let us fade out the image
            hand.FadeTo(0, 1000);
        }

        private ContentPage GetRtlContentPage(Thickness padding, string content)
        {
            var IntroductionPage = new ContentPage
            {
                Padding = padding,
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = content,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HorizontalTextAlignment = TextAlignment.End
                        },
                    }
                }
            };
            return IntroductionPage;
        }
    }
}