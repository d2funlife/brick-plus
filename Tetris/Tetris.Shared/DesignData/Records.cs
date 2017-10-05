using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Tetris.Models.Core;

namespace Tetris.DesignData
{
    public class Records : ViewModelBase
    {
        private ObservableCollection<GameResult> gameResults;

        public ObservableCollection<GameResult> GameResults
        {
            get { return gameResults; }
            set { Set(() => GameResults, ref gameResults, value); }
        }


        public Records()
        {
            GameResults = new ObservableCollection<GameResult>
                {
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 1, PlayerName = "123456789", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 2, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 3, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 4, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 5, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 6, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 7, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 8, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 9, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 10, PlayerName = "d2funlife", Score = 3600, Time = "00:00"},
                    new GameResult {CreateTime = DateTime.Now, Level = 1, Number = 11, PlayerName = "d2funlife", Score = 3600, Time = "00:00"}
                };
        }
    }
}
