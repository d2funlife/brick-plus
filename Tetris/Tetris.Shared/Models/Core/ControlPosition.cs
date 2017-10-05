using GalaSoft.MvvmLight;
using Tetris.Enums;

namespace Tetris.Models.Core
{
    public class ControlPosition : ObservableObject
    {
        public ControlPosition() { }

        public ControlPosition(Position position)
        {
            Position = position;
            Description = new Windows.ApplicationModel.Resources.ResourceLoader().GetString(position.ToString());
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { Set(() => IsSelected, ref isSelected, value); }
        }

        public Position Position
        {
            get { return position; }
            set { Set(() => Position, ref position, value); }
        }
        
        public string Description
        {
            get { return description; }
            set { Set(() => Description, ref description, value); }
        }

        private bool isSelected;
        private Position position;
        private string description;
    }
}