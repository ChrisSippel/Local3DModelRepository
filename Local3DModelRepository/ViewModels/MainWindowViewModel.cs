using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
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
        private readonly ITagsWindowViewModelFactory _tagsWindowViewModelFactory;

        private Model3DGroup _selectedModel;
        private bool _isLoading;
        private IEnumerable<TagFilterViewModel> _includeFilterTags;
        private IEnumerable<TagFilterViewModel> _excludeFilterTags;

        private IModelRepositoryCollection _modelRepositoryCollection;
        private CancellationTokenSource _loadingModelCancellationTokenSource;

        public MainWindowViewModel(
            IStorageModule storageModule,
            IModelsLoader modelsLoader,
            IDialogService dialogService,
            ITagsWindowViewModelFactory tagsWindowViewModelFactory)
        {
            _storageModule = storageModule;
            _modelsLoader = modelsLoader;
            _dialogService = dialogService;
            _tagsWindowViewModelFactory = tagsWindowViewModelFactory;

            _isLoading = false;
            _modelImporter = new ModelImporter();
            _modelRepositoryCollection = new ModelRepositoryCollection();

            OpenFolderCommand = new RelayCommand(LoadModelsFromFolder);
            SelectionChangedCommand = new AsyncRelayCommand<ModelViewModel>(DisplaySelectedModel);
            AddTagsCommand = new RelayCommand(AddTagsToModel, () => !IsLoading && SelectedModel != null);
            RemoveTagsCommand = new RelayCommand<object>(RemoveTagsFromModel, (object o) => SelectedModel != null);
            IncludeFilterChanged = new RelayCommand<object>(IncludeFilterTagsChangedImpl);
            ExcludeFilterChanged = new RelayCommand(ExcludeFilterTagsChangedImpl);

            ModelViewModels = new ObservableCollection<ModelViewModel>();
            TagViewModels = new ObservableCollection<TagViewModel>();

            _includeFilterTags = new[] { IncludeAllTagFilterViewModel };
            _excludeFilterTags = Array.Empty<TagFilterViewModel>();
        }

        public event EventHandler SelectedModelChanged;

        public RelayCommand OpenFolderCommand { get; }

        public ICommand ExitApplicationCommand { get; }

        public AsyncRelayCommand<ModelViewModel> SelectionChangedCommand { get; }

        public RelayCommand AddTagsCommand { get; }

        public RelayCommand<object> RemoveTagsCommand { get; }

        public RelayCommand FilterComboBoxSelectedItemChanged { get; }

        public RelayCommand<object> IncludeFilterChanged { get; }

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

        public async Task LoadModelRespositoriesFromStorage()
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

                _modelRepositoryCollection = modelRepositoryCollection.ValueOrFailure();
                foreach (var modelRepository in _modelRepositoryCollection.ModelRepositories)
                {
                    var modelViewModels = modelRepository.Models.Select(x => new ModelViewModel(x)).ToArray();
                    modelViewModels.ForEach(x => ModelViewModels.Add(x));
                }

                UpdateFilterTags();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async ValueTask SaveModelRepositoriesToStorage()
        {
            try
            {
                IsLoading = true;
                await _storageModule.Save(_modelRepositoryCollection);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void LoadModelsFromFolder()
        {
            var userSelectedFolder = _dialogService.HaveUserSelectFolder();
            if (!userSelectedFolder.HasValue)
            {
                return;
            }

            var userSelectedFolderString = userSelectedFolder.ValueOrFailure();
            if (_modelRepositoryCollection.ModelRepositories.Any(x => x.DirectoryPath == userSelectedFolderString))
            {
                return;
            }

            ModelViewModels.Clear();

            var loadedModels = _modelsLoader.LoadAllModels(userSelectedFolderString);
            loadedModels.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));

            var modelRepository = new ModelRepository(
                userSelectedFolderString,
                loadedModels);

            _modelRepositoryCollection.ModelRepositories.Add(modelRepository);
        }

        private async Task DisplaySelectedModel(ModelViewModel modelViewModel)
        {
            _loadingModelCancellationTokenSource?.Cancel();
            _loadingModelCancellationTokenSource?.Dispose();
            _loadingModelCancellationTokenSource = new CancellationTokenSource();

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
                },
                _loadingModelCancellationTokenSource.Token);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddTagsToModel()
        {
            var viewModel = _tagsWindowViewModelFactory.Create(
                ModelViewModels.SelectMany(x => x.Model.Tags),
                SelectedModelViewModel.Model.Tags);

            _dialogService.ShowTagsDialog(viewModel);

            if (viewModel.SaveChanges)
            {
                TagViewModels.Clear();
                Array.ForEach(viewModel.SelectedTags.ToArray(), x =>
                {
                    if (!TagViewModels.Any(y => y.Tag.Equals(x)))
                    {
                        TagViewModels.Add(new TagViewModel(x));
                    }

                    if (!SelectedModelViewModel.Model.Tags.Any(y => y.Equals(x)))
                    {
                        SelectedModelViewModel.Model.Tags.Add(x);
                    }
                });

                _storageModule.Save(_modelRepositoryCollection);
            }

            UpdateFilterTags();
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

            _storageModule.Save(_modelRepositoryCollection);

            UpdateFilterTags();
        }

        private void IncludeFilterTagsChangedImpl(object turnedOnFilterObject)
        {
            var filtersToEnforce = IncludeFilterTags.Where(x => x.IsChecked);
            var turnedOnFilterString = (string)turnedOnFilterObject;
            if (turnedOnFilterString == IncludeAllTagFilterViewModel.Tag.Value ||
                !filtersToEnforce.Any())
            {
                IncludeAllTagFilterViewModel.IsChecked = true;
                IncludeFilterTags = ModelViewModels
                    .Select(x => x.Model)
                    .SelectMany(x => x.Tags)
                    .Distinct()
                    .OrderBy(x => x.Value)
                    .Select(x => new TagFilterViewModel(x))
                    .Prepend(IncludeAllTagFilterViewModel)
                    .ToArray();

                ModelViewModels.ForEach(x => x.IsVisible = true);
                return;
            }

            IncludeAllTagFilterViewModel.IsChecked = false;

            var modelsToDisplayHashSet = new HashSet<ModelViewModel>();
            foreach (var filterToEnforce in filtersToEnforce)
            {
                var modelsToDisplay =  ModelViewModels.Where(x => x.Model.Tags.Contains(filterToEnforce.Tag));
                foreach (var modelToDisplay in modelsToDisplay)
                {
                    modelsToDisplayHashSet.Add(modelToDisplay);
                }
            }

            ModelViewModels.ForEach(x => x.IsVisible = modelsToDisplayHashSet.Contains(x));
        }

        private void ExcludeFilterTagsChangedImpl()
        {
            var filtersToEnforce = ExcludeFilterTags.Where(x => x.IsChecked);
            var modelViewModelsToRemove = new HashSet<ModelViewModel>();
            foreach (var filterToEnforce in filtersToEnforce)
            {
                ModelViewModels
                    .Where(x => x.Model.Tags.Contains(filterToEnforce.Tag))
                    .ForEach(x => modelViewModelsToRemove.Add(x));
            }

            ModelViewModels.ForEach(x => x.IsVisible = !modelViewModelsToRemove.Contains(x));
        }

        private void UpdateFilterTags()
        {
            var tagFilterViewModels = ModelViewModels
                .SelectMany(x => x.Model.Tags)
                .Distinct()
                .OrderBy(x => x.Value)
                .Select(x => new TagFilterViewModel(x))
                .ToArray();

            IncludeFilterTags = new List<TagFilterViewModel>(tagFilterViewModels);
            IncludeFilterTags = IncludeFilterTags.Prepend(IncludeAllTagFilterViewModel);

            ExcludeFilterTags = new List<TagFilterViewModel>(tagFilterViewModels);
        }
    }
}
