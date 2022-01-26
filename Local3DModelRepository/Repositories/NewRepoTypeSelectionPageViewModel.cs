using System.Collections.Generic;
using Local3DModelRepository.Repositories.Api;

namespace Local3DModelRepository.Repositories
{
    public sealed class NewRepoTypeSelectionPageViewModel : INewRepoTypeSelectionPageViewModel
    {
        private readonly IEnumerable<ISupportedRepoInformation> _supportedRepos;

        public NewRepoTypeSelectionPageViewModel(IEnumerable<ISupportedRepoInformation> supportedRepos)
        {
            _supportedRepos = supportedRepos;
            SupportedReposInformation = supportedRepos;
        }

        public IEnumerable<ISupportedRepoInformation> SupportedReposInformation { get; }
    }
}