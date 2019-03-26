using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GitHubXamarin
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            var navigationPage = new Xamarin.Forms.NavigationPage(new GitHubRepositoryPage())
            {
                BarBackgroundColor = ColorConstants.DarkBlue,
                BarTextColor = Color.White
            };
            navigationPage.On<iOS>().PrefersLargeTitles();

            MainPage = navigationPage;
        }
    }
}
