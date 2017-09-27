using Xamarin.Forms;

namespace Awrad.Helpers
{
    public class Constants
    {
        public static readonly string[] HandSequence =
         {
            "zero.png", // 0
            "one.png", // 1
            "two.png", // 2
            "three.png", // 3
            "four.png", // 4
            "five.png", // 5
            "four.png", // 6
            "three.png", // 7
            "two.png", // 8
            "one.png", // 9
        };

        public const int ContentFontSize = 32;
        public const int QuranContentFontSize = 24;
        public const int TitleFontSize = 40;
        public const int RelatedThikerSize = 0;
        public const double PaddingValue = 20;

        public static readonly Thickness Padding = new Thickness(PaddingValue, PaddingValue, PaddingValue, PaddingValue);
        public class Android
        {
            public static readonly string ContentFontName = "arabtype.ttf#Arabic Typesetting";
            public static readonly string QuranFontName = "UthmanicHafs.otf#KFGQPC Uthmanic Script HAFS";
        }

        public class iOS
        {
            public static readonly string ContentFontName = "Arabic Typesetting";
            public static readonly string QuranFontName = "KFGQPC Uthmanic Script HAFS";
        }

        public static string ContentFontFamilyName => Device.RuntimePlatform == Device.iOS ? iOS.ContentFontName : 
            Android.ContentFontName;

        public static string QuranFontFamilyName => Device.RuntimePlatform == Device.iOS ? iOS.QuranFontName : 
            Android.QuranFontName;
    }
}