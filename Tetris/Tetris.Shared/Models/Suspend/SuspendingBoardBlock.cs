using Tetris.Models.Scene;

namespace Tetris.Models.Suspend
{
    public class SuspendingBoardBlock
    {
        public SuspendingBoardBlock()
        {
        }

        public SuspendingBoardBlock(int x, int y, int colorIndex)
        {
            GridIndex = new GridIndex(x, y);
            ColorIndex = colorIndex;
        }

        public GridIndex GridIndex { get; set; }
        public int ColorIndex { get; set; }
    }
}