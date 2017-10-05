using System;
using System.Collections.Generic;
using Windows.UI;
using Tetris.Contracts;

namespace Tetris.Models.Blocks
{
    public abstract class BaseBlock : IBlock
    {
        public Type CurrentType { get; set; }

        public abstract List<int[,]> BlockStates { get; }

        public int[,] BlockMatrix { get; set; }

        public Color Color { get; set; }

        public int State { get; set; } 

        public void Initialize()
        {
            BlockMatrix = BlockStates[State];            
        }

        public void Rotate()
        {
            State = GetNextBlockState();
            BlockMatrix = BlockStates[State];            
        }

        public void Rotate(int stateNumber)
        {
            State = stateNumber;
            BlockMatrix = BlockStates[stateNumber];
        }

        public int GetNextBlockState()
        {
            return State >= BlockStates.Count - 1 ? 0 : State + 1;
        }

        public int[,] GetRotateMatrix()
        {
            return BlockStates[GetNextBlockState()];
        }
    }
}