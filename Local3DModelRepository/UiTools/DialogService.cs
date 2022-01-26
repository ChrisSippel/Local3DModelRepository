using Local3DModelRepository.Controls;
using Local3DModelRepository.Repositories;
using Local3DModelRepository.ViewModels;

namespace Local3DModelRepository.UiTools
{
    public sealed class DialogService : IDialogService
    {
        public void ShowNewRepoDialog(
            INewRepoWindowViewModel newRepoWindowViewModel,
            INewRepoTypeSelectionPageViewModel newRepoTypeSelectionPageViewModel)
        {
            var window = new NewRepoWindow(newRepoWindowViewModel, newRepoTypeSelectionPageViewModel);
            window.ShowDialog();
        }

        /// <inheritdoc />
        public void ShowTagsDialog(ITagsWindowViewModel viewModel)
        {
            var window = new TagsWindow(viewModel);
            window.ShowDialog();
        }
    }
}