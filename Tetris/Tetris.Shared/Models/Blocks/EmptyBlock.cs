using System.Collections.Generic;

namespace Tetris.Models.Blocks
{
    public class EmptyBlock : BaseBlock
    {
        public override List<int[,]> BlockStates => null;
    }
}