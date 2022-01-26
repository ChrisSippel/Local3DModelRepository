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