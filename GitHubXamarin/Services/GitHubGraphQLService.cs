using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Refit;
using Polly;

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
