using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awrad.Helpers;
using Xamarin.Forms;

namespace Awrad.Views
{
    public class RtlTitleContentPage : ContentPage
    {
        public RtlTitleContentPage(Thickness padding, string content,
            bool quran = false)
        {
            // Extract the tile which is the first line of the content
            var titleLocation = content.IndexOf(Environment.NewLine, StringComparison.Ordinal);
            var title = content.Substring(0, titleLocation);
            var contentNoTitle = content.Substring(titleLocation + 1);
            Padding = padding;

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = title,
                            FontSize = Constants.TitleFontSize,
                            TextColor = Color.Brown,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontFamily = Constants.ContentFontFamilyName
                        },
                        new Label
                        {
                            Text = contentNoTitle,
                            FontSize = quran ? Constants.QuranContentFontSize : Constants.ContentFontSize,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HorizontalTextAlignment = TextAlignment.End,
                            FontFamily = quran ? Constants.QuranFontFamilyName : Constants.ContentFontFamilyName
                        },
                    }
                }
            };
        }
    }
}