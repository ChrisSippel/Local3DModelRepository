using System.Collections.ObjectModel;
using System.Windows.Input;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;

namespace Local3DModelRepository.ViewModels
{
    public interface ITagsWindowViewModel : ICloseableSaveableWindow
    {
        ICommand RemoveSelectedTag { get; }

        ICommand AddSelectedTag { get; }

        ICommand AddUserGivenTags { get; }

        ObservableCollection<ITag> PossibleTags { get; }

        ObservableCollection<ITag> SelectedTags { get; }

        bool SaveChanges { get; }

        string UserAddedTags { get; set; }
    }
}