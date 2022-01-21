using Local3DModelRepository.DataLoaders;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.FileSystemAccess;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;
using Local3DModelRepository.ViewModels;
using Microsoft.Toolkit.Mvvm.Input;
using Optional;
using Optional.Unsafe;
using System.IO;
using Page = ModernWpf.Controls.Page;

namespace Local3DModelRepository.Controls
{
    /// <summary>
    /// Interaction logic for LocalRepoSelectionPage.xaml
    /// </summary>
    public partial class LocalRepoSelectionPage : Page
    {
        public LocalRepoSelectionPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialogService = new DialogService();
            var userSelectedFolder = dialogService.HaveUserSelectFolder();
            if (!userSelectedFolder.HasValue)
            {
                return;
            }

            RepoLocationTextBox.Text = userSelectedFolder.ValueOr(string.Empty);
            RepoNameTextBox.Text = Path.GetFileName(userSelectedFolder.ValueOr(string.Empty));

            var modelsLoader = new ModelsLoader(new ModelFactory(), new DirectoryWrapper());
            var userSelectedFolderString = userSelectedFolder.ValueOrFailure();
            var loadedModels = modelsLoader.LoadAllModels(userSelectedFolderString);
            var modelRepository = new LocalModelRepository("Name", userSelectedFolderString, loadedModels);
            ((NewRepoWindowViewModel)DataContext).ModelRepsitory = Option.Some<IModelRepository>(modelRepository);
            ((NewRepoWindowViewModel)DataContext).CanCreateNewRepo = true;

            ((RelayCommand<IClosableWindow>)((NewRepoWindowViewModel)DataContext).CloseAndSaveCommand).NotifyCanExecuteChanged();
        }

        private void RepoNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
