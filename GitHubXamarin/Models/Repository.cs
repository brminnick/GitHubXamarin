using System.Text;
using Newtonsoft.Json;
using System;

namespace GitHubXamarin
{
    public class Repository
    {
        public Repository()
        {

        }

        [JsonConstructor]
        public Repository(StarGazers stargazers, IssuesConnection issues) =>
            (StarCount, IssuesCount) = (stargazers?.TotalCount ?? 0, issues.IssuesCount);

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("forkCount")]
        public long ForkCount { get; set; }

        [JsonProperty("owner")]
        public RepositoryOwner Owner { get; set; }

        [JsonProperty("issues")]
        public int IssuesCount { get; set; }

        [JsonProperty("url")]
        public Uri Uri { get; set; }

        public int StarCount { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(Name)}: {Name}");
            stringBuilder.AppendLine($"{nameof(Owner)} {nameof(Owner.Login)}: {Owner?.Login}");
            stringBuilder.AppendLine($"{nameof(Owner)} {nameof(Owner.AvatarUrl)}: {Owner?.AvatarUrl}");
            stringBuilder.AppendLine($"{nameof(StarCount)}: {StarCount}");
            stringBuilder.AppendLine($"{nameof(Description)}: {Description}");
            stringBuilder.AppendLine($"{nameof(ForkCount)}: {ForkCount}");
            stringBuilder.AppendLine($"{nameof(IssuesCount)}: {IssuesCount}");

            return stringBuilder.ToString();
        }

    }

    public class RepositoryOwner
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("avatarUrl")]
        public Uri AvatarUrl { get; set; }
    }

    public class StarGazers
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
