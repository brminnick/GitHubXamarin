using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GitHubXamarin
{
    public static class GitHubSettings
    {
        readonly static string _tokenKey = nameof(_tokenKey);

        public static string User
        {
            get => Preferences.Get(nameof(User), "");
            set => Preferences.Set(nameof(User), value);
        }

        public static Task<string> GetToken() => SecureStorage.GetAsync(_tokenKey);
        public static Task SetToken(string token) => SecureStorage.SetAsync(_tokenKey, token);
    }
}
