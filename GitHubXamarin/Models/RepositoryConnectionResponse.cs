using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class RepositoryConnectionResponse
    {
        public RepositoryConnectionResponse(User user) => GitHubUser = user;

        [JsonProperty("user")]
        public User GitHubUser { get; }
    }
}
