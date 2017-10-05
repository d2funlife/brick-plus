using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Tetris.Controls;
using Tetris.Enums;
using Tetris.Helpers;
using Tetris.Models.Blocks;
using Tetris.Models.Core;
using Tetris.Models.Scene;
using Tetris.Models.Suspend;
using Windows.UI;

namespace Tetris.Engines
{
    public class GameEngine
    {
        public BlockUI[,] BackgroundBlocks { get; private set; }
        public BlockUI[,] MainBlocks { get; private set; }
        public ShadowBlockUI[,] ShadowBlocks { get; private set; }
        public BlockUI[,] NextBlock { get; private set; }
        public GameStatus GameStatus { get; private set; }
        public ViewConfig Config { get; set; }

        public event Action<int> OnScoreChanged;
        public event Action<int> OnLevelChanged;
        public event Action OnGameOver;

        private AudioEngine audioEngine;
        private Random randomNumberGenerator;
        private GridIndex currentIndex;
        private GridIndex shadowIndex;
        private BaseBlock currentBlock;
        private BaseBlock nextBlock;
        private bool isSpeadStable;
        private int speed;
        private LevelsNumber levelsNumber;
        private LevelsScore levelsScore;
        private int score;
        private int rowCount;
        private DispatcherTimer timer;
        private bool isShadowAvailable;

        public GameEngine(bool shadow, int colorIndex = 0)
        {
            InitializeParams(colorIndex, shadow);
            InitializeShadowBoxes();
            InitializeBackgroundBlocks();
            InitializeMainBlocks();
            InitializeNextBlock();
            InitializeGame();
        }

        private void InitializeParams(int colorIndex, bool shadow)
        {
            isShadowAvailable = shadow;
            shadowIndex = new GridIndex(3, 19);
            currentIndex = new GridIndex();
            randomNumberGenerator = new Random(DateTime.Now.Millisecond);
            Config = ResolutionHelper.GetViewConfig(colorIndex);
            audioEngine = new AudioEngine();
            speed = GameConfig.InitializationSpeed;
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(speed) };
            timer.Tick += (sender, o) => MoveToDown();
            levelsScore = LevelsScore.A;
            levelsNumber = LevelsNumber.A;
        }

        private void InitializeShadowBoxes()
        {
            ShadowBlocks = new ShadowBlockUI[GameConfig.RowsCount, GameConfig.ColumnsCount];
            for (var y = 3; y < GameConfig.RowsCount; y++)
            {
                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                {
                    var block = new ShadowBlockUI(Config.BlockSize);
                    block.Top = (y - 3) * block.rectangle.ActualHeight;
                    block.Left = x * block.rectangle.ActualWidth;
                    ShadowBlocks[y, x] = block;
                }
            }
        }

        private void InitializeBackgroundBlocks()
        {
            BackgroundBlocks = new BlockUI[GameConfig.RowsCount, GameConfig.ColumnsCount];
            for (var y = 3; y < GameConfig.RowsCount; y++)
            {
                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                {
                    var block = new BlockUI(Config.BlockSize);
                    block.Top = (y - 3) * block.rectangle.ActualHeight;
                    block.Left = x * block.rectangle.ActualWidth;
                    BackgroundBlocks[y, x] = block;
                }
            }
        }

        private void InitializeMainBlocks()
        {
            MainBlocks = new BlockUI[GameConfig.RowsCount, GameConfig.ColumnsCount];
            for (var y = 0; y < GameConfig.RowsCount; y++)
            {
                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                {
                    var block = new BlockUI(Config.BlockSize);
                    block.Top = (y - 3) * block.rectangle.ActualHeight;
                    block.Left = x * block.rectangle.ActualWidth;
                    block.Color = null;
                    MainBlocks[y, x] = block;
                }
            }
        }

        private void InitializeNextBlock()
        {
            NextBlock = new BlockUI[4, 4];
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    var block = new BlockUI(Config.PreviewBlockSize);
                    block.Top = x * block.rectangle.ActualHeight;
                    block.Left = y * block.rectangle.ActualWidth;
                    block.Color = null;
                    NextBlock[x, y] = block;
                }
            }
        }

        private void InitializeGame()
        {
            CreatePiece();
            AddPiece();
            GameStatus = GameStatus.Ready;
        }

        public void Play()
        {
            GameStatus = GameStatus.Play;
            AddShadow();
            timer.Start();
        }

        public void Pause()
        {
            GameStatus = GameStatus.Pause;
            timer.Stop();
        }

        public void Restart()
        {
            ResetAllBlocks();
            speed = GameConfig.InitializationSpeed;
            timer.Interval = TimeSpan.FromMilliseconds(speed);
            levelsNumber = LevelsNumber.A;
            levelsScore = LevelsScore.A;
            UpdateScore();
            UpdateLevel();
            InitializeGame();
            Play();
        }

        public void GameOver()
        {
            ClearShadow(0, 0);
            GameStatus = GameStatus.Over;
            timer.Stop();
            OnGameOver?.Invoke();
        }

        public void SetIsSpeedStable(bool value)
        {
            isSpeadStable = value;
        }

        public void MoveToLeft()
        {
            if (GameStatus != GameStatus.Play || IsLeftLimit()) return;
            RemovePiece();
            AddPiece(-1);
        }

        public void MoveToRight()
        {
            if (GameStatus != GameStatus.Play || IsRightLimit()) return;
            RemovePiece();
            AddPiece(1);
        }

        public void MoveToDown()
        {
            if (GameStatus != GameStatus.Play) return;

            if (IsDownLimit(1))
                audioEngine.PlayDropDownEffect();

            if (IsDownLimit())
            {
                audioEngine.UnlockDropDownEffect();
                RemoveRows();
                CreatePiece();
                AddPiece();
                score++;
                OnScoreChanged?.Invoke(score);
                return;
            }

            RemovePiece();
            AddPiece(0, 1);
        }

        public void MoveDropDown()
        {
            if (GameStatus != GameStatus.Play) return;
            while (!IsDownLimit())
            {
                RemovePiece();
                AddPiece(0, 1);
            }

            audioEngine.PlayDropDownEffect();
            audioEngine.UnlockDropDownEffect();
            RemoveRows();
            CreatePiece();
            AddPiece();
            score++;
            OnScoreChanged?.Invoke(score);
        }

        public void RotateMove()
        {
            if (GameStatus != GameStatus.Play) return;

            var rotateMatrix = currentBlock.GetRotateMatrix();
            if (IsBlockPlaceClear(rotateMatrix))
            {
                RotateBlock();
                return;
            }

            RemovePiece();
            var correctX = 0;
            for (var x = 3; x >= 0; x--)
            {
                for (var y = 3; y >= 0; y--)
                {
                    if (rotateMatrix[y, x] != 1) continue;
                    var rotateInLimit = IsXIndexInLimit(currentIndex.X + x);
                    if (rotateInLimit)
                    {
                        var rotateNotOverrides = MainBlocks[currentIndex.Y + y, currentIndex.X + x].Color == null;
                        if (rotateNotOverrides) continue;
                        var direction = (x < 2) ? 1 : -1;
                        if (!IsXIndexInLimit(currentIndex.X + x + 4 * direction))
                        {
                            AddPiece();
                            return;
                        }
                        if (MainBlocks[currentIndex.Y + y, currentIndex.X + x + 4 * direction].Color != null)
                        {
                            AddPiece();
                            return;
                        }
                        correctX++;
                        correctX *= direction;
                    }
                    else
                    {
                        var direction = (x < 2) ? 1 : -1;
                        if (MainBlocks[currentIndex.Y + y, currentIndex.X + x + 4 * direction].Color != null)
                        {
                            AddPiece();
                            return;
                        }
                        if (currentIndex.X + x == 11 &&
                            MainBlocks[currentIndex.Y + y, currentIndex.X + x + 4 * direction].Color == null)
                            correctX++;
                        correctX++;
                        correctX *= direction;
                        if (IsBlockPlaceClear(rotateMatrix, correctX)) RotateBlock(correctX);
                        return;
                    }

                }
            }

            if (IsBlockPlaceClear(rotateMatrix, correctX))
                RotateBlock(correctX);
        }

        private void CreatePiece()
        {
            //Check available space
            //If blocks with color - game over
            for (var x = 3; x < 7; x++)
            {
                if (MainBlocks[3, x].Color != null)
                {
                    GameOver();
                    return;
                }
            }

            currentBlock = nextBlock ?? Config.BlockTypes[randomNumberGenerator.Next(0, 7)];
            nextBlock = Config.BlockTypes[randomNumberGenerator.Next(0, 7)];
            if (nextBlock.CurrentType == currentBlock.CurrentType)
                nextBlock = Config.BlockTypes[randomNumberGenerator.Next(0, 7)];

            currentIndex.X = 3;
            currentIndex.Y = 0;
            SetNextBlock();
        }

        //Add piece on board
        private void AddPiece(int offsetX = 0, int offsetY = 0)
        {
            //Get shadow index
            //If block isn't move - no need to search index (-1)
            var yShadowIndex = (offsetX == 0 && offsetY == 1) ? -1 : GetDownLimit(offsetX, offsetY);
            ClearShadow(offsetX, offsetY);

            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (currentBlock.BlockMatrix[x, y] != 1) continue;
                    MainBlocks[x + currentIndex.Y + offsetY, y + currentIndex.X + offsetX].Color = currentBlock.Color;
                    AddShadow(offsetX, offsetY, yShadowIndex, x, y);
                }
            }

            currentIndex.X += offsetX;
            currentIndex.Y += offsetY;
        }

        private void RotateBlock(int xOfset = 0, int yOfset = 0)
        {
            RemovePiece();
            currentBlock.Rotate();
            AddPiece(xOfset, yOfset);
        }

        private void SetNextBlock()
        {
            ResetNextBlockColor();
            var emptyBlocks = 0;
            var xOffset = 0;
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (nextBlock.BlockMatrix[x, y] == 1)
                        NextBlock[x - xOffset, y].Color = nextBlock.Color;
                    else
                        emptyBlocks++;
                }
                if (emptyBlocks == 4) xOffset++;
                emptyBlocks = 0;
            }
        }

        private void RemovePiece()
        {
            for (var y = 0; y < 4; y++)
            {
                for (var x = 0; x < 4; x++)
                {
                    if (currentBlock.BlockMatrix[y, x] == 1)
                    {
                        MainBlocks[y + currentIndex.Y, x + currentIndex.X].Color = null;
                    }
                }
            }
        }

        private void RemoveRows()
        {
            var removeRowCount = 0;

            for (var y = 0; y < GameConfig.RowsCount; y++)
            {
                var isLine = true;

                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                {
                    if (MainBlocks[y, x].Color == null)
                    {
                        isLine = false;
                        break;
                    }
                }

                if (!isLine) continue;
                removeRowCount++;

                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                    MainBlocks[y, x].Color = null;

                for (var i = y; i > 0; i--)
                {
                    for (var x = 0; x < GameConfig.ColumnsCount; x++)
                    {
                        MainBlocks[i, x].Color = MainBlocks[i - 1, x].Color;
                    }
                }

            }
            if (removeRowCount == 0) return;
            rowCount += removeRowCount;
            score += removeRowCount * 10 * removeRowCount;
            OnScoreChanged?.Invoke(score);
            LevelUp();
            audioEngine.PlayRemoveRowEffect();
        }

        private void ResetAllBlocks()
        {
            ResetMainBlocksColor();
            ResetNextBlockColor();
            currentBlock = null;
            nextBlock = null;
        }

        private void ResetNextBlockColor()
        {
            foreach (var block in NextBlock)
                block.Color = null;
        }

        private void ResetMainBlocksColor()
        {
            foreach (var block in MainBlocks)
                block.Color = null;
        }

        private bool IsLeftLimit()
        {
            for (var y = 0; y < 4; y++)
            {
                for (var x = 0; x < 4; x++)
                {
                    if (currentBlock.BlockMatrix[y, x] != 1) continue;
                    if (x + currentIndex.X - 1 < 0) return true;
                    if (x != 0 && currentBlock.BlockMatrix[y, x - 1] == 1) continue;
                    if (MainBlocks[y + currentIndex.Y, x + currentIndex.X - 1].Color != null) return true;
                }
            }

            return false;
        }

        private bool IsRightLimit()
        {
            for (var x = 3; x >= 0; x--)
            {
                for (var y = 3; y >= 0; y--)
                {
                    if (currentBlock.BlockMatrix[y, x] != 1) continue;
                    if (x != 3 && currentBlock.BlockMatrix[y, x + 1] == 1) continue;
                    if (x + currentIndex.X + 1 > GameConfig.ColumnsCount - 1) return true;
                    if (MainBlocks[y + currentIndex.Y, x + currentIndex.X + 1].Color != null) return true;
                }
            }

            return false;
        }

        private bool IsDownLimit(int offsetY = 0)
        {
            if (currentIndex.Y + 4 + offsetY > GameConfig.RowsCount - 1) return true;
            for (var y = 3; y >= 0; y--)
            {
                for (var x = 3; x >= 0; x--)
                {
                    if (currentBlock.BlockMatrix[y, x] != 1) continue;
                    if (y != 3 && currentBlock.BlockMatrix[y + 1, x] == 1) continue;
                    if (MainBlocks[y + currentIndex.Y + offsetY + 1, x + currentIndex.X].Color != null)
                        return true;
                }
            }

            return false;
        }

        private int GetDownLimit(int offsetX, int offsetY)
        {
            if (GameStatus != GameStatus.Play) return 19;

            var result = 19;
            for (var y = 3; y >= 0; y--)
            {
                for (var x = 3; x >= 0; x--)
                {
                    if (currentBlock.BlockMatrix[y, x] != 1) continue;
                    if (y != 3 && currentBlock.BlockMatrix[y + 1, x] == 1)
                        continue;

                    for (var i = currentIndex.Y + offsetY + y; i < GameConfig.RowsCount; i++)
                    {
                        if (MainBlocks[i, currentIndex.X + x + offsetX].Color != null)
                        {
                            result = (result >= i - (y + 1)) ? i - (y + 1) : result;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool IsXIndexInLimit(int x)
        {
            return x < GameConfig.ColumnsCount && x >= 0;
        }

        private bool IsBlockPlaceClear(int[,] matrix, int offsetX = 0)
        {
            RemovePiece();
            var result = true;
            for (var y = 3; y >= 0; y--)
            {
                for (var x = 3; x >= 0; x--)
                {
                    if (matrix[y, x] != 1) continue;
                    if (currentIndex.X + x + offsetX > GameConfig.ColumnsCount - 1 || currentIndex.X + x + offsetX < 0)
                    {
                        result = false;
                        continue;
                    }
                    if (MainBlocks[currentIndex.Y + y, currentIndex.X + x + offsetX].Color != null)
                        result = false;
                }
            }
            AddPiece();
            return result;
        }

        public void SwitchBlocksColors(Dictionary<Color, Color> colors, int newIndex)
        {
            foreach (var block in MainBlocks)
            {
                if (block.Color == null) continue;
                block.Color = colors[block.Color.Value];
            }

            foreach (var block in NextBlock)
            {
                if (block.Color == null) continue;
                block.Color = colors[block.Color.Value];
            }

            foreach (var block in ShadowBlocks)
            {
                if (block == null) continue;
                if (block.Color == null) continue;
                block.Color = colors[block.Color.Value];
            }

            currentBlock.Color = colors[currentBlock.Color];
            nextBlock.Color = colors[nextBlock.Color];
            Config.Colors = GameConfig.ColorShemes[newIndex];
            Config.BlockTypes = new List<BaseBlock> {
                new I(Config.Colors[(int) GameBlocks.I]), new L(Config.Colors[(int) GameBlocks.L]),
                new J(Config.Colors[(int) GameBlocks.J]), new S(Config.Colors[(int) GameBlocks.S]),
                new Z(Config.Colors[(int) GameBlocks.Z]), new O(Config.Colors[(int) GameBlocks.O]),
                new T(Config.Colors[(int) GameBlocks.T]) };
        }

        public void SwitchShadow(bool shadow)
        {
            isShadowAvailable = shadow;
            ClearShadowWithouChecks();
        }

        private void ClearShadow(int offsetX, int offsetY)
        {
            //Check shdow available
            //Check is game plays
            //Check is block position not changed
            if (!isShadowAvailable) return;
            if (GameStatus != GameStatus.Play) return;
            if (offsetX == 0 && offsetY == 1) return;

            for (var y = 3; y < GameConfig.RowsCount; y++)
            {
                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                {
                    ShadowBlocks[y, x].Color = null;
                }
            }

        }

        private void ClearShadowWithouChecks()
        {
            for (var y = 3; y < GameConfig.RowsCount; y++)
            {
                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                {
                    ShadowBlocks[y, x].Color = null;
                }
            }
        }

        private void AddShadow(int offsetX, int offsetY, int yShadowIndex, int x, int y)
        {
            //Check shdow available
            //Check is game plays
            //Check is block position not changed
            if (!isShadowAvailable) return;
            if (GameStatus != GameStatus.Play) return;
            if (offsetX == 0 && offsetY == 1) return;

            //Check is shadowIndex and offsets in limits
            if (x + yShadowIndex > 22 ||
                y + currentIndex.X + offsetX < 0 ||
                y + currentIndex.X + offsetX > 9 ||
                x + yShadowIndex < 3) return;

            ShadowBlocks[x + yShadowIndex, y + currentIndex.X + offsetX].Color = currentBlock.Color;
        }

        private void AddShadow()
        {
            if (!isShadowAvailable) return;
            if (GameStatus != GameStatus.Play) return;

            ClearShadowWithouChecks();

            var yShadowIndex = GetDownLimit(0, 1);

            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (currentBlock.BlockMatrix[x, y] != 1) continue;

                    if (x + yShadowIndex > 22 ||
                        y + currentIndex.X < 0 ||
                        y + currentIndex.X > 9 ||
                        x + yShadowIndex < 3) continue;

                    ShadowBlocks[x + yShadowIndex, y + currentIndex.X].Color = currentBlock.Color;
                }
            }
        }

        private void LevelUp()
        {
            if (score < (int)levelsScore) return;
            if (levelsScore == LevelsScore.Q) return;

            levelsNumber++;
            levelsScore = (LevelsScore)Enum.Parse(typeof(LevelsScore), levelsNumber.ToString());
            UpdateLevel();
            if (isSpeadStable) return;
            if (levelsScore == LevelsScore.Q) speed = 200; else speed -= 15;
            timer.Interval = TimeSpan.FromMilliseconds(speed);
        }

        private void UpdateLevel()
        {
            OnLevelChanged?.Invoke((int)levelsNumber);
        }

        private void UpdateScore(int value = 0)
        {
            score = value;
            OnScoreChanged?.Invoke(score);
        }

        public void SwitchAduioAvailable(bool isAvalable)
        {
            audioEngine.SwitchAudioAvailable(isAvalable);
        }

        public SuspendingGameEngine GetSuspending()
        {
            if (GameStatus == GameStatus.Over) return null;

            var suspending = new SuspendingGameEngine
            {
                CurrentBlock = new SuspendingBlock(currentBlock),
                NextBlock = new SuspendingBlock(nextBlock),
                CurrentIndex = currentIndex,
                GameStatus = GameStatus,
                Level = (int)levelsNumber,
                Score = score,
                Speed = speed,
            };

            for (var y = 0; y < GameConfig.RowsCount; y++)
                for (var x = 0; x < GameConfig.ColumnsCount; x++)
                    if (MainBlocks[y, x].Color != null)
                        suspending.MainBlocks.Add(new SuspendingBoardBlock(x, y, Config.Colors.IndexOf((Color)MainBlocks[y, x].Color)));

            for (var y = 0; y < 4; y++)
                for (var x = 0; x < 4; x++)
                    if (NextBlock[y, x].Color != null)
                        suspending.NextBlocks.Add(new SuspendingBoardBlock(x, y, Config.Colors.IndexOf((Color)NextBlock[y, x].Color)));

            return suspending;
        }

        public void SetSuspending(SuspendingGameEngine game)
        {
            if (game == null) return;

            ResetNextBlockColor();
            RemovePiece();

            foreach (var block in game.MainBlocks)
                MainBlocks[block.GridIndex.Y, block.GridIndex.X].Color = Config.Colors[block.ColorIndex];
            foreach (var block in game.NextBlocks)
                NextBlock[block.GridIndex.Y, block.GridIndex.X].Color = Config.Colors[block.ColorIndex];

            currentBlock = GetBaseBlock(game.CurrentBlock);
            nextBlock = GetBaseBlock(game.NextBlock);

            currentIndex = game.CurrentIndex;
            GameStatus = game.GameStatus;

            levelsNumber = (LevelsNumber)game.Level;
            levelsScore = (LevelsScore)Enum.Parse(typeof(LevelsScore), levelsNumber.ToString());
            UpdateScore(game.Score);
            UpdateLevel();
            speed = game.Speed;

            timer.Interval = TimeSpan.FromMilliseconds(speed);
        }

        private BaseBlock GetBaseBlock(SuspendingBlock block)
        {
            var baseBlock = (BaseBlock)Activator.CreateInstance(Type.GetType(block.Type));
            baseBlock.Color = Config.Colors[(int)Enum.Parse(typeof(GameBlocks), baseBlock.GetType().Name)];
            baseBlock.Rotate(block.State);
            return baseBlock;
        }
    }
}