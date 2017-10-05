using System.Collections.Generic;
using Windows.UI;

namespace Tetris.Models.Blocks
{
    public class S : BaseBlock
    {
        public S()
        {
            Initialize();
            CurrentType = GetType();
        }

        public S(Color blockColor) : this()
        {
            Color = blockColor;
        }

        public override List<int[,]> BlockStates => new List<int[,]>
        {
            new [,]
            {
                {0,0,0,0},
                {0,0,1,0},
                {0,1,1,0},
                {0,1,0,0}
            },
            new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,1,1,0},
                {0,0,1,1}
            }};
    }
}