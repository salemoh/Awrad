using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awrad.Helpers;
using Awrad.Models;
using Awrad.ViewModels;
using FFImageLoading.Forms;
using Xamarin.Forms;
using static System.String;

namespace Awrad.Views
{
    public partial class WirdDetailPage
    {
        private readonly WirdDetailViewModel _viewModel;

        private readonly string _fontFamily;

        private const int ContentFontSize = 32;
        private const int TitleFontSize = 40;

        private const int RelatedThikerSize = 0;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public WirdDetailPage()
        {
            InitializeComponent();
        }

        public WirdDetailPage(WirdDetailViewModel wirdDetailViewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = wirdDetailViewModel;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    _fontFamily = "Arabic Typesetting";
                    break;

                case Device.Android:
                    _fontFamily = "arabtype.ttf#Arabic Typesetting";
                    break;
            }

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // If not yet, load the thiker from DB on appearing
            //if (viewModel.WirdClass.ThikerClass == null || viewModel.WirdClass.ThikerClass.Count == 0)
            //    viewModel.LoadThikerCommand.Execute(null);

            // If we have children don't populate again
            if (Children.Count > 0)
            {
                return;
            }

            // Populate the pages
            await Task.Run(async () =>
            {
                var tcs = new TaskCompletionSource<bool>();

                // Lets await to fill the thiker
                await _viewModel.PopulateThiker();

                // Lets await to fill the related thiker if such thiker is present
                if(_viewModel.Wird.RelatedThiker == "Y")
                {
                    await _viewModel.PopulateRelatedThiker(RelatedThikerSize);
                }

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
            double paddingValue = 0;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    paddingValue = 20;
                    break;
            }

            // Border padding
            var padding = new Thickness(paddingValue, paddingValue, paddingValue, paddingValue);

            // Populate the thiker pages
            var thikerPages = new List<ContentPage>();
            foreach (var thiker in _viewModel.Wird.ThikerList)
            {
                // We only publish a counting page if the Iterations > 1
                var thikerPage = thiker.Iterations > 1 ? GetRtlCountingPage(padding, thiker) : 
                    GetRtlContentPage(padding, thiker.Content);

                // Add page to list
                thikerPages.Add(thikerPage);
            }

            // Add the summary page
            if (!IsNullOrWhiteSpace(_viewModel.Wird.Summary))
            {
                var summaryPage = GetRtlContentPage(padding, _viewModel.Wird.Summary);
                Children.Add(summaryPage);
            }

            // Add pages in proper RTL order
            foreach (var thikerPage in Enumerable.Reverse(thikerPages))
            {
                // Add the pages in reverse order
                Children.Add(thikerPage);
            }

            // If there are related thiker add in proper RTL order
            if (_viewModel.Wird.RelatedThiker != null)
            {
                foreach (var relatedThiker in Enumerable.Reverse(_viewModel.Wird.RelatedThikerList))
                {
                    // We only publish a counting page if the Iterations > 1
                    var thikerPage = relatedThiker.Iterations > 1 ? GetRtlCountingPage(padding, relatedThiker) :
                        GetRtlTitleContentPage(padding, relatedThiker.Content);

                    // Add to the pages in reverse order
                    Children.Add(thikerPage);
                }
            }

            // If there is an introduction page add it
            if (!IsNullOrWhiteSpace(_viewModel.Wird.Introduction))
            {
                var introductionPage = GetRtlContentPage(padding, _viewModel.Wird.Introduction);
                Children.Add(introductionPage);
            }

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
                                FontSize = ContentFontSize,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HorizontalTextAlignment = TextAlignment.End,
                                FontFamily = _fontFamily
                            },
                        }
                    }
                }
            };
            return contentPage;
        }

        /// <summary>
        /// Create a content page to be used for intro and sum
        /// </summary>
        /// <param name="padding"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private ContentPage GetRtlTitleContentPage(Thickness padding, string content)
        {
            // Extract the tile which is the first line of the content
            var titleLocation = content.IndexOf(Environment.NewLine, StringComparison.Ordinal);
            var title = content.Substring(0, titleLocation);
            var contentNoTitle = content.Substring(titleLocation + 1);
            
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
                                Text = title,
                                FontSize = TitleFontSize,
                                TextColor = Color.Brown,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HorizontalTextAlignment = TextAlignment.Center,
                                FontFamily = _fontFamily
                            },
                            new Label
                            {
                                Text = contentNoTitle,
                                FontSize = ContentFontSize,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HorizontalTextAlignment = TextAlignment.End,
                                FontFamily = _fontFamily
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
                FontSize = ContentFontSize,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.End,
                FontFamily = _fontFamily
            };

            var counterLabel = new Label
            {
                Text = "0\\" + thiker.Iterations,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(_viewModel.Wird.Accent.Substring(1))
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