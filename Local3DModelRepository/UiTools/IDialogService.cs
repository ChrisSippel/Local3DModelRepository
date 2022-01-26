using Local3DModelRepository.Controls;
using Local3DModelRepository.Repositories;
using Local3DModelRepository.ViewModels;

namespace Local3DModelRepository.UiTools
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows a <see cref="TagsWindow"/>, using the provided <see cref="ITagsWindowViewModel"/>.
        /// </summary>
        void ShowTagsDialog(ITagsWindowViewModel viewModel);

        /// <summary>
        /// Shows a <see cref="NewRepoWindow"/>, using the provided <see cref="INewRepoWindowViewModel"/>.
        /// </summary>
        void ShowNewRepoDialog(
            INewRepoWindowViewModel viewModel,
            INewRepoTypeSelectionPageViewModel newRepoTypeSelectionPageViewModel);
    }
}