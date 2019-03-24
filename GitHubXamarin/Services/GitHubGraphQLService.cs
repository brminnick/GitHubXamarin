using System;
using System.Linq;
using System.Threading.Tasks;

using Refit;
using Polly;
using System.Collections.Generic;

namespace GitHubXamarin
{
    static class GitHubGraphQLService
    {
        #region Constant Fields
        readonly static Lazy<IGitHubAPI> _githubApiClientHolder = new Lazy<IGitHubAPI>(() => RestService.For<IGitHubAPI>(GitHubConstants.APIUrl));
        #endregion

        #region Properties
        static IGitHubAPI GitHubApiClient => _githubApiClientHolder.Value;
        #endregion

        #region Methods
        public static async Task<GitHubUser> GetUser(string username)
        {
            var requestString = "query { user(login:" + username + "){ name,company,createdAt, followers{ totalCount }}}";

            var data = await ExecuteGraphQLRequest(() => GitHubApiClient.UserQuery(new GraphQLRequest(requestString))).ConfigureAwait(false);

            return data.User;
        }

        public static async Task<GitHubRepository> GetRepository(string repositoryOwner, string repositoryName, int numberOfIssuesToRequest = 100)
        {
            var requestString = "query { repository(owner:\"" + repositoryOwner + "\" name:\"" + repositoryName + "\"){ name, description, forkCount, owner { login }, issues(first:" + numberOfIssuesToRequest + "){ nodes { title, body, createdAt, closedAt, state }}}}";

            var data = await ExecuteGraphQLRequest(() => GitHubApiClient.RepositoryQuery(new GraphQLRequest(requestString))).ConfigureAwait(false);

            return data.Repository;
        }

        public static async Task<IEnumerable<GitHubRepository>> GetRepositories(string repositoryOwner, int numberOfIssuesPerRequest = 100)
        {
            RepositoryConnection repositoryConnection = null;

            List<GitHubRepository> gitHubRepositoryList = new List<GitHubRepository>();

            do
            {
                repositoryConnection = await GetRepositoryConnection(repositoryOwner, numberOfIssuesPerRequest, repositoryConnection?.PageInfo?.EndCursor).ConfigureAwait(false);
                gitHubRepositoryList.AddRange(repositoryConnection.RepositoryList);
            }
            while (repositoryConnection?.PageInfo?.HasNextPage is true);

            return gitHubRepositoryList;
        }

        static async Task<RepositoryConnection> GetRepositoryConnection(string repositoryOwner, int numberOfRepositoriesPerRequest, string endCursor)
        {
            var endCursorString = string.IsNullOrWhiteSpace(endCursor) ? string.Empty : "after: \"" + endCursor + "\"";

            var requestString = "query{ user(login:" + repositoryOwner + "}) { repositories(first: " + numberOfRepositoriesPerRequest + endCursorString + ") { nodes { name, description, stargazers { totalCount } } pageInfo { endCursor, hasNextPage, hasPreviousPage, startCursor } } } }";

            var data = await ExecuteGraphQLRequest(() => GitHubApiClient.RepositoryConnectionQuery(new GraphQLRequest(requestString))).ConfigureAwait(false);

            return data.RepositoryConnection;
        }

        static async Task<T> ExecuteGraphQLRequest<T>(Func<Task<GraphQLResponse<T>>> action, int numRetries = 3)
        {
            var response = await Policy
                                .Handle<Exception>()
                                .WaitAndRetryAsync
                                (
                                    numRetries,
                                    pollyRetryAttempt
                                ).ExecuteAsync(action);


            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data;

            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
        #endregion
    }
}
