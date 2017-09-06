using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awrad.Helpers;
using Xamarin.Forms;

namespace Awrad.Views
{
    /// <inheritdoc />
    /// <summary>
    /// Setup content page with right-to-left
    /// </summary>
    public class RtlContentPage : ContentPage
    {
        public RtlContentPage(Thickness padding, string content,
            bool quran = false)
        {            
            Padding = padding;

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = content,
                            FontSize = Constants.ContentFontSize,
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