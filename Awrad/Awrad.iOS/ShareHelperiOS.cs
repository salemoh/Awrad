using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Awrad.Helpers;
using Awrad.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(ShareHelperiOS))]
namespace Awrad.iOS
{
    class ShareHelperiOS : IShareHelper
    {
        public async void Share(string imageUrl)
        {
            // Get the URI of the image
            var imageUri = new Uri(imageUrl);
            var imageSource = ImageSource.FromUri(imageUri);
            var handler = new ImageLoaderSourceHandler();
            var uiImage = await handler.LoadImageAsync(imageSource);

            var item = NSObject.FromObject(uiImage);
            var activityItems = new[] { item };
            var activityController = new UIActivityViewController(activityItems, null);

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, () => { });
        }
    }
}