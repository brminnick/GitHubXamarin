using System;
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
                RefreshControlColor = ColorConstants.DarkBlue,
                BackgroundColor = ColorConstants.LightBlue
            };
            _listView.ItemTapped += HandleListViewItemTapped;
            _listView.SetBinding(Xamarin.Forms.ListView.IsRefreshingProperty, nameof(ViewModel.IsRefreshing));
            _listView.SetBinding(Xamarin.Forms.ListView.ItemsSourceProperty, nameof(ViewModel.RepositoryCollection));
            _listView.SetBinding(Xamarin.Forms.ListView.RefreshCommandProperty, nameof(ViewModel.PullToRefreshCommand));

            Title = GitHubConstants.User;
            BackgroundColor = ColorConstants.LightBlue;

            Content = _listView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _listView.BeginRefresh();
        }

        async void HandleListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is Xamarin.Forms.ListView listView)
                listView.SelectedItem = null;

            if (e.Item is Repository repository && repository.Uri.IsAbsoluteUri && repository.Uri.Scheme.Equals(Uri.UriSchemeHttps))
                await Xamarin.Essentials.Browser.OpenAsync(repository.Uri);
        }
    }
}
