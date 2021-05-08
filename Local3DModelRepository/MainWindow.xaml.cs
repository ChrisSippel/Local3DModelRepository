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
        }

        private void MainWindowViewModel_SelectedModelChanged(object sender, EventArgs e)
        {
            ViewPort.ZoomExtents();
        }
    }
}
