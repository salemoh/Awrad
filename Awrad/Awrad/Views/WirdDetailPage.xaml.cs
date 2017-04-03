using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awrad.Helpers;
using Awrad.Models;
using Awrad.ViewModels;
using FFImageLoading.Forms;
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

        protected override async void OnAppearing()
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

        private void PopulateWirdPages()
        {
            // Border padding
            var padding = new Thickness(Device.OnPlatform(20, 20, 0), Device.OnPlatform(20, 20, 0),
                Device.OnPlatform(20, 20, 0), Device.OnPlatform(20, 20, 0));

            // Create WirdClass specific pages
            var introductionPage = GetRtlContentPage(padding, viewModel.Wird.Introduction);
            var summaryPage = GetRtlContentPage(padding, viewModel.Wird.Summary);

            // Populate the thiker pages
            var thikerPages = new List<ContentPage>();
            foreach (var thiker in viewModel.Wird.Thiker)
            {
                // Populate the summary page
                var thikerPage = GetRtlCountingPage(padding, thiker);

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

            // Set the current page to the last page
            if (Children.Count > 0)
            {
                CurrentPage = Children[Children.Count - 1];
            }
        }

        #region Create Pages

        /// <summary>
        /// Create a content page to be used for intro and sum
        /// </summary>
        /// <param name="padding"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private ContentPage GetRtlContentPage(Thickness padding, string content)
        {
            var contentPage = new ContentPage
            {
                Padding = padding,
                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new Label
                            {
                                Text = content,
                                FontSize = 36,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HorizontalTextAlignment = TextAlignment.End,
                                FontFamily = Device.OnPlatform(
                                    "Arabic Typesetting",
                                    "arabtype.ttf#Arabic Typesetting",
                                    null)
                            },
                        }
                    }
                }
            };
            return contentPage;
        }


        /// <summary>
        /// Generates a counting page for a thiker
        /// </summary>
        /// <param name="padding"></param>
        /// <param name="thiker"></param>
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
                FontSize = 36,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.End,
                FontFamily = Device.OnPlatform(
                    "Arabic Typesetting",
                    "arabtype.ttf#Arabic Typesetting",
                    null)
            };

            var counterLabel = new Label
            {
                Text = "0\\" + thiker.Iterations,
                FontSize = 48,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(viewModel.Wird.Accent.Substring(1))
            };

            // Align the content and counting elements
            grid.Children.Add(contentLabel, 0, 0);
            Grid.SetColumnSpan(contentLabel, 2);
            grid.Children.Add(counterLabel, 0, 1);

            // Add the image for the hand
            var handImage = new CachedImage
            {
                Source = Constants.HandSequence[0],
                DownsampleWidth = 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            grid.Children.Add(handImage, 1, 1);

            // Local method to be hooked to tap
            void OnTapThiker(ThikerCounter thikerCounter) => thikerCounter.IncrementIteration();
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

        #endregion
    }
}