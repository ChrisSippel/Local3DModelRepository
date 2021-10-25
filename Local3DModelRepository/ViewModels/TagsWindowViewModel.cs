using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Local3DModelRepository.Models;
using Local3DModelRepository.UiTools;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Local3DModelRepository.ViewModels
{
    public sealed class TagsWindowViewModel : ObservableObject, ITagsWindowViewModel
    {
        private string _userAddedTags;

        public TagsWindowViewModel(
            IEnumerable<ITag> possibleTags,
            IEnumerable<ITag> modelTags)
        {
            _userAddedTags = string.Empty;

            PossibleTags = new ObservableCollection<ITag>(possibleTags.Distinct());
            SelectedTags = new ObservableCollection<ITag>(modelTags);

            CloseWithoutSavingCommand = new RelayCommand<IClosableWindow>(CloseWithoutSavingCommandImpl);
            CloseAndSaveCommand = new RelayCommand<IClosableWindow>(CloseAndSaveCommandImpl);
            RemoveSelectedTag = new RelayCommand<object>(RemoveSelectedTagCommandImpl);
            AddSelectedTag = new RelayCommand<object>(AddSelectedTagCommandImpl);
            AddUserGivenTags = new RelayCommand(AddUserGivenTagsImpl);
        }

        public ObservableCollection<ITag> PossibleTags { get; }

        public ObservableCollection<ITag> SelectedTags { get; }

        public bool SaveChanges { get; private set; }

        public string UserAddedTags
        {
            get => _userAddedTags;
            set => SetProperty(ref _userAddedTags, value);
        }

        public ICommand CloseWithoutSavingCommand { get; }

        public ICommand CloseAndSaveCommand { get; }

        public ICommand RemoveSelectedTag { get; }

        public ICommand AddSelectedTag { get; }

        public ICommand AddUserGivenTags { get; }

        private void CloseWithoutSavingCommandImpl(IClosableWindow window)
        {
            SaveChanges = false;
            window.Close();
        }

        private void CloseAndSaveCommandImpl(IClosableWindow window)
        {
            SaveChanges = true;
            window.Close();
        }

        private void RemoveSelectedTagCommandImpl(object tagToRemove)
        {
            if (tagToRemove is not Tag tag)
            {
                return;
            }

            SelectedTags.Remove(tag);
        }

        private void AddSelectedTagCommandImpl(object tagToAdd)
        {
            if (tagToAdd is not Tag tag)
            {
                return;
            }

            if (!SelectedTags.Contains(tag))
            {
                SelectedTags.Add(tag);
            }
        }

        private void AddUserGivenTagsImpl()
        {
            var tagStringsInArray = UserAddedTags.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Array.ForEach(tagStringsInArray, x =>
            {
                var tag = new Tag(x);
                if (!SelectedTags.Contains(tag))
                {
                    SelectedTags.Add(tag);
                }

                if (!PossibleTags.Contains(tag))
                {
                    PossibleTags.Add(tag);
                }
            });

            UserAddedTags = string.Empty;
        }
    }
}
