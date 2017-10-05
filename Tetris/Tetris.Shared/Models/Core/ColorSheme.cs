using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;

namespace Tetris.Models.Core
{
    public class ColorSheme : ObservableObject
    {
        public ColorSheme() { }

        public ColorSheme(List<Color> colors)
        {
            FirstColor = new SolidColorBrush(colors[0]);
            SecondColor = new SolidColorBrush(colors[1]);
            ThirdColor = new SolidColorBrush(colors[2]);
            FourthColor = new SolidColorBrush(colors[3]);
            FifthColor = new SolidColorBrush(colors[4]);
            SixthColor = new SolidColorBrush(colors[5]);
            SeventhColor = new SolidColorBrush(colors[6]);
        }

        private bool isSelected; 
        
        public bool IsSelected
        {
            get { return isSelected; }
            set { Set(() => IsSelected, ref isSelected, value); }
        }

        public SolidColorBrush FirstColor { get; set; }
        public SolidColorBrush SecondColor { get; set; }
        public SolidColorBrush ThirdColor { get; set; }
        public SolidColorBrush FourthColor { get; set; }
        public SolidColorBrush FifthColor { get; set; }
        public SolidColorBrush SixthColor { get; set; }
        public SolidColorBrush SeventhColor { get; set; }

    }
}