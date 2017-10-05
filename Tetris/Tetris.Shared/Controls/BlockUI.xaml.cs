#region Using statements

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using Tetris.Enums;

#endregion

namespace Tetris.Controls
{
    public partial class BlockUI : UserControl
    {
        public BlockUI()
        {
            InitializeComponent();
        }

        public BlockUI(double blocksize) : this()
        {
            ResizeBlock(blocksize);
        }

        private void ResizeBlock(double blocksize)
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
                return ((SolidColorBrush)rectangle.Fill).Color;
            }
        }

        public void UpdateSize(double size)
        {
            rectangle.Height = size;
            rectangle.Width = size;
        }
    }
}