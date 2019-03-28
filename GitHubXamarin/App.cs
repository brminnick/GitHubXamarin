namespace GitHubXamarin
{
    public class App : Xamarin.Forms.Application
    {
        public App() => MainPage = new BaseNavigationPage(new RepositoryPage());
    }
}
