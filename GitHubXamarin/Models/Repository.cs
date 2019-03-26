using System.Text;
using Newtonsoft.Json;
using System;

namespace GitHubXamarin
{
    public class Repository
    {
        public Repository() { }

        [JsonConstructor]
        public Repository(StarGazers starGazers) => StarGazers = starGazers;

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("forkCount")]
        public long ForkCount { get; set; }

        [JsonProperty("owner")]
        public RepositoryOwner Owner { get; set; }

        [JsonProperty("issues")]
        public IssuesConnection Issues { get; set; }

        public int StarCount
        {
            get => StarGazers?.TotalCount ?? 0;
            set
            {
                if (StarGazers is null)
                    StarGazers = new StarGazers { TotalCount = value };
                else
                    StarGazers.TotalCount = value;
            }
        }

        [JsonProperty("stargazers")]
        StarGazers StarGazers { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(Name)}: {Name}");
            stringBuilder.AppendLine($"{nameof(Owner)} {nameof(Owner.Login)}: {Owner?.Login}");
            stringBuilder.AppendLine($"{nameof(Owner)} {nameof(Owner.AvatarUrl)}: {Owner?.AvatarUrl}");
            stringBuilder.AppendLine($"{nameof(StarCount)}: {StarCount}");
            stringBuilder.AppendLine($"{nameof(Description)}: {Description}");
            stringBuilder.AppendLine($"{nameof(ForkCount)}: {ForkCount}");
            stringBuilder.AppendLine($"{nameof(Issues)}Count: {Issues?.IssueList.Count}");

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
