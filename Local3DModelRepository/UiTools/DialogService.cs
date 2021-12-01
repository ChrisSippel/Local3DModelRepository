using Local3DModelRepository.Controls;
using Local3DModelRepository.ViewModels;
using Ookii.Dialogs.Wpf;
using Optional;

namespace Local3DModelRepository.UiTools
{
    public sealed class DialogService : IDialogService
    {
        /// <inheritdoc />
        public Option<string> HaveUserSelectFolder()
        {
            var folderBrowserDialog = new VistaFolderBrowserDialog();

            var result = folderBrowserDialog.ShowDialog();
            return result.HasValue && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath)
                ? Option.Some(folderBrowserDialog.SelectedPath)
                : Option.None<string>();
        }

        public void ShowNewRepoDialog(INewRepoWindowViewModel newRepoWindowViewModel)
        {
            var window = new NewRepoWindow(newRepoWindowViewModel);
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
