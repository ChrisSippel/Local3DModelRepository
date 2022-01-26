using System.Windows.Input;

namespace Local3DModelRepository.UiTools
{
    public interface ICloseableSaveableWindow
    {
        ICommand CloseWithoutSavingCommand { get; }

        ICommand CloseAndSaveCommand { get; }
    }
}