using System.IO;
using Local3DModelRepository.Models;

namespace Local3DModelRepository.ViewModels
{
    public sealed class ModelViewModel
    {
        public ModelViewModel(IModel model)
        {
            Model = model;

            DisplayText = Path.GetFileNameWithoutExtension(Model.FileName);
        }

        public string DisplayText { get; }

        public IModel Model { get; }
    }
}
