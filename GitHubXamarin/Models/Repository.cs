using System.Text;
using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class Repository
    {
        public Repository() { }

        [JsonConstructor]
        public Repository(Owner owner, StarGazers starGazers) =>
            (RepositoryOwner, StarGazers) = (owner, starGazers);

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("forkCount")]
        public long ForkCount { get; set; }

        [JsonProperty("issues")]
        public Issues Issues { get; set; }

        public string Owner
        {
            get => RepositoryOwner?.Login;
            set
            {
                if (RepositoryOwner is null)
                    RepositoryOwner = new Owner { Login = value };
                else
                    RepositoryOwner.Login = value;
            }
        }

        public int TotalStars
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

        [JsonProperty("owner")]
        Owner RepositoryOwner { get; set; }

        [JsonProperty("stargazers")]
        StarGazers StarGazers { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(Name)}: {Name}");
            stringBuilder.AppendLine($"{nameof(Owner)}: {Owner}");
            stringBuilder.AppendLine($"{nameof(TotalStars)}: {TotalStars}");
            stringBuilder.AppendLine($"{nameof(Description)}: {Description}");
            stringBuilder.AppendLine($"{nameof(ForkCount)}: {ForkCount}");
            stringBuilder.AppendLine($"{nameof(Issues)}Count: {Issues.IssueList.Count}");

            return stringBuilder.ToString();
        }

    }

    public class Owner
    {
        [JsonProperty("login")]
        public string Login { get; set; }
    }

    public class StarGazers
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
