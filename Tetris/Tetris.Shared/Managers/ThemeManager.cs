#region Usign statements
using System.Collections.ObjectModel;
using System.Linq;
using Tetris.Models.Core;
#endregion

namespace Tetris.Managers
{
    public static class ThemeManager
    {
        public static ObservableCollection<ThemeScheme> GetThemes()
        {
            var themes =  new ObservableCollection<ThemeScheme>();
            foreach (var theme in GameConfig.Themes)
                themes.Add(new ThemeScheme(theme));
            return themes;
        }
    }
}