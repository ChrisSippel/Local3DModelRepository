using ModernWpf.Controls;

namespace Local3DModelRepository.Repositories.Api
{
    public interface ISupportedRepoInformation
    {
        /// <summary>
        /// The path to the icon to display to the user.
        /// </summary>
        /// <remarks>
        /// Is is expected the icon is within the Assets folder of the program.
        /// </remarks>>
        string IconPath { get; }

        /// <summary>
        /// The name to display to the user, that describes this repo.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The <see cref="INewRepoCreationPageViewModel"/> to use when the
        /// user is creating a new repo.
        /// </summary>
        INewRepoCreationPageViewModel CreationViewModel { get; }

        /// <summary>
        /// The <see cref="Page"/> to display when the user is creating a
        /// new repo.
        /// </summary>
        Page CreationPage { get; }
    }
}