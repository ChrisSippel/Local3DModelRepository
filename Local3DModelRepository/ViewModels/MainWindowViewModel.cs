using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Local3DModelRepository.Controls;
using Local3DModelRepository.DataLoaders;
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.ExtensionMethods;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Optional;
using Optional.Unsafe;

namespace Local3DModelRepository.ViewModels
{
    public sealed class MainWindowViewModel : ObservableObject
    {
        private static readonly TagFilterViewModel IncludeAllTagFilterViewModel = new TagFilterViewModel(new Tag("Include all tags"), true);

        private readonly IStorageModule _storageModule;
        private readonly ModelImporter _modelImporter;
        private readonly IModelsLoader _modelsLoader;
        private readonly IDialogService _dialogService;

        private Model3DGroup _selectedModel;
        private bool _isLoading;
        private IEnumerable<TagFilterViewModel> _includeFilterTags;
        private IEnumerable<TagFilterViewModel> _excludeFilterTags;

        public MainWindowViewModel(
            IStorageModule storageModule,
            IModelsLoader modelsLoader,
            IDialogService dialogService)
        {
            _storageModule = storageModule;
            _modelsLoader = modelsLoader;
            _dialogService = dialogService;

            _isLoading = false;
            _modelImporter = new ModelImporter();

            OpenFolderCommand = new RelayCommand(LoadModelsFromFolder);
            SelectionChangedCommand = new AsyncRelayCommand<ModelViewModel>(DisplaySelectedModel);
            AddTagsCommand = new RelayCommand(AddTagsToModel, () => !IsLoading && SelectedModel != null);
            RemoveTagsCommand = new RelayCommand<object>(RemoveTagsFromModel, (object o) => SelectedModel != null);
            IncludeFilterChanged = new RelayCommand(IncludeFilterTagsChangedImpl);
            ExcludeFilterChanged = new RelayCommand(ExcludeFilterTagsChangedImpl);

            ModelViewModels = new ObservableCollection<ModelViewModel>();
            TagViewModels = new ObservableCollection<TagViewModel>();

            /*_includeFilterTags = _storageModule.Models
                .SelectMany(x => x.Tags)
                .Distinct()
                .OrderBy(x => x.Value)
                .Select(x => new TagFilterViewModel(x))
                .Prepend(IncludeAllTagFilterViewModel)
                .ToArray();

            _excludeFilterTags = _storageModule.Models
                .SelectMany(x => x.Tags)
                .Distinct()
                .OrderBy(x => x.Value)
                .Select(x => new TagFilterViewModel(x))
                .ToArray();
            */
        }

        public event EventHandler SelectedModelChanged;

        public RelayCommand OpenFolderCommand { get; }

        public ICommand ExitApplicationCommand { get; }

        public AsyncRelayCommand<ModelViewModel> SelectionChangedCommand { get; }

        public RelayCommand AddTagsCommand { get; }

        public RelayCommand<object> RemoveTagsCommand { get; }

        public RelayCommand FilterComboBoxSelectedItemChanged { get; }

        public RelayCommand IncludeFilterChanged { get; }

        public RelayCommand ExcludeFilterChanged { get; }

        public ObservableCollection<ModelViewModel> ModelViewModels { get; }

        public ObservableCollection<TagViewModel> TagViewModels { get; }

        public IEnumerable<TagFilterViewModel> IncludeFilterTags
        {
            get => _includeFilterTags;
            set => SetProperty(ref _includeFilterTags, value);
        }

        public IEnumerable<TagFilterViewModel> ExcludeFilterTags
        {
            get => _excludeFilterTags;
            set => SetProperty(ref _excludeFilterTags, value);
        }

        public ModelViewModel SelectedModelViewModel { get; set; }

        public Model3DGroup SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (SetProperty(ref _selectedModel, value) &&
                    value != null)
                {
                    AddTagsCommand.NotifyCanExecuteChanged();
                    RemoveTagsCommand.NotifyCanExecuteChanged();
                    SelectedModelChanged?.Invoke(this, EventArgs.Empty);
                    TagViewModels.Clear();
                    SelectedModelViewModel.Model.Tags.ForEach(x => TagViewModels.Add(new TagViewModel(x)));
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set 
            { 
                if (SetProperty(ref _isLoading, value))
                {
                    AddTagsCommand.NotifyCanExecuteChanged();
                    RemoveTagsCommand.NotifyCanExecuteChanged();
                } 
            }
        }

        public string TagsToIncludeInFilterText
        {
            get => "Tags to require in filter";
            set
            {
                // Prevent the text from being changed
                return;
            }
        }

        public string TagsToExcludeInFilterText
        {
            get => "Tags to exclude in filter";
            set
            {
                // Prevent the text from being changed
                return;
            }
        }

        public async Task LoadExistingModelRespositories()
        {
            IsLoading = true;

            try
            {
                var modelRepositoryCollection = Option.None<IModelRepositoryCollection>();
                await Task.Run(() => modelRepositoryCollection = _storageModule.Load());

                if (!modelRepositoryCollection.HasValue)
                {
                    IsLoading = false;
                    return;
                }

                var existingModelRepositoryCollection = modelRepositoryCollection.ValueOrFailure();
                foreach (var modelRepository in existingModelRepositoryCollection.ModelRepositories)
                {
                    var modelViewModels = modelRepository.Models.Select(x => new ModelViewModel(x)).ToArray();
                    modelViewModels.ForEach(x => ModelViewModels.Add(x));
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SaveModelRepositories()
        {
            // Make if file doesn't exist
            // blind overwrite otherwise

            // Create IModelRepositoryCollection object
            // using the tags and models we have access to

            // Serialize object to file
        }

        private void LoadModelsFromFolder()
        {
            var userSelectedFolder = _dialogService.HaveUserSelectFolder();
            if (!userSelectedFolder.HasValue)
            {
                return;
            }

            ModelViewModels.Clear();

            var loadedModels = _modelsLoader.LoadAllModels(userSelectedFolder.ValueOrFailure());
            loadedModels.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));
        }

        private async Task DisplaySelectedModel(ModelViewModel modelViewModel)
        {
            if (modelViewModel == null)
            {
                SelectedModel = null;
                return;
            }

            IsLoading = true;

            try
            {
                await Task.Run(() =>
                {
                    Model3DGroup loadedModel = _modelImporter.Load(modelViewModel.Model.FullPath);
                    loadedModel.Freeze();

                    Application.Current.Dispatcher.Invoke(() => SelectedModel = loadedModel);
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddTagsToModel()
        {
            /*
            var viewModel = new TagsWindowViewModel(ModelViewModels.SelectMany(x => x.Model.Tags), SelectedModelViewModel.Model.Tags);
            var tagsWindow = new TagsWindow(viewModel);
            tagsWindow.ShowDialog();

            if (viewModel.SaveChanges)
            {
                TagViewModels.Clear();
                Array.ForEach(viewModel.SelectedTags.ToArray(), x =>
                {
                    if (!TagViewModels.Any(y => y.Tag.Equals(x)))
                    {
                        TagViewModels.Add(new TagViewModel(x));
                    }
                });

                Array.ForEach(viewModel.SelectedTags.ToArray(), x =>
                {
                    if (!SelectedModelViewModel.Model.Tags.Any(y => y.Equals(x)))
                    {
                        SelectedModelViewModel.Model.Tags.Add(x);
                    }
                });

                ////_storageModule.Save();
            }
            */

            ////IncludeFilterTags = _storageModule.Models.SelectMany(x => x.Tags).Distinct().OrderBy(x => x.Value).Select(x => new TagFilterViewModel(x)).ToArray();
            ////ExcludeFilterTags = _storageModule.Models.SelectMany(x => x.Tags).Distinct().OrderBy(x => x.Value).Select(x => new TagFilterViewModel(x)).ToArray();
        }

        private void RemoveTagsFromModel(object tagsAsObjects)
        {
            if (tagsAsObjects is not IList tagsAsList)
            {
                return;
            }

            var typedTagsAsList = tagsAsList.Cast<TagViewModel>().ToList();
            foreach(var tagViewModel in typedTagsAsList)
            {
                SelectedModelViewModel.Model.Tags.Remove(tagViewModel.Tag);
                TagViewModels.Remove(tagViewModel);
            }

            ////_storageModule.Save();

            /*IncludeFilterTags = _storageModule.Models
                .SelectMany(x => x.Tags)
                .Distinct()
                .OrderBy(x => x.Value)
                .Select(x => new TagFilterViewModel(x))
                .Prepend(IncludeAllTagFilterViewModel)
                .ToArray();*/
        }

        private void IncludeFilterTagsChangedImpl()
        {
            /*
            ModelViewModels.Clear();

            var filtersToEnforce = IncludeFilterTags.Where(x => x.IsChecked);
            if (!filtersToEnforce.Any() ||
                (filtersToEnforce.Count() == 1 &&
                 filtersToEnforce.First().Equals(IncludeAllTagFilterViewModel)))
            {
                IncludeAllTagFilterViewModel.IsChecked = true;
                IncludeFilterTags = _storageModule.Models
                    .SelectMany(x => x.Tags)
                    .Distinct()
                    .OrderBy(x => x.Value)
                    .Select(x => new TagFilterViewModel(x))
                    .Prepend(IncludeAllTagFilterViewModel)
                    .ToArray();

                _storageModule.Models.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));
                return;
            }
            else if (filtersToEnforce.Count() > 1)
            {
                IncludeAllTagFilterViewModel.IsChecked = false;
            }

            var modelsToDisplayHashSet = new HashSet<IModel>();
            foreach (var filterToEnforce in filtersToEnforce)
            {
                var modelsToDisplay = _storageModule.Models.Where(x => !modelsToDisplayHashSet.Contains(x) && x.Tags.Contains(filterToEnforce.Tag));
                foreach (var modelToDisplay in modelsToDisplay)
                {
                    modelsToDisplayHashSet.Add(modelToDisplay);
                }
            }

            modelsToDisplayHashSet.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));
            */
        }

        private void ExcludeFilterTagsChangedImpl()
        {
            /*
            ModelViewModels.Clear();
            _storageModule.Models.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));

            var filtersToEnforce = ExcludeFilterTags.Where(x => x.IsChecked);
            var modelViewModelsToRemove = new HashSet<ModelViewModel>();
            foreach (var filterToEnforce in filtersToEnforce)
            {
                ModelViewModels
                    .Where(x => !modelViewModelsToRemove.Contains(x) && x.Model.Tags.Contains(filterToEnforce.Tag))
                    .ForEach(x => modelViewModelsToRemove.Add(x));
            }

            foreach (var modelToRemove in modelViewModelsToRemove)
            {
                ModelViewModels.Remove(modelToRemove);
            }
            */
        }
    }
}
