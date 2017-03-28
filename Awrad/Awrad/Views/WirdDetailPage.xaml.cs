using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            //if (viewModel.Wird.Thiker == null || viewModel.Wird.Thiker.Count == 0)
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

            // Populate the Introduction page
            var IntroductionPage = new ContentPage
            {
                Padding = padding,
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = viewModel.Wird.Introduction,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HorizontalTextAlignment = TextAlignment.End
                        },
                    }
                }
            };

            // Populate the summary page
            var SummaryPage = new ContentPage
            {
                Padding = padding,
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = viewModel.Wird.Summary,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HorizontalTextAlignment = TextAlignment.End
                        },
                    }
                }
            };

            // Populate the thiker pages
            var thikerPages = new List<ContentPage>();
            foreach (var thiker in viewModel.Wird.Thiker)
            {
                // Populate the summary page
                var ThikerPage = new ContentPage
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
                thikerPages.Add(ThikerPage);
            }

            // Add pages in proper RTL order
            Children.Add(SummaryPage);
            foreach (var thikerPage in Enumerable.Reverse(thikerPages))
            {
                // Add the pages in reverse order
                Children.Add(thikerPage);
            }
            Children.Add(IntroductionPage);

            // Set the current page to the last page
            if (Children.Count > 0)
            {
                CurrentPage = Children[Children.Count - 1];
            }
        }
    }
}