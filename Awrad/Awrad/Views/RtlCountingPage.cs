using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awrad.Helpers;
using Awrad.Models;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace Awrad.Views
{
    public class RtlCountingPage : ContentPage
    {
        public RtlCountingPage(Thickness padding, ThikerClass thiker, string accent)
        {
            var grid = new Grid();

            // Define our grid columns
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(80) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var contentLabel = new Label
            {
                Text = thiker.Content,
                FontSize = thiker.Type == (int)ThikerTypes.Quran ? Constants.QuranContentFontSize : Constants.ContentFontSize,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.End,
                FontFamily = thiker.Type == (int)ThikerTypes.Quran ? Constants.QuranFontFamilyName : Constants.ContentFontFamilyName
            };

            var counterLabel = new Label
            {
                Text = "0\\" + thiker.Iterations,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(accent.Substring(1))
            };

            // Align the content and counting elements
            grid.Children.Add(contentLabel, 0, 0);
            Grid.SetColumnSpan(contentLabel, 2);
            grid.Children.Add(counterLabel, 0, 1);

            // Add the image for the hand
            var handImage = new CachedImage
            {
                Source = Constants.HandSequence[0],
                //DownsampleWidth = 120,
                //HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.Center,
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

            Padding = padding;
            Content = grid;
        }
    }
}