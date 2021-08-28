using System;
using System.Windows;
using Local3DModelRepository.ViewModels;

namespace Local3DModelRepository
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();

            DataContext = mainWindowViewModel;
            mainWindowViewModel.SelectedModelChanged += MainWindowViewModel_SelectedModelChanged;

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await ((MainWindowViewModel)DataContext).LoadExistingModelRespositories();
        }

        private void MainWindowViewModel_SelectedModelChanged(object sender, EventArgs e)
        {
            ViewPort.ZoomExtents();
        }
    }
}
