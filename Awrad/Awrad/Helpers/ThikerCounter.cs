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
        public ThikerClass Thiker { get; }
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
            // Don't increment if we are done
            if (CurrentIteration >= Thiker.Iterations)
            {
                return;
            }

            // Increment the current iteration
            CurrentIteration++;
            int nextHandIndex = (CurrentIteration % Constants.HandSequence.Length);

            // Update the grid UI with the new lable and hand
            Device.BeginInvokeOnMainThread(() =>
            {
                // Update label
                if (Grid.Children[1] is Label label)
                {
                    label.Text = CurrentIteration.ToString() + "\\" + Thiker.Iterations;
                }

                // Hide current hand
                if (Grid.Children[2] is CachedImage currentHand)
                    currentHand.Source = Constants.HandSequence[nextHandIndex];

            });
        }

    }
}
