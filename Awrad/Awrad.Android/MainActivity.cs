using Android.App;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using FFImageLoading;
using FFImageLoading.Config;
using FFImageLoading.Forms.Droid;

namespace Awrad.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = 
        ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            CarouselViewRenderer.Init();

            CachedImageRenderer.Init(true);

            ImageService.Instance.Initialize(new Configuration { VerboseLogging = true });

            LoadApplication(new App());
        }
    }
}