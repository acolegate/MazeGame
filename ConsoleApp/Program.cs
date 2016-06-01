using System;
using System.Threading;

using MazeGame;

namespace ConsoleApp
{
    internal class Program
    {
        // http://pacman.wikia.com/wiki/Pac-Man_(game)

        private const int DisplayRefresDelayMilliseconds = 20; // 50 times per second

        private const char DotChar = '.';
        private const char PowerPillChar = 'O';
        private const char GhostChar = 'G';
        private const char PlayerChar = 'P';
        private const char EmptyChar = ' ';

        private static Engine _engine;

        private static Engine.GameState _gameState;

        private static readonly string[] MazeStrings = {
                                                           "┌────────────┐┌────────────┐",
                                                           "│............││............│",
                                                           "│.┌──┐.┌───┐.││.┌───┐.┌──┐.│",
                                                           "│O│  │.│   │.││.│   │.│  │O│",
                                                           "│.└──┘.└───┘.└┘.└───┘.└──┘.│",
                                                           "│..........................│",
                                                           "│.┌──┐.┌┐.┌──────┐.┌┐.┌──┐.│",
                                                           "│.└──┘.││.└──────┘.││.└──┘.│",
                                                           "│......││....┌┐....││......│",
                                                           "└────┐.│└──┐ ││ ┌──┘│.┌────┘",
                                                           "     │.│┌──┘ └┘ └──┐│.│     ",
                                                           "     │.││GGGGGGGGGG││.│     ",
                                                           "     │.││ ┌──────┐ ││.│     ",
                                                           "─────┘.└┘ │      │ └┘.└─────",
                                                           "      .   │      │   .      ",
                                                           "─────┐.┌┐ │      │ ┌┐.┌─────",
                                                           "     │.││ └──────┘ ││.│     ",
                                                           "     │.││          ││.│     ",
                                                           "     │.││ ┌──────┐ ││.│     ",
                                                           "┌────┘.└┘ └──┐┌──┘ └┘.└────┐",
                                                           "│............││............│",
                                                           "│.┌──┐.┌───┐.││.┌───┐.┌──┐.│",
                                                           "│.└─┐│.└───┘.└┘.└───┘.│┌─┘.│",
                                                           "│O..││.......P........││..O│",
                                                           "└─┐.││.┌┐.┌──────┐.┌┐.││.┌─┘",
                                                           "┌─┘.└┘.││.└──┐┌──┘.││.└┘.└─┐",
                                                           "│......││....││....││......│",
                                                           "│.┌────┘└──┐.││.┌──┘└────┐.│",
                                                           "│.└────────┘.└┘.└────────┘.│",
                                                           "│..........................│",
                                                           "└──────────────────────────┘"
                                                       };

        private static readonly ConsoleColor[] GhostColours = {
                                                                  ConsoleColor.Red,
                                                                  ConsoleColor.Magenta,
                                                                  ConsoleColor.Cyan,
                                                                  ConsoleColor.DarkYellow
                                                              };

        private const int MazeRowOffset = 2;

        private const ConsoleColor PlayerColour = ConsoleColor.Yellow;
        private const ConsoleColor DotColour = ConsoleColor.Yellow;
        private const ConsoleColor WallColour = ConsoleColor.Blue;

        private static void Main(string[] args)
        {
            Random random = new Random();

            _engine = new Engine(MazeStrings, random);

            _gameState = Engine.GameState.Running;

            RenderMaze();
            _engine.MazeData.ScoreUpdated += MazeData_ScoreUpdated;
            _engine.StartGame();

            do
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                _engine.MazeData.Player.Direction = Direction.North;
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                _engine.MazeData.Player.Direction = Direction.South;
                                break;
                            }

                        case ConsoleKey.RightArrow:
                            {
                                _engine.MazeData.Player.Direction = Direction.East;
                                break;
                            }

                        case ConsoleKey.LeftArrow:
                            {
                                _engine.MazeData.Player.Direction = Direction.West;
                                break;
                            }
                        case ConsoleKey.Escape:
                            {
                                _gameState = Engine.GameState.Quit;
                                break;
                            }
                    }
                }

                RenderEntities();

                Thread.Sleep(DisplayRefresDelayMilliseconds);
            }
            while (_gameState != Engine.GameState.Quit);
        }

        private static void MazeData_ScoreUpdated(object sender, ScoreUpdatedEventArgs args)
        {
            UpdateStats(args.LivesRemaining, args.Score);
        }

        private static void UpdateStats(int lives, int score)
        {
            // lives
            Console.SetCursorPosition(0, _engine.MazeData.MaxRowIndex + 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(lives);

            // score
            Console.SetCursorPosition(3, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("1UP   HIGH SCORE");
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(score.ToString().PadLeft(6));
        }

        private static void RenderMaze()
        {
            Console.Clear();

            Console.SetWindowSize(_engine.MazeData.MaxColumnIndex + 1, _engine.MazeData.MaxRowIndex + 4);

            for (int r = 0; r <= _engine.MazeData.MaxRowIndex; r++)
            {
                for (int c = 0; c <= _engine.MazeData.MaxColumnIndex; c++)
                {
                    ConsoleColor color = GetColourForBlock(_engine.MazeData.BlockAtPosition(r, c));

                    if (color != ConsoleColor.Black)
                    {
                        Console.ForegroundColor = color;
                        Console.SetCursorPosition(c, r + MazeRowOffset);
                        Console.Write(MazeStrings[r][c]);
                    }
                }
            }

            Console.ResetColor();
        }

        private static void RenderEntities()
        {
            if (_engine.MazeData.Player.Row != _engine.MazeData.Player.PreviousRow || _engine.MazeData.Player.Column != _engine.MazeData.Player.PreviousColumn)
            {
                Maze.BlockType previousBlock = _engine.MazeData.BlockAtPosition(_engine.MazeData.Player.PreviousRow, _engine.MazeData.Player.PreviousColumn);
                Console.ForegroundColor = GetColourForBlock(previousBlock);
                RenderChar(CharForBlockType(previousBlock), _engine.MazeData.Player.PreviousColumn, _engine.MazeData.Player.PreviousRow);
            }
            Console.ForegroundColor = PlayerColour;
            RenderChar(PlayerChar, _engine.MazeData.Player.Column, _engine.MazeData.Player.Row);

            for (int i = 0; i < _engine.MazeData.Ghosts.Count; i++)
            {
                Entity entity = _engine.MazeData.Ghosts[i];

                Maze.BlockType previousBlock = _engine.MazeData.BlockAtPosition(entity.PreviousRow, entity.PreviousColumn);
                Console.ForegroundColor = GetColourForBlock(previousBlock);
                RenderChar(CharForBlockType(previousBlock), entity.PreviousColumn, entity.PreviousRow);

                Console.ForegroundColor = GhostColours[i % 4];
                RenderChar(GhostChar, entity.Column, entity.Row);
            }
        }

        private static ConsoleColor GetColourForBlock(Maze.BlockType previousBlock)
        {
            switch (previousBlock)
            {
                case Maze.BlockType.Dot:
                case Maze.BlockType.PowerPill:
                    {
                        return DotColour;
                    }
                case Maze.BlockType.Wall:
                    {
                        return WallColour;
                    }

                default:
                    return ConsoleColor.Black;
            }
        }

        private static void RenderChar(char character, int column, int row)
        {
            Console.SetCursorPosition(column, row + MazeRowOffset);
            Console.Write(character);
        }

        private static char CharForBlockType(Maze.BlockType blockType)
        {
            switch (blockType)
            {
                case Maze.BlockType.Dot:
                    return DotChar;
                case Maze.BlockType.PowerPill:
                    return PowerPillChar;
                case Maze.BlockType.Empty:
                    return EmptyChar;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}