using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awrad.Models;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace Awrad.Helpers
{
    /// <summary>
    /// An instance of this class is used to count thiker
    /// </summary>
    class ThikerCounter
    {
        public ThikerClass Thiker { get; private set; }
        public Grid Grid { get; set; }

        public int CurrentIteration { get; private set; }

        /// <summary>
        /// Gets the correct image corresponding to the current iteration
        /// </summary>
        public string HandImage => Constants.HandSequence[CurrentIteration % Constants.HandSequence.Length];

        /// <summary>
        /// Label for the current iteration
        /// </summary>
        public Label CounterLabel => new Label
        {
            Text = CurrentIteration.ToString(),
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            HorizontalOptions = LayoutOptions.FillAndExpand,
            HorizontalTextAlignment = TextAlignment.End
        };



        public ThikerCounter(ThikerClass thiker, Grid grid)
        {
            Thiker = thiker;
            Grid = grid;
            CurrentIteration = 0;
        }

        /// <summary>
        /// This increments the current thiker iteration and updates UI
        /// </summary>
        public void IncrementIteration()
        {
            // Increment the current iteration
            CurrentIteration++;
            int nextHandIndex = (CurrentIteration % Constants.HandSequence.Length);

            // Update the grid UI with the new lable and hand
            Device.BeginInvokeOnMainThread(() =>
            {
                // Update label
                var label = Grid.Children[1] as Label;
                if (label != null)
                {
                    label.Text = CurrentIteration.ToString() + "\\" + Thiker.Iterations;
                }

                // Hide current hand
                var currentHand = Grid.Children[2] as CachedImage;
                if (currentHand != null)
                    currentHand.Source = Constants.HandSequence[nextHandIndex];

            });
        }

    }
}
