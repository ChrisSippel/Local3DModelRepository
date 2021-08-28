using Ookii.Dialogs.Wpf;
using Optional;

namespace Local3DModelRepository.UiTools
{
    public sealed class DialogService : IDialogService
    {
        public Option<string> HaveUserSelectFolder()
        {
            var folderBrowserDialog = new VistaFolderBrowserDialog();

            var result = folderBrowserDialog.ShowDialog();
            return result.HasValue
                ? Option.Some(folderBrowserDialog.SelectedPath)
                : Option.None<string>();
        }
    }
}
