using Local3DModelRepository.Repositories.Api;
using ModernWpf.Controls;

namespace Local3DModelRepository.Repositories.Local
{
    public sealed class SupportedRepoInformation : ISupportedRepoInformation
    {
        public SupportedRepoInformation(INewRepoCreationPageViewModel newRepoCreationPageViewModel)
        {
            CreationViewModel = newRepoCreationPageViewModel;
        }

        /// <inheritdoc />
        public string IconPath { get; } = "Assets/folder.png";

        /// <inheritdoc />
        public string DisplayName { get; } = "Local";

        /// <inheritdoc />
        public INewRepoCreationPageViewModel CreationViewModel { get; }

        /// <inheritdoc />
        public Page CreationPage { get; } = new LocalRepoCreationPage();
    }
}