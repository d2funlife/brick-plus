using System.Collections.Generic;
using Windows.UI;

namespace Tetris.Models.Blocks
{
    public class O : BaseBlock
    {
        public O()
        {
            Initialize();
            CurrentType = GetType();
        }

        public O(Color blockColor) : this()
        {
            Color = blockColor;
        }

        public override List<int[,]> BlockStates => new List<int[,]>
        {
            new[,]
            {
                {0,0,0,0},
                {0,0,0,0},
                {0,1,1,0},
                {0,1,1,0}
            }
        };
    }
}