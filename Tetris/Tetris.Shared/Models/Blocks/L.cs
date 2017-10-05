using System.Collections.Generic;
using Windows.UI;

namespace Tetris.Models.Blocks
{
    public class L : BaseBlock
    {
        public L()
        {
            Initialize();
            CurrentType = GetType();
        }

        public L(Color blcokColor) : this()
        {
            Color = blcokColor;
        }

        public override List<int[,]> BlockStates => new List<int[,]>
        {
            new[,]
            {
                {0,0,0,0},
                {0,1,0,0},
                {0,1,0,0},
                {0,1,1,0}
            },
            new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,1,1,1},
                {0,1,0,0}
            },
            new[,]
            {
                {0,0,0,0},
                {0,1,1,0},
                {0,0,1,0},
                {0,0,1,0}
            },
            new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,0,1,0},
                {1,1,1,0}
            }
        };
    }
}