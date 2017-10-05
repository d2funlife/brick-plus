using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Tetris.Models.Core;
using Tetris.ViewModel;

namespace Tetris.Controls
{
    public sealed partial class RecordsMenu : UserControl
    {
        private bool isSwipeStarted;

        private double startPositionX;

        private Action switchRecordsToLeft;
        private Action switchrecordsToRight;

        public RecordsMenu()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            RecordsTable.PointerPressed += RecordsTableOnPointerPressed;
            RecordsTable.PointerReleased += RecordsTableOnPointerReleased;
            RecordsTable.PointerMoved += RecordsTableOnPointerMoved;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataContext = (GameViewModelVnext)DataContext;
            switchRecordsToLeft = dataContext.SwitchRecordsLeft;
            switchrecordsToRight = dataContext.SwitchRecordsRight;
        }

        private void RecordsTableOnPointerMoved(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (!isSwipeStarted) return;
            var pointerOnGrid = pointerRoutedEventArgs.GetCurrentPoint((UIElement)sender).Position;
            var deltaX = startPositionX - pointerOnGrid.X;
            var deltaXAbs = Math.Abs(deltaX);

            if (deltaXAbs < 50) return;

            if (deltaX > 50)
                switchrecordsToRight();
            if (deltaX < -50)
                switchRecordsToLeft();

            pointerRoutedEventArgs.Handled = true;
            isSwipeStarted = false;
        }

        private void RecordsTableOnPointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (!isSwipeStarted) return;
            isSwipeStarted = false;
        }

        private void RecordsTableOnPointerPressed(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            var pointerOnGrid = pointerRoutedEventArgs.GetCurrentPoint((UIElement)sender).Position;
            startPositionX = pointerOnGrid.X;
            isSwipeStarted = true;
        }
    }
}