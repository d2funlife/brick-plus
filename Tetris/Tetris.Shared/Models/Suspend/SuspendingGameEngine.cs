using System.Collections.Generic;
using Tetris.Enums;
using Tetris.Models.Scene;

namespace Tetris.Models.Suspend
{
    public class SuspendingGameEngine
    {
        public SuspendingGameEngine()
        {
            MainBlocks = new List<SuspendingBoardBlock>();
            NextBlocks = new List<SuspendingBoardBlock>();
        }

        public List<SuspendingBoardBlock> MainBlocks { get; set; }
        public List<SuspendingBoardBlock> NextBlocks { get; set; }
        public Mode GameMode { get; set; }
        public GameStatus GameStatus { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public int Speed { get; set; }
        public GridIndex CurrentIndex { get; set; }
        public SuspendingBlock CurrentBlock { get; set; }
        public SuspendingBlock NextBlock { get; set; }
    }
}