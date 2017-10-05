using GalaSoft.MvvmLight;
using Tetris.Enums;

namespace Tetris.Models.Core
{
    public class ControlMargin : ObservableObject
    {
        public ControlMargin() { }

        public ControlMargin(Margin margin)
        {
            Margin = margin;
            Description = new Windows.ApplicationModel.Resources.ResourceLoader().GetString(margin.ToString());
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { Set(() => IsSelected, ref isSelected, value); }
        }

        public Margin Margin
        {
            get { return margin; }
            set { Set(() => Margin, ref margin, value); }
        }

        public string Description
        {
            get { return description; }
            set { Set(() => Description, ref description, value); }
        }

        private bool isSelected;
        private Margin margin;
        private string description;
    }
}