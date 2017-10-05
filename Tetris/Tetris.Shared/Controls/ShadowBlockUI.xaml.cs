using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Tetris.Controls
{
    public sealed partial class ShadowBlockUI : UserControl
    {
        public ShadowBlockUI()
        {
            InitializeComponent();
        }

        public ShadowBlockUI(double blocksize) :this()
        {
            ResizeBlock(blocksize);
        }

        public void ResizeBlock(double blocksize)
        {
            rectangle.Width = blocksize;
            rectangle.Height = blocksize;
        }

        public double Top
        {
            set
            {
                rectangle.SetValue(Canvas.TopProperty, value);
            }
        }

        public double Left
        {
            set
            {
                rectangle.SetValue(Canvas.LeftProperty, value);
            }
        }

        public Color? Color
        {
            set
            {
                if (value == null)
                {
                    rectangle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    rectangle.Fill = new SolidColorBrush((Color)value);
                    rectangle.Visibility = Visibility.Visible;
                }
            }
            get
            {
                if (rectangle.Visibility == Visibility.Collapsed)
                    return null;
                if (rectangle.Fill == null)
                    return null;
                return ((SolidColorBrush)rectangle.Fill).Color;
            }
        }
    }
}
