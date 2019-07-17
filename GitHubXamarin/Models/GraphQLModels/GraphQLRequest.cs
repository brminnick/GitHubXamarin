﻿using Newtonsoft.Json;

namespace GitHubXamarin
{
    abstract class GraphQLRequest
    {
        protected GraphQLRequest(string query, string variables = null) => (Query, Variables) = (query, variables);

        [JsonProperty("query")]
        public string Query { get; }

        [JsonProperty("variables")]
        public string Variables { get; }
    }
}
