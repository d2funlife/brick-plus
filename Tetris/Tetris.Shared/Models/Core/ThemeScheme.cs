using GalaSoft.MvvmLight;
using Tetris.Enums;

namespace Tetris.Models.Core
{
    public class ThemeScheme : ObservableObject
    {
        public ThemeScheme() { }

        public ThemeScheme(Theme theme)
        {
            Theme = theme;
            Description = new Windows.ApplicationModel.Resources.ResourceLoader().GetString(theme.ToString());
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { Set(() => IsSelected, ref isSelected, value); }
        }

        public Theme Theme
        {
            get { return theme; }
            set { Set(() => Theme, ref theme, value); }
        }

        public string Description
        {
            get { return description; }
            set { Set(() => Description, ref description, value); }
        }

        private bool isSelected;
        private Theme theme;
        private string description;
    }
}