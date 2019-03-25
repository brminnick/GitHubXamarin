using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class RepositoryConnectionResponse
    {
        public RepositoryConnectionResponse(User gitHubUser) => GitHubUser = gitHubUser;

        [JsonProperty("user")]
        public User GitHubUser { get; }
    }
}
