using System;

namespace Tetris.Models.Core
{
    public class GameResult
    {
        public int Number { get; set; }
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public string Time { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsLastResult { get; set; }
    }
}