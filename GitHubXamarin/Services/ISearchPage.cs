using System;
namespace GitHubXamarin
{
    public interface ISearchPage
    {
        void OnSearchBarTextChanged(in string text);
        event EventHandler<string> SearchBarTextChanged;
    }
}
