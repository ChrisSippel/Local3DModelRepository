using Optional;

namespace Local3DModelRepository.Wrappers
{
    public interface IFolderSelectionDialogWrapper
    {
        Option<string> HaveUserSelectFolder();
    }
}