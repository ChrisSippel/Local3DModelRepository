using System.IO;
using Local3DModelRepository.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Local3DModelRepository.ViewModels
{
    public sealed class ModelViewModel : ObservableObject
    {
        private bool _isVisible;

        public ModelViewModel(IModel model)
        {
            Model = model;

            DisplayText = Path.GetFileNameWithoutExtension(Model.FileName);
            IsVisible = true;
        }

        public string DisplayText { get; }

        public IModel Model { get; }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
    }
}