using Tetris.Models.Blocks;

namespace Tetris.Models.Suspend
{
    public class SuspendingBlock
    {
        public string Type { get; set; }
        public int State { get; set; }
        public SuspendingBlock() { }
        public SuspendingBlock(BaseBlock block)
        {
            Type = block.CurrentType.FullName;
            State = block.State;
        }
    }
}