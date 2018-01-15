
using CarouselView.FormsPlugin.iOS;
using FFImageLoading;
using FFImageLoading.Config;
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;

namespace Awrad.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

            CarouselViewRenderer.Init();

            CachedImageRenderer.Init();

		    ImageService.Instance.Initialize(new Configuration{VerboseLogging = true});

            LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
