using Awrad.Services;
using Awrad.Views;
using Awrad.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Awrad
{
    public partial class App : Application
    {
        private static AwradDatabase database;

        public App()
        {
            InitializeComponent();

            SetMainPage();
        }

        public static AwradDatabase Database => database ??
                                                (database =
                                                    new AwradDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("Awrad.sqlite")));

        public static void SetMainPage()
        {
            // Since no RTL we manually align pages right to left
            Current.MainPage = new NavigationPage(new ItemsPage());

            //Current.MainPage = new TabbedPage
            //{
            //    Children =
            //    {
            //        new NavigationPage(new AboutPage())
            //        {
            //            Title = "المفضلة",
            //            Icon = Device.OnPlatform("tab_about.png",null,null)
            //        },
            //        new NavigationPage(new ItemsPage())
            //        {
            //            Title = "الأوراد",
            //            Icon = Device.OnPlatform("tab_feed.png",null,null)
            //        }
            //    }
            //};

            // Set default page to be the award page
            //var tabbed = Current.MainPage as TabbedPage;
            //tabbed.CurrentPage = tabbed.Children[1];
        }
    }
}
