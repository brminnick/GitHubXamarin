using Xamarin.Forms;

namespace GitHubXamarin
{
    public class App : Application
    {
        public App() => MainPage = new ContentPage();

        protected override async void OnStart()
        {
            base.OnStart();

            var temp = await GitHubGraphQLService.GetUser("brminnick").ConfigureAwait(false);
            var temp2 = await GitHubGraphQLService.GetRepositories("brminnick").ConfigureAwait(false);
            var temp3 = await GitHubGraphQLService.GetRepository("brminnick", "FaceOff");
        }
    }
}
