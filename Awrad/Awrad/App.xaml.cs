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
            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Browse",
                        Icon = Device.OnPlatform("tab_feed.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                }
            };
        }
    }
}
