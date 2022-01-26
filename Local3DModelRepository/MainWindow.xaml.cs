using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Local3DModelRepository.ViewModels;

namespace Local3DModelRepository
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;

            InitializeComponent();

            DataContext = mainWindowViewModel;
            mainWindowViewModel.SelectedModelChanged += MainWindowViewModel_SelectedModelChanged;

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ////await _mainWindowViewModel.SaveModelRepositoriesToStorage();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ////await _mainWindowViewModel.LoadModelRespositoriesFromStorage();
        }

        private void MainWindowViewModel_SelectedModelChanged(object sender, EventArgs e)
        {
            ////ViewPort.ZoomExtents();
        }
    }
}