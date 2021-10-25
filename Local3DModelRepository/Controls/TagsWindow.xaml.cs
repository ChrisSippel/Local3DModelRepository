using System.Windows;
using Local3DModelRepository.UiTools;
using Local3DModelRepository.ViewModels;


namespace Local3DModelRepository.Controls
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TagsWindow : Window, IClosableWindow
    {
        public TagsWindow(ITagsWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
