using System.Windows;
using Local3DModelRepository.UiTools;
using Local3DModelRepository.ViewModels;

namespace Local3DModelRepository.Controls
{
    public partial class TagsWindow : Window, IClosableWindow
    {
        public TagsWindow(ITagsWindowViewModel tagsWindowViewModel)
        {
            InitializeComponent();
            DataContext = tagsWindowViewModel;
        }
    }
}