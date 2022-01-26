using Ookii.Dialogs.Wpf;
using Optional;

namespace Local3DModelRepository.Wrappers
{
    internal sealed class FolderSelectionDialogWrapper : IFolderSelectionDialogWrapper
    {
        public Option<string> HaveUserSelectFolder()
        {
            var folderBrowserDialog = new VistaFolderBrowserDialog();

            var result = folderBrowserDialog.ShowDialog();
            return result.HasValue && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath)
                ? Option.Some(folderBrowserDialog.SelectedPath)
                : Option.None<string>();
        }
    }
}