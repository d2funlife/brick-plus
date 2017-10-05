using System;
using System.Collections.Generic;
using Windows.UI;

namespace Tetris.Contracts
{
    public interface IBlock
    {
        Type CurrentType { get; set; }
        int State { get; set; } 
        List<int[,]> BlockStates { get; } 
        int[,] BlockMatrix { get; set; }
        Color Color { get; }
        void Initialize();
        int GetNextBlockState();
        int[,] GetRotateMatrix();
        void Rotate();
    }
}