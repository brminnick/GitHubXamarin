using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class PageInfo
    {
        [JsonProperty("endCursor")]
        public string EndCursor { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonProperty("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }

        [JsonProperty("startCursor")]
        public string StartCursor { get; set; }
    }
}
