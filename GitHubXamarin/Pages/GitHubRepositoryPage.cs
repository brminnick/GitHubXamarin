using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GitHubXamarin
{
    class GitHubRepositoryPage : BaseContentPage<GitHubRepositoryViewModel>
    {
        readonly Xamarin.Forms.ListView _listView;

        public GitHubRepositoryPage()
        {
            _listView = new Xamarin.Forms.ListView(ListViewCachingStrategy.RecycleElement)
            {
                IsPullToRefreshEnabled = true,
                ItemTemplate = new DataTemplate(typeof(RepositoryViewCell)),
                SeparatorVisibility = SeparatorVisibility.None,
                RowHeight = RepositoryViewCell.ImageHeight,
            }; 
            _listView.SetBinding(Xamarin.Forms.ListView.IsRefreshingProperty, nameof(ViewModel.IsRefreshing));
            _listView.SetBinding(Xamarin.Forms.ListView.ItemsSourceProperty, nameof(ViewModel.RepositoryCollection));
            _listView.SetBinding(Xamarin.Forms.ListView.RefreshCommandProperty, nameof(ViewModel.PullToRefreshCommand));

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            Content = _listView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _listView.BeginRefresh();
        }
    }
}
