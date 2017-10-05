using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Tetris.Common;
using Tetris.ViewModel;

namespace Tetris.Pages
{
    public sealed partial class Game : Page
    {
        private GameViewModelVnext gameEngine;
        private NavigationHelper navigationHelper;

        public Game()
        {
            InitializeComponent();
            InitializePageEvents();
            InitializeGame();
            InitializeNavigationHelper();
            InitializeSwype();
        }

        private void InitializePageEvents()
        {
            Window.Current.CoreWindow.Activated += (sender, args) =>
            {
                if (args.WindowActivationState == CoreWindowActivationState.Deactivated && MainMenu.Visibility == Visibility.Collapsed)
                {
                    gameEngine.PauseGame();
                    gameEngine.IsMenuAvailable = true;
                }
            };
            Window.Current.CoreWindow.Closed += (sender, args) => gameEngine.SaveStateOnClose();
            GameResult.Nick.KeyUp += (sender, args) => { if (args.Key == VirtualKey.Enter) Focus(FocusState.Pointer); };
        }

        private void InitializeGame()
        {
            gameEngine = new GameViewModelVnext();
            DataContext = gameEngine;

            for (var y = 3; y < GameConfig.RowsCount; y++)
                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                {
                    MainBoardCanvas.Children.Add(gameEngine.BackgroundBlocks[y, x]);
                    MainBoardCanvas.Children.Add(gameEngine.MainBlocks[y, x]);
                    MainBoardCanvas.Children.Add(gameEngine.ShadowBlocks[y, x]);
                }

            foreach (var block in gameEngine.NextBlock)
                NextBoardCanvas.Children.Add(block);
        }

        private void InitializeNavigationHelper()
        {
            navigationHelper = new NavigationHelper(this, gameEngine.LoadState, gameEngine.SaveState);
        }

        private void InitializeSwype()
        {
            MainBoardGrid.PointerPressed += gameEngine.SwipeStart;
            MainBoardGrid.PointerMoved += gameEngine.SwipeMove;
            MainBoardGrid.PointerReleased += gameEngine.SwipeEnd;
            MainBoardBottom.Tapped += gameEngine.SwipeTap;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
    }
}