namespace Tetris.Models.Scene
{
    public class GridIndex
    {
        public GridIndex() { }

        public GridIndex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}