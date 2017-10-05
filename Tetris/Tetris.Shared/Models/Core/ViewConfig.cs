using System.Collections.Generic;
using Windows.UI;
using Tetris.Enums;
using Tetris.Models.Blocks;

namespace Tetris.Models.Core
{
    public class ViewConfig
    {
        public  ViewConfig(double blockSize, double previewBlockSize, int colorIndex = 0)
        {
            ColorIndex = colorIndex;
            BlockSize = blockSize;
            PreviewBlockSize = previewBlockSize;
            Colors = GameConfig.ColorShemes[ColorIndex];
            BlockTypes = new List<BaseBlock> {
                new I(Colors[(int) GameBlocks.I]), new L(Colors[(int) GameBlocks.L]), 
                new J(Colors[(int) GameBlocks.J]), new S(Colors[(int) GameBlocks.S]), 
                new Z(Colors[(int) GameBlocks.Z]), new O(Colors[(int) GameBlocks.O]), 
                new T(Colors[(int) GameBlocks.T]) };
        }

        public double BlockSize { get; set; }

        public double PreviewBlockSize { get; set; }

        public int ColorIndex { get; set; }

        public List<Color> Colors { get; set; }

        public List<BaseBlock> BlockTypes { get; set; }
    }
}