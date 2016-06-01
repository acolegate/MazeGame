using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGame
{
    public class Maze
    {
        public delegate void ScoreUpdatedEventHandler(object sender, ScoreUpdatedEventArgs args);

        public enum BlockType
        {
            Empty,
            Wall,
            Dot,
            PowerPill,
            Ghost,
            Player
        }

        public enum FruitType
        {
            Cherry = 100,
            Strawberry = 300,
            Orange = 500,
            Apple = 700,
            Melon = 1000,
            GalxianBoss = 2000,
            Bell = 3000,
            Key = 5000,
        }

        public enum GhostNames
        {
            Blinky = 0,
            Pinky = 1,
            Inky = 2,
            Clyde = 3
        }

        internal const char DotChar = '.';
        internal const char PowerPillChar = 'O';
        internal const char EmptyChar = ' ';
        internal const char GhostChar = 'G';
        internal const char PlayerChar = 'P';

        internal const Direction PlayerDefaultDirection = Direction.West;

        internal const int PlayerDefaultSpeed = 2;
        internal const int PlayerFastSpeed = 3;
        internal const int GhostDefaultSpeed = 2;
        internal const int GhostSlowSpeed = 1;
        internal const int ScentTrailLength = 8;

        internal const int DotPoints = 10;
        internal const int PowerPillPoints = 50;
        internal const int StartingLives = 3;
        internal const int BonusLifeAtScore = 10000;

        internal const int Ghost1EatenPoints = 200;
        internal const int Ghost2EatenPoints = 400;
        internal const int Ghost3EatenPoints = 800;
        internal const int Ghost4EatenPoints = 1600;

        /*
        Cherry         100 points
        Strawberry     300 points
        Orange         500 points
        Apple          700 points
        Melon         1000 points
        Galxian Boss  2000 points
        Bell          3000 points
        Key           5000 points
        */

        /*
        Red ghost, Blinky, doggedly pursues Pac-Man;
        Pink ghost, Pinky, tries to ambush Pac-Man by moving parallel to him;
        Cyan ghost, Inky, tends not to chase Pac-Man directly unless Blinky is near;
        Orange ghost, Clyde, pursues Pac-Man when far from him, but usually wanders away when he gets close.
        */

        // ReSharper disable once InconsistentNaming
        internal readonly Random _random;

        internal readonly Block[,] Blocks;

        public List<Entity> Ghosts;
        public int LivesRemaining;
        public Entity Player;

        internal Dictionary<int, int> ScentMap;
        public int Score;

        public Maze(string[] strings, Random random)
        {
            _random = random;
            MaxRowIndex = strings.GetUpperBound(0);
            MaxColumnIndex = strings[0].Length - 1;

            Blocks = MakeMaze(strings);
            ScentMap = new Dictionary<int, int>();
            Score = 0;
            LivesRemaining = StartingLives - 1;

            if (Ghosts.Any() == false)
            {
                throw new ApplicationException("No ghosts defined in maze");
            }

            if (Player == null)
            {
                throw new ApplicationException("No player defined in maze");
            }

            if (DotsRemaining == 0)
            {
                throw new ApplicationException("No dots placed in maze");
            }

            // now we have a maze built, point the ghosts in a random direction
            foreach (Entity ghost in Ghosts)
            {
                ghost.Direction = ChooseRandomDirection(AvailableDirections(ghost.Row, ghost.Column));
            }
        }

        public int DotsRemaining { get; private set; }
        public int MaxRowIndex { get; }
        public int MaxColumnIndex { get; }
        public event ScoreUpdatedEventHandler ScoreUpdated;

        internal Block[,] MakeMaze(string[] mazeStrings)
        {
            Block[,] blocks = new Block[MaxRowIndex + 1, MaxColumnIndex + 1];

            Ghosts = new List<Entity>();
            DotsRemaining = 0;
            for (int y = 0; y <= MaxRowIndex; y++)
            {
                string s = mazeStrings[y];
                for (int x = 0; x <= MaxColumnIndex; x++)
                {
                    BlockType blockType = EvaluateMazeBlock(s[x]);

                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (blockType)
                    {
                        case BlockType.Dot:
                            {
                                DotsRemaining++;
                                blocks[y, x] = new Block(blockType);
                                break;
                            }
                        case BlockType.Empty:
                        case BlockType.PowerPill:
                        case BlockType.Wall:
                            {
                                blocks[y, x] = new Block(blockType);
                                break;
                            }
                        case BlockType.Player:
                            {
                                blocks[y, x] = new Block(BlockType.Empty);
                                Player = new Entity(y, x, PlayerDefaultSpeed, PlayerDefaultDirection);
                                break;
                            }
                        case BlockType.Ghost:
                            {
                                blocks[y, x] = new Block(BlockType.Empty);
                                Ghosts.Add(new Entity(y, x, GhostDefaultSpeed, RandomDirection()));
                                break;
                            }
                    }
                }
            }

            return blocks;
        }

        internal Direction RandomDirection()
        {
            return (Direction)_random.Next(0, 3 + 1);
        }

        internal static BlockType EvaluateMazeBlock(char c)
        {
            switch (c)
            {
                case DotChar:
                    {
                        return BlockType.Dot;
                    }
                case PowerPillChar:
                    {
                        return BlockType.PowerPill;
                    }
                case GhostChar:
                    {
                        return BlockType.Ghost;
                    }
                case PlayerChar:
                    {
                        return BlockType.Player;
                    }
                case EmptyChar:
                    {
                        return BlockType.Empty;
                    }
                default:
                    {
                        return BlockType.Wall;
                    }
            }
        }

        internal List<Direction> AvailableDirections(int row, int column)
        {
            List<Direction> directions = new List<Direction>();

            if (DirectionIsAvailable(row, column, Direction.North))
            {
                directions.Add(Direction.North);
            }
            if (DirectionIsAvailable(row, column, Direction.East))
            {
                directions.Add(Direction.East);
            }
            if (DirectionIsAvailable(row, column, Direction.South))
            {
                directions.Add(Direction.South);
            }
            if (DirectionIsAvailable(row, column, Direction.West))
            {
                directions.Add(Direction.West);
            }

            return directions;
        }

        public void MoveGhost(Entity ghost)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            ChangePosition(ghost);

            List<Direction> availableDirections = AvailableDirections(ghost.Row, ghost.Column);

            if (availableDirections.Count > 1)
            {
                // Remove the reciprocal direction to stop the ghost from doubling back on itself
                availableDirections.Remove(Reciprocal(ghost.Direction));
            }

            // pick a random direction
            ghost.Direction = ChooseRandomDirection(availableDirections);
        }

        internal Direction Reciprocal(Direction direction)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (direction)
            {
                case Direction.East:
                    {
                        return Direction.West;
                    }
                case Direction.North:
                    {
                        return Direction.South;
                    }
                case Direction.South:
                    {
                        return Direction.North;
                    }
                case Direction.West:
                default:
                    {
                        return Direction.East;
                    }
            }
        }

        public void MovePlayer()
        {
            List<Direction> availableDirections = AvailableDirections(Player.Row, Player.Column);

            if (availableDirections.Contains(Player.Direction))
            {
                // player can continue going in the same direction
                ChangePosition(Player);
            }
            else
            {
                // player has hit a wall
                switch (availableDirections.Count)
                {
                    case 2:
                        // they can go in one of two directions
                        // remove their reciprocal direction so the player doesn't double back on themselves and instead goes round a corner
                        availableDirections.Remove(Reciprocal(Player.Direction));

                        Player.Direction = availableDirections[0];

                        ChangePosition(Player);
                        break;
                    case 1:
                        // the player can only go in one direction
                        Player.Direction = availableDirections[0];
                        ChangePosition(Player);
                        break;
                }
            }

            AddToScentMap(Player.Column, Player.Row);

            DegradeScentMap();

            BlockType blockUnderPlayer = BlockAtPosition(Player.Row, Player.Column);

            if (blockUnderPlayer == BlockType.Dot || blockUnderPlayer == BlockType.PowerPill)
            {
                if (blockUnderPlayer == BlockType.Dot)
                {
                    Score += DotPoints;
                }
                else
                {
                    Score += PowerPillPoints;
                }

                Blocks[Player.Row, Player.Column].BlockType = BlockType.Empty;

                RaiseScoreUpdatedEvent();
            }
        }

        internal void RaiseScoreUpdatedEvent()
        {
            ScoreUpdated?.Invoke(this, new ScoreUpdatedEventArgs
                                           {
                                               LivesRemaining = LivesRemaining,
                                               Score = Score
                                           });
        }

        internal void AddToScentMap(int column, int row)
        {
            int key = (row * MaxRowIndex) + column;

            if (ScentMap.ContainsKey(key))
            {
                ScentMap[key] = ScentTrailLength;
            }
            else
            {
                ScentMap.Add(key, ScentTrailLength);
            }
        }

        internal void DegradeScentMap()
        {
            foreach (int key in ScentMap.Keys.ToArray())
            {
                ScentMap[key]--;

                if (ScentMap[key] <= 0)
                {
                    ScentMap.Remove(key);
                }
            }
        }

        internal void ChangePosition(Entity entity)
        {
            entity.PreviousRow = entity.Row;
            entity.PreviousColumn = entity.Column;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (entity.Direction)
            {
                case Direction.North:
                    {
                        if (entity.Row > 0)
                        {
                            entity.Row--;
                        }
                        else
                        {
                            entity.Row = MaxRowIndex;
                        }
                        break;
                    }
                case Direction.East:
                    {
                        if (entity.Column < MaxColumnIndex)
                        {
                            entity.Column++;
                        }
                        else
                        {
                            entity.Column = 0;
                        }
                        break;
                    }
                case Direction.South:
                    {
                        if (entity.Row < MaxRowIndex)
                        {
                            entity.Row++;
                        }
                        else
                        {
                            entity.Row = 0;
                        }
                        break;
                    }
                case Direction.West:
                    {
                        if (entity.Column > 0)
                        {
                            entity.Column--;
                        }
                        else
                        {
                            entity.Column = MaxColumnIndex;
                        }
                        break;
                    }
            }
        }

        internal Direction ChooseRandomDirection(IReadOnlyList<Direction> directions)
        {
            // Use Directions.Count here because Random.Next(intMin, intMax) only ever goes up to intMax-1
            return directions[directions.Count == 1 ? 0 : _random.Next(0, directions.Count)];
        }

        internal bool DirectionIsAvailable(int row, int column, Direction direction)
        {
            bool returnValue = false;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (direction)
            {
                case Direction.North:
                    {
                        if (row == 0)
                        {
                            returnValue = Blocks[MaxRowIndex, column].BlockType != BlockType.Wall;
                        }
                        else
                        {
                            returnValue = Blocks[row - 1, column].BlockType != BlockType.Wall;
                        }
                        break;
                    }

                case Direction.East:
                    {
                        if (column == MaxColumnIndex)
                        {
                            returnValue = Blocks[row, 0].BlockType != BlockType.Wall;
                        }
                        else
                        {
                            returnValue = Blocks[row, column + 1].BlockType != BlockType.Wall;
                        }
                        break;
                    }
                case Direction.South:
                    {
                        if (row == MaxRowIndex)
                        {
                            returnValue = Blocks[0, column].BlockType != BlockType.Wall;
                        }
                        else
                        {
                            returnValue = Blocks[row + 1, column].BlockType != BlockType.Wall;
                        }
                        break;
                    }
                case Direction.West:
                    {
                        returnValue = column == 0 ? Blocks[row, MaxColumnIndex].BlockType != BlockType.Wall : Blocks[row, column - 1].BlockType != BlockType.Wall;
                        break;
                    }
            }

            return returnValue;
        }

        public BlockType BlockAtPosition(int row, int column)
        {
            return Blocks[row, column].BlockType;
        }

        public class Block
        {
            public Block(BlockType blockType, float trailStrength = 0)
            {
                BlockType = blockType;
                TrailStrength = trailStrength;
            }

            public BlockType BlockType { get; set; }
            public float TrailStrength { get; }
        }
    }

    public class ScoreUpdatedEventArgs
    {
        public int LivesRemaining { get; set; }
        public int Score { get; set; }
    }
}