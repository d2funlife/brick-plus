using System.Collections.Generic;
using Tetris.Enums;
using Tetris.Models.Scene;

namespace Tetris.Models.Suspend
{
    //OLD
    public class SuspendingGame
    {
        public SuspendingGame()
        {
            MainBlocks = new List<SuspendingBoardBlock>();
            NextBlocks = new List<SuspendingBoardBlock>();
        }

        public List<SuspendingBoardBlock> MainBlocks { get; set; }

        public List<SuspendingBoardBlock> NextBlocks { get; set; }

        public bool ContinueAvailable { get; set; }

        public GameStatus GameStatus { get; set; }

        public int Score { get; set; }

        public int Level { get; set; }

        public string Time { get; set; }

        public int Speed { get; set; }
        
        public int SecondCount { get; set; }

        public GridIndex CurrentIndex { get; set; }

        public SuspendingBlock CurrentBlock { get; set; }

        public SuspendingBlock NextBlock { get; set; }
    }
}