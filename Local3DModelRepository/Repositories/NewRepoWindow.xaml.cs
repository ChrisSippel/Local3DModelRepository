using Local3DModelRepository.UiTools;
using ModernWpf.Media.Animation;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using Local3DModelRepository.Repositories.Api;
using System.Collections.Generic;

namespace Local3DModelRepository.Repositories
{
    public partial class NewRepoWindow : Window, IClosableWindow
    {
        private const int DistanceToMoveBackButton = 180;
        private const int DistanceToMoveCreateButton = 200;
        private static readonly TimeSpan ButtonMovementDuration = TimeSpan.FromSeconds(0.2);

        private readonly Stack<object> _activeViewModelsStack;

        private readonly NavigationTransitionInfo SlideInFromRightTransition = new SlideNavigationTransitionInfo
        {
            Effect = SlideNavigationTransitionEffect.FromRight
        };

        public NewRepoWindow(
            INewRepoWindowViewModel newRepoWindowViewModel,
            INewRepoTypeSelectionPageViewModel newRepoTypeSelectionPageViewModel)
        {
            _activeViewModelsStack = new Stack<object>();

            InitializeComponent();

            newRepoWindowViewModel.NavigateForward += NewRepoWindowViewModel_NavigateForward;
            newRepoWindowViewModel.NavigateBackwards += NewRepoWindowViewModel_NavigateBackwards;

            DataContext = newRepoWindowViewModel;

            ContentFrame.Navigate(typeof(NewRepoTypeSelectionPage));
            ContentFrame.DataContext = newRepoTypeSelectionPageViewModel;
        }

        private void NewRepoWindowViewModel_NavigateForward(object sender, (Type xamlPage, INewRepoCreationPageViewModel viewModel) e)
        {
            _activeViewModelsStack.Push(ContentFrame.DataContext);
            ContentFrame.DataContext = e.viewModel;
            ContentFrame.Navigate(e.xamlPage, null, SlideInFromRightTransition);

            var pushBackButtonLeftAnimation = new ThicknessAnimation
            {
                From = new Thickness(BackButton.Margin.Left, BackButton.Margin.Top, BackButton.Margin.Right, BackButton.Margin.Bottom),
                To = new Thickness(BackButton.Margin.Left, BackButton.Margin.Top, BackButton.Margin.Right + DistanceToMoveBackButton, BackButton.Margin.Bottom),
                Duration = ButtonMovementDuration,
            };

            BackButton.BeginAnimation(MarginProperty, pushBackButtonLeftAnimation);
            BackButton.Content = "Back";

            var pushCreateButtonLeftAnimation = new ThicknessAnimation
            {
                From = new Thickness(CreateButton.Margin.Left, CreateButton.Margin.Top, CreateButton.Margin.Right, CreateButton.Margin.Bottom),
                To = new Thickness(CreateButton.Margin.Left - DistanceToMoveCreateButton, CreateButton.Margin.Top, CreateButton.Margin.Right, CreateButton.Margin.Bottom),
                Duration = ButtonMovementDuration,
            };

            CreateButton.BeginAnimation(MarginProperty, pushCreateButtonLeftAnimation);
        }

        private void NewRepoWindowViewModel_NavigateBackwards(object sender, EventArgs e)
        {
            ContentFrame.DataContext = _activeViewModelsStack.Pop();
            ContentFrame.GoBack();

            var pushBackButtonRightAnimation = new ThicknessAnimation
            {
                From = new Thickness(BackButton.Margin.Left, BackButton.Margin.Top, BackButton.Margin.Right, BackButton.Margin.Bottom),
                To = new Thickness(BackButton.Margin.Left, BackButton.Margin.Top, BackButton.Margin.Right - DistanceToMoveBackButton, BackButton.Margin.Bottom),
                Duration = ButtonMovementDuration,
            };

            BackButton.BeginAnimation(MarginProperty, pushBackButtonRightAnimation);
            BackButton.Content = "Close";

            var pushCreateButtonRightAnimation = new ThicknessAnimation
            {
                From = new Thickness(CreateButton.Margin.Left, CreateButton.Margin.Top, CreateButton.Margin.Right, CreateButton.Margin.Bottom),
                To = new Thickness(CreateButton.Margin.Left + DistanceToMoveCreateButton, CreateButton.Margin.Top, CreateButton.Margin.Right, CreateButton.Margin.Bottom),
                Duration = ButtonMovementDuration,
            };

            CreateButton.BeginAnimation(MarginProperty, pushCreateButtonRightAnimation);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ((INewRepoWindowViewModel)DataContext).NavigateForward -= NewRepoWindowViewModel_NavigateForward;
            ((INewRepoWindowViewModel)DataContext).NavigateBackwards -= NewRepoWindowViewModel_NavigateBackwards;
        }
    }
}