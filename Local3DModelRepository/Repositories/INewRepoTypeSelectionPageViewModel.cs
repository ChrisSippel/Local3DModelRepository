using System.Collections.Generic;
using Local3DModelRepository.Repositories.Api;

namespace Local3DModelRepository.Repositories
{
    public interface INewRepoTypeSelectionPageViewModel
    {
        IEnumerable<ISupportedRepoInformation> SupportedReposInformation { get; }
    }
}