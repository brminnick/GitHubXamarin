using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class GitHubUserResponse
    {
        public GitHubUserResponse(User user) => User = user;

        [JsonProperty("user")]
        public User User { get; }
    }
}
