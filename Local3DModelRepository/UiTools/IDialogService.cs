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
    }
}
