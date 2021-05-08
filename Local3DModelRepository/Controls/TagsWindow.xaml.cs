using System.Windows;
using Local3DModelRepository.ViewModels;


namespace Local3DModelRepository.Controls
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TagsWindow : Window
    {
        public TagsWindow(ITagsWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
