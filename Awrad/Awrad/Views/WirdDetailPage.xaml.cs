using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awrad.Helpers;
using Awrad.ViewModels;
using Xamarin.Forms;
using static System.String;

namespace Awrad.Views
{
    public partial class WirdDetailPage
    {
        private readonly WirdDetailViewModel _viewModel;



        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public WirdDetailPage()
        {
            InitializeComponent();
        }

        public WirdDetailPage(WirdDetailViewModel wirdDetailViewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = wirdDetailViewModel;
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
                if (_viewModel.Wird.RelatedThiker == "Y")
                {
                    await _viewModel.PopulateRelatedThiker(Constants.RelatedThikerSize);
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
            // Populate the thiker pages
            var thikerPages = new List<ContentPage>();
            for (var thikerId = 0; thikerId < _viewModel.Wird.ThikerList.Count; thikerId++)
            {
                var thiker = _viewModel.Wird.ThikerList[thikerId];
                ContentPage thikerPage;
                // For thiker with iteration > 1 we publish one thiker per page

                // We only publish a counting page if the Iterations > 1
                if (thiker.Iterations > 1)
                {
                    thikerPage = new RtlCountingPage(Constants.Padding, thiker, _viewModel.Wird.Accent);
                }
                else
                {
                    // Combine the content if the thiker is part of the same group
                    var content = thiker.Content;
                    var thikerGroup = thiker.ThikerGroup;

                    if (thikerGroup != 0)
                    {
                        while (thikerId < _viewModel.Wird.ThikerList.Count - 1 &&
                            thikerGroup == _viewModel.Wird.ThikerList[thikerId + 1].ThikerGroup)
                        {
                            content += Environment.NewLine + Environment.NewLine +
                                _viewModel.Wird.ThikerList[thikerId + 1].Content;
                            thikerId++;
                        }
                    }

                    thikerPage = new RtlContentPage(Constants.Padding, content, thiker.Type == (int)ThikerTypes.Quran);
                }

                // Add page to list
                thikerPages.Add(thikerPage);
            }

            // Add the summary page
            if (!IsNullOrWhiteSpace(_viewModel.Wird.Summary))
            {
                var summaryPage = new RtlContentPage(Constants.Padding, _viewModel.Wird.Summary);
                Children.Add(summaryPage);
            }

            // Add pages in proper RTL order
            foreach (var thikerPage in Enumerable.Reverse(thikerPages))
            {
                // Add the pages in reverse order
                Children.Add(thikerPage);
            }

            // If there are related thiker add in proper RTL order
            if (_viewModel.Wird.RelatedThiker == "Y")
            {
                foreach (var relatedThiker in Enumerable.Reverse(_viewModel.Wird.RelatedThikerList))
                {
                    // We only publish a counting page if the Iterations > 1
                    var thikerPage = relatedThiker.Iterations > 1 ?
                        (ContentPage)new RtlCountingPage(Constants.Padding, relatedThiker, _viewModel.Wird.Accent) :
                        (ContentPage)new RtlTitleContentPage(Constants.Padding, relatedThiker.Content,
                            relatedThiker.Type == (int)ThikerTypes.Quran);

                    // Add to the pages in reverse order
                    Children.Add(thikerPage);
                }
            }

            // If there is an introduction page add it
            if (!IsNullOrWhiteSpace(_viewModel.Wird.Introduction))
            {
                var introductionPage = new RtlContentPage(Constants.Padding, _viewModel.Wird.Introduction);
                Children.Add(introductionPage);
            }

            // Set the current page to the last page
            if (Children.Count > 0)
            {
                CurrentPage = Children[Children.Count - 1];
            }
        }
    }
}