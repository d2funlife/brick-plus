using GalaSoft.MvvmLight.Command;
using Tetris.Controls;
using Tetris.Enums;

namespace Tetris.Contracts
{
    public interface IGameViewModel
    {
        BlockUI[,] BackgroundBlocks { get; set; }
        BlockUI[,] MainBlocks { get; set; }
        BlockUI[,] NextBlock { get; set; }
        GameStatus GameStatus { get; set; }
        int Score { get; set; }
        int Level { get; set; }
        string Time { get; set; }
        void Play();
        void Pause();
        RelayCommand MoveToLeftCommand { get; set; }
        RelayCommand MoveToRightCommand { get; set; }
        RelayCommand MoveToDownCommand { get; set; }
        RelayCommand RotateCommand { get; set; }
        RelayCommand CompletlyMoveToLeftCommand { get; set; }
        RelayCommand CompletlyMoveToRightCommand { get; set; }
        RelayCommand CompletelyMoveToDownCommand { get; set; }
        RelayCommand PointerReleasedCommand { get; set; }
    }
}