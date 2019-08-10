using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Refit;
using Polly;

namespace GitHubXamarin
{
    static class GitHubGraphQLService
    {
        readonly static Lazy<IGitHubAPI> _githubApiClientHolder = new Lazy<IGitHubAPI>(() => RestService.For<IGitHubAPI>(GitHubConstants.APIUrl));

        static IGitHubAPI GitHubApiClient => _githubApiClientHolder.Value;

        public static async Task<User> GetUser(string username)
        {
            var token = await GitHubSettings.GetToken().ConfigureAwait(false);
            var data = await ExecuteGraphQLRequest(() => GitHubApiClient.UserQuery(new UserQueryContent(username), GetBearerTokenHeader(token))).ConfigureAwait(false);

            return data.User;
        }

        public static async Task<Repository> GetRepository(string repositoryOwner, string repositoryName, int numberOfIssuesPerRequest = 100)
        {
            var token = await GitHubSettings.GetToken().ConfigureAwait(false);
            var data = await ExecuteGraphQLRequest(() => GitHubApiClient.RepositoryQuery(new RepositoryQueryContent(repositoryOwner, repositoryName, numberOfIssuesPerRequest), GetBearerTokenHeader(token))).ConfigureAwait(false);

            return data.Repository;
        }

        public static async Task<List<Repository>> GetRepositories(string repositoryOwner, int numberOfRepositoriesPerRequest = 100)
        {
            RepositoryConnection repositoryConnection = null;

            List<Repository> gitHubRepositoryList = new List<Repository>();

            do
            {
                repositoryConnection = await GetRepositoryConnection(repositoryOwner, repositoryConnection?.PageInfo?.EndCursor, numberOfRepositoriesPerRequest).ConfigureAwait(false);
                gitHubRepositoryList.AddRange(repositoryConnection?.RepositoryList);
            }
            while (repositoryConnection?.PageInfo?.HasNextPage is true);

            return gitHubRepositoryList;
        }

        static async Task<RepositoryConnection> GetRepositoryConnection(string repositoryOwner, string endCursor, int numberOfRepositoriesPerRequest = 100)
        {
            var token = await GitHubSettings.GetToken().ConfigureAwait(false);
            var data = await ExecuteGraphQLRequest(() => GitHubApiClient.RepositoryConnectionQuery(new RepositoryConnectionQueryContent(repositoryOwner, GetEndCursorString(endCursor), numberOfRepositoriesPerRequest), GetBearerTokenHeader(token))).ConfigureAwait(false);

            return data.GitHubUser.RepositoryConnection;
        }

        static async Task<T> ExecuteGraphQLRequest<T>(Func<Task<GraphQLResponse<T>>> action, int numRetries = 2)
        {
            var response = await Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action).ConfigureAwait(false);

            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data;

            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }

        static string GetEndCursorString(string endCursor) => string.IsNullOrWhiteSpace(endCursor) ? string.Empty : "after: \"" + endCursor + "\"";
        static string GetBearerTokenHeader(string githubApiToken) => $"bearer {githubApiToken}";
    }
}
