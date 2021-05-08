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
using Local3DModelRepository.DataStorage;
using Local3DModelRepository.ExtensionMethods;
using Local3DModelRepository.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace Local3DModelRepository.ViewModels
{
    public sealed class MainWindowViewModel : ObservableObject
    {
        private static readonly TagFilterViewModel IncludeAllTagFilterViewModel = new TagFilterViewModel(new Tag("Include all tags"), true);

        private readonly IStorageModule _storageModule;
        private readonly ModelImporter _modelImporter;

        private Model3DGroup _selectedModel;
        private bool _isLoading;
        private IEnumerable<TagFilterViewModel> _includeFilterTags;
        private IEnumerable<TagFilterViewModel> _excludeFilterTags;

        public MainWindowViewModel(IStorageModule storageModule)
        {
            _storageModule = storageModule;

            _isLoading = false;
            _modelImporter = new ModelImporter();

            OpenFolderCommand = new AsyncRelayCommand(SelectFolder);
            SelectionChangedCommand = new AsyncRelayCommand<ModelViewModel>(LoadModel);
            AddTagsCommand = new RelayCommand(AddTagsToModel, () => !IsLoading && SelectedModel != null);
            RemoveTagsCommand = new RelayCommand<object>(RemoveTagsFromModel, (object o) => SelectedModel != null);
            IncludeFilterChanged = new RelayCommand(IncludeFilterTagsChangedImpl);
            ExcludeFilterChanged = new RelayCommand(ExcludeFilterTagsChangedImpl);

            ModelViewModels = new ObservableCollection<ModelViewModel>();
            TagViewModels = new ObservableCollection<TagViewModel>();

            _storageModule.Initialize();
            _storageModule.LoadedModels.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));

            _includeFilterTags = _storageModule.LoadedModels
                .SelectMany(x => x.Tags)
                .Distinct()
                .OrderBy(x => x.Value)
                .Select(x => new TagFilterViewModel(x))
                .Prepend(IncludeAllTagFilterViewModel)
                .ToArray();

            _excludeFilterTags = _storageModule.LoadedModels
                .SelectMany(x => x.Tags)
                .Distinct()
                .OrderBy(x => x.Value)
                .Select(x => new TagFilterViewModel(x))
                .ToArray();
        }

        public event EventHandler SelectedModelChanged;

        public AsyncRelayCommand OpenFolderCommand { get; }

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

        private async Task SelectFolder()
        {
            var folderBrowserDialog = new VistaFolderBrowserDialog();

            var result = folderBrowserDialog.ShowDialog();
            if (!result.HasValue ||
                !result.Value)
            {
                return;
            }

            ModelViewModels.Clear();

            await Task.Run(() =>
            {
                _storageModule.LoadAllModels(folderBrowserDialog.SelectedPath);
            });

            _storageModule.LoadedModels.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));
        }

        private async Task LoadModel(ModelViewModel modelViewModel)
        {
            IsLoading = true;
            SelectedModel = null;

            try
            {
                if (modelViewModel != null)
                {
                    await Task.Run(() =>
                    {
                        Model3DGroup loadedModel = _modelImporter.Load(modelViewModel.Model.FullPath);
                        loadedModel.Freeze();

                        Application.Current.Dispatcher.Invoke(() => SelectedModel = loadedModel);
                    });
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddTagsToModel()
        {
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

                _storageModule.SaveAllModels();
            }

            IncludeFilterTags = _storageModule.LoadedModels.SelectMany(x => x.Tags).Distinct().OrderBy(x => x.Value).Select(x => new TagFilterViewModel(x)).ToArray();
            ExcludeFilterTags = _storageModule.LoadedModels.SelectMany(x => x.Tags).Distinct().OrderBy(x => x.Value).Select(x => new TagFilterViewModel(x)).ToArray();
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

            _storageModule.SaveAllModels();

            IncludeFilterTags = _storageModule.LoadedModels
                .SelectMany(x => x.Tags)
                .Distinct()
                .OrderBy(x => x.Value)
                .Select(x => new TagFilterViewModel(x))
                .Prepend(IncludeAllTagFilterViewModel)
                .ToArray();
        }

        private void IncludeFilterTagsChangedImpl()
        {
            ModelViewModels.Clear();

            var filtersToEnforce = IncludeFilterTags.Where(x => x.IsChecked);
            if (!filtersToEnforce.Any() ||
                (filtersToEnforce.Count() == 1 &&
                 filtersToEnforce.First().Equals(IncludeAllTagFilterViewModel)))
            {
                IncludeAllTagFilterViewModel.IsChecked = true;
                IncludeFilterTags = _storageModule.LoadedModels
                    .SelectMany(x => x.Tags)
                    .Distinct()
                    .OrderBy(x => x.Value)
                    .Select(x => new TagFilterViewModel(x))
                    .Prepend(IncludeAllTagFilterViewModel)
                    .ToArray();

                _storageModule.LoadedModels.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));
                return;
            }
            else if (filtersToEnforce.Count() > 1)
            {
                IncludeAllTagFilterViewModel.IsChecked = false;
            }

            var modelsToDisplayHashSet = new HashSet<IModel>();
            foreach (var filterToEnforce in filtersToEnforce)
            {
                var modelsToDisplay = _storageModule.LoadedModels.Where(x => !modelsToDisplayHashSet.Contains(x) && x.Tags.Contains(filterToEnforce.Tag));
                foreach (var modelToDisplay in modelsToDisplay)
                {
                    modelsToDisplayHashSet.Add(modelToDisplay);
                }
            }

            modelsToDisplayHashSet.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));
        }

        private void ExcludeFilterTagsChangedImpl()
        {
            ModelViewModels.Clear();
            _storageModule.LoadedModels.ForEach(x => ModelViewModels.Add(new ModelViewModel(x)));

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
        }
    }
}
