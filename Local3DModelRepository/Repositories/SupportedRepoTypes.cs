using Local3DModelRepository.Repositories.Local;
using System;
using System.Collections.Generic;

namespace Local3DModelRepository.Repositories
{
    public enum SupportedRepoTypes
    {
        Local = 1,
    }

    public sealed class SupportedRepos
    {
        public static SupportedRepos LocalRepo = new SupportedRepos(SupportedRepoTypes.Local, typeof(LocalRepoPage), typeof(LocalRepoPageViewModel));

        public static IEnumerable<SupportedRepos> All = new List<SupportedRepos>
        {
            LocalRepo,
        };

        public static IReadOnlyDictionary<SupportedRepoTypes, SupportedRepos> SupportedReposByType = new Dictionary<SupportedRepoTypes, SupportedRepos>
        {
            { SupportedRepoTypes.Local, LocalRepo }
        };

        private SupportedRepos(
            SupportedRepoTypes repoType,
            Type xamlPage,
            Type viewModel)
        {
            RepoType = repoType;
            XamlPage = xamlPage;
            ViewModel = viewModel;
        }

        public SupportedRepoTypes RepoType { get; }

        public Type XamlPage { get; }

        public Type ViewModel { get; }
    }
}
