using Local3DModelRepository.Controls;
using Local3DModelRepository.Repositories.Creation;
using Local3DModelRepository.ViewModels;
using Optional;

namespace Local3DModelRepository.UiTools
{
    public interface IDialogService
    {
        /// <summary>
        /// Has the user pick a folder, and returns the full path
        /// to the folder they selected.
        /// </summary>
        /// <returns>
        /// The full path to the folder the user selected.
        /// </returns>
        Option<string> HaveUserSelectFolder();

        /// <summary>
        /// Shows a <see cref="TagsWindow"/>, using the provided <see cref="ITagsWindowViewModel"/>.
        /// </summary>
        void ShowTagsDialog(ITagsWindowViewModel viewModel);

        /// <summary>
        /// Shows a <see cref="NewRepoWindow"/>, using the provided <see cref="INewRepoWindowViewModel"/>.
        /// </summary>
        void ShowNewRepoDialog(INewRepoWindowViewModel viewModel);
    }
}
