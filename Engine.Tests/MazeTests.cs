using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using MazeGame;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MazeTests
    {
        private readonly string[] _testStrings = {
                                                     "####.###",
                                                     "#G.#.#G#",
                                                     "####.#.#",
                                                     "...#G..O",
                                                     "#.##.#.#",
                                                     "#P.#   #",
                                                     "#### ###",
                                                 };

        private Maze _classUnderTest;
        private Random _random;

        [TestInitialize]
        public void Initialise()
        {
            _random = new Random(0);

            _classUnderTest = new Maze(_testStrings, _random);
        }

        [TestMethod]
        public void Maze_AddToScentMap_DoesntAlreadyExist_AddsToScentMap()
        {
            // arrange
            const int Key = 31;

            // Assert
            Assert.AreEqual(0, _classUnderTest.ScentMap.Count, "Unexpected initial state");

            _classUnderTest.AddToScentMap(1, 5);

            // Assert
            Assert.IsTrue(_classUnderTest.ScentMap.Count == 1 && _classUnderTest.ScentMap[Key] == Maze.ScentTrailLength, "Unexpected scent trail");
        }

        [TestMethod]
        public void Maze_AddToScentMap_AlreadyExists_UpdatedExistingValue()
        {
            // arrange
            _classUnderTest.ScentMap = new Dictionary<int, int>
                                           {
                                               { 9, 8 },
                                               { 10, 7 },
                                               { 11, 6 },
                                               { 12, 5 },
                                               { 13, 4 },
                                               { 14, 3 }
                                           };

            _classUnderTest.AddToScentMap(6, 1);

            // Assert
            Assert.AreEqual(6, _classUnderTest.ScentMap.Count, "Unexpected scent marks");

            Assert.AreEqual(8, _classUnderTest.ScentMap[9]);
            Assert.AreEqual(7, _classUnderTest.ScentMap[10]);
            Assert.AreEqual(6, _classUnderTest.ScentMap[11]);
            Assert.AreEqual(8, _classUnderTest.ScentMap[12]); // 8 again
            Assert.AreEqual(4, _classUnderTest.ScentMap[13]);
            Assert.AreEqual(3, _classUnderTest.ScentMap[14]);
        }

        [TestMethod]
        public void Maze_DegradeScentMap_DegradesScentmap()
        {
            // arrange
            _classUnderTest.ScentMap = new Dictionary<int, int>
                                           {
                                               { 1, 8 },
                                               { 2, 7 },
                                               { 3, 6 },
                                               { 4, 5 },
                                               { 5, 4 },
                                               { 6, 3 },
                                               { 7, 2 },
                                               { 8, 1 }
                                           };

            _classUnderTest.DegradeScentMap();

            // Assert
            Assert.AreEqual(7, _classUnderTest.ScentMap.Count, "Unexpected scent marks");

            Assert.AreEqual(7, _classUnderTest.ScentMap[1]);
            Assert.AreEqual(6, _classUnderTest.ScentMap[2]);
            Assert.AreEqual(5, _classUnderTest.ScentMap[3]);
            Assert.AreEqual(4, _classUnderTest.ScentMap[4]);
            Assert.AreEqual(3, _classUnderTest.ScentMap[5]);
            Assert.AreEqual(2, _classUnderTest.ScentMap[6]);
            Assert.AreEqual(1, _classUnderTest.ScentMap[7]);
        }

        [TestMethod]
        public void Maze_MovePlayer()
        {
            // Initial state
            Assert.IsTrue(_classUnderTest.Player.Column == 1 && _classUnderTest.Player.Row == 5 && _classUnderTest.Player.Direction == Direction.West && _classUnderTest.Player.PreviousColumn == 1 && _classUnderTest.Player.PreviousRow == 5, "Unexpected player position and direction");

            // act/Assert
            // Move North
            _classUnderTest.MovePlayer();
            Assert.IsTrue(_classUnderTest.Player.Column == 1 && _classUnderTest.Player.Row == 4 && _classUnderTest.Player.Direction == Direction.North && _classUnderTest.Player.PreviousColumn == 1 && _classUnderTest.Player.PreviousRow == 5, "Unexpected player position and direction");

            // act/Assert
            // Move North
            _classUnderTest.MovePlayer();
            Assert.IsTrue(_classUnderTest.Player.Column == 1 && _classUnderTest.Player.Row == 3 && _classUnderTest.Player.Direction == Direction.North && _classUnderTest.Player.PreviousColumn == 1 && _classUnderTest.Player.PreviousRow == 4, "Unexpected player position and direction");

            // act/Assert
            // Hit wall - 3 directions available, East, West or reciprocal South
            _classUnderTest.MovePlayer();
            Assert.IsTrue(_classUnderTest.Player.Column == 1 && _classUnderTest.Player.Row == 3 && _classUnderTest.Player.Direction == Direction.North && _classUnderTest.Player.PreviousColumn == 1 && _classUnderTest.Player.PreviousRow == 4, "Unexpected player position and direction");

            _classUnderTest.Player.Direction = Direction.East;
            _classUnderTest.MovePlayer();

            Assert.IsTrue(_classUnderTest.Player.Column == 2 && _classUnderTest.Player.Row == 3 && _classUnderTest.Player.Direction == Direction.East && _classUnderTest.Player.PreviousColumn == 1 && _classUnderTest.Player.PreviousRow == 3, "Unexpected player position and direction");

            // hits dead end
            // turns around
            _classUnderTest.MovePlayer();

            Assert.IsTrue(_classUnderTest.Player.Column == 1 && _classUnderTest.Player.Row == 3 && _classUnderTest.Player.Direction == Direction.West && _classUnderTest.Player.PreviousColumn == 2 && _classUnderTest.Player.PreviousRow == 3, "Unexpected player position and direction");

            // Move west
            _classUnderTest.MovePlayer();

            Assert.IsTrue(_classUnderTest.Player.Column == 0 && _classUnderTest.Player.Row == 3 && _classUnderTest.Player.Direction == Direction.West && _classUnderTest.Player.PreviousColumn == 1 && _classUnderTest.Player.PreviousRow == 3, "Unexpected player position and direction");

            // Move west / wrap around to right hand side
            _classUnderTest.MovePlayer();

            Assert.IsTrue(_classUnderTest.Player.Column == 7 && _classUnderTest.Player.Row == 3 && _classUnderTest.Player.Direction == Direction.West && _classUnderTest.Player.PreviousColumn == 0 && _classUnderTest.Player.PreviousRow == 3, "Unexpected player position and direction");
        }

        [TestMethod]
        public void Maze_ChangePosition_WrapsAround()
        {
            // point north at top edge of the map
            _classUnderTest.Player.Direction = Direction.North;
            _classUnderTest.Player.Column = 4;
            _classUnderTest.Player.Row = 0;
            // move and wrap round to the bottom of the maze
            _classUnderTest.ChangePosition(_classUnderTest.Player);
            Assert.IsTrue(_classUnderTest.Player.Column == 4 && _classUnderTest.Player.Row == 6 && _classUnderTest.Player.Direction == Direction.North, "Unexpected wrapped position");

            // point east at right edge of the map
            _classUnderTest.Player.Direction = Direction.East;
            _classUnderTest.Player.Column = 7;
            _classUnderTest.Player.Row = 3;
            // move and wrap round to the bottom of the maze
            _classUnderTest.ChangePosition(_classUnderTest.Player);
            Assert.IsTrue(_classUnderTest.Player.Column == 0 && _classUnderTest.Player.Row == 3 && _classUnderTest.Player.Direction == Direction.East, "Unexpected wrapped position");

            // point south at bottom edge of the map
            _classUnderTest.Player.Direction = Direction.South;
            _classUnderTest.Player.Column = 4;
            _classUnderTest.Player.Row = 6;
            // move and wrap round to the bottom of the maze
            _classUnderTest.ChangePosition(_classUnderTest.Player);
            Assert.IsTrue(_classUnderTest.Player.Column == 4 && _classUnderTest.Player.Row == 0 && _classUnderTest.Player.Direction == Direction.South, "Unexpected wrapped position");

            // point west at left edge of the map
            _classUnderTest.Player.Direction = Direction.West;
            _classUnderTest.Player.Column = 0;
            _classUnderTest.Player.Row = 4;
            // move and wrap round to the bottom of the maze
            _classUnderTest.ChangePosition(_classUnderTest.Player);
            Assert.IsTrue(_classUnderTest.Player.Column == 7 && _classUnderTest.Player.Row == 4 && _classUnderTest.Player.Direction == Direction.West, "Unexpected wrapped position");
        }

        [TestMethod]
        public void Maze_BlockAtPosition_ReturnsBlockTypeAtPosition()
        {
            // "####.###"
            // "#G.#.#G#"
            // "####.#.#"
            // "...#G..O"
            // "#.##.#.#"
            // "#P.#   #"
            // "#### ###"

            Assert.AreEqual(Maze.BlockType.Dot, _classUnderTest.BlockAtPosition(1, 2), "Unexpected block type at 1,2");
            Assert.AreEqual(Maze.BlockType.Empty, _classUnderTest.BlockAtPosition(5, 4), "Unexpected block type at 5,4");
            Assert.AreEqual(Maze.BlockType.PowerPill, _classUnderTest.BlockAtPosition(3, 7), "Unexpected block type at 3,7");
            Assert.AreEqual(Maze.BlockType.Wall, _classUnderTest.BlockAtPosition(0, 0), "Unexpected block type at 0,0");
        }

        [TestMethod]
        public void Maze_RaiseScoreUpdatedEvent_RaiseScoreUpdatedEvent_E()
        {
            // Arrange
            _classUnderTest.LivesRemaining = 99;
            _classUnderTest.Score = 999;

            _classUnderTest.ScoreUpdated += (sender, args) => { Assert.IsTrue(args.LivesRemaining == 99 && args.Score == 999, "Unexpected events args"); };

            // Act / Assert
            _classUnderTest.RaiseScoreUpdatedEvent();
        }
    }
}