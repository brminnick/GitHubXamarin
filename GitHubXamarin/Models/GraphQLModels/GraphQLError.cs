using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitHubXamarin
{
    class GraphQLError
    {
        public GraphQLError(string message, GraphQLLocation[] locations, IDictionary<string, JToken> additonalEntries = null)
        {
            Message = message;
            Locations = locations;
            AdditonalEntries = additonalEntries;
        }

        [JsonProperty("message")]
        public string Message { get; }

        [JsonProperty("locations")]
        public GraphQLLocation[] Locations { get; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditonalEntries { get; }
    }

    class GraphQLLocation
    {
        [JsonProperty("line")]
        public long Line { get; }

        [JsonProperty("column")]
        public long Column { get; }
    }
}
