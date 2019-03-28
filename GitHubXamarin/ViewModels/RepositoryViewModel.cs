using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Refit;

namespace GitHubXamarin
{
    class RepositoryViewModel : BaseViewModel
    {
        #region Constant Fields
        readonly WeakEventManager<PullToRefreshFailedEventArgs> _pullToRefreshFailedEventManager = new WeakEventManager<PullToRefreshFailedEventArgs>();
        #endregion

        #region Fields
        bool _isRefreshing;
        ICommand _pullToRefreshCommand;
        ObservableCollection<Repository> _repositoryCollection = new ObservableCollection<Repository>();
        #endregion

        #region Events
        public event EventHandler<PullToRefreshFailedEventArgs> PullToRefreshFailed
        {
            add => _pullToRefreshFailedEventManager.AddEventHandler(value);
            remove => _pullToRefreshFailedEventManager.RemoveEventHandler(value);
        }
        #endregion

        #region Properties
        public ICommand PullToRefreshCommand => _pullToRefreshCommand ??
            (_pullToRefreshCommand = new AsyncCommand(() => ExecutePullToRefreshCommand(GitHubSettings.User), continueOnCapturedContext: false));

        public ObservableCollection<Repository> RepositoryCollection
        {
            get => _repositoryCollection;
            set => SetProperty(ref _repositoryCollection, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        #endregion

        #region Methods
        async Task ExecutePullToRefreshCommand(string repositoryOwner)
        {
            try
            {
                var repositoryList = await GitHubGraphQLService.GetRepositories(repositoryOwner).ConfigureAwait(false);

                foreach (var repository in repositoryList.OrderByDescending(x => x.StarCount))
                {
                    _repositoryCollection.Add(repository);
                }
            }
            catch (ApiException e) when (e.StatusCode is System.Net.HttpStatusCode.Unauthorized)
            {
                OnPullToRefreshFailed("Invalid Api Token", "Edit the Github API Token in Settings");
            }
            catch (Exception e)
            {
                OnPullToRefreshFailed("Error", e.Message);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        void OnPullToRefreshFailed(string title, string message) =>
            _pullToRefreshFailedEventManager.HandleEvent(this, new PullToRefreshFailedEventArgs(message, title), nameof(PullToRefreshFailed));
        #endregion
    }
}
