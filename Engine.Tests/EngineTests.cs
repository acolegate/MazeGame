using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Engine.Tests.ExtensionMethods;

using MazeGame;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EngineTests
    {
        private readonly Maze.Block[,] _expectedSmallMazeBlockArray = {
                                                                          {
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Empty)
                                                                          },
                                                                          {
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Dot),
                                                                              new Maze.Block(Maze.BlockType.Dot),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Dot),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Wall)
                                                                          },
                                                                          {
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Dot),
                                                                              new Maze.Block(Maze.BlockType.Empty)
                                                                          },
                                                                          {
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Dot),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.PowerPill),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Wall) //
                                                                          },
                                                                          {
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Empty),
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Wall),
                                                                              new Maze.Block(Maze.BlockType.Empty)
                                                                          }
                                                                      };

        private readonly string[] _smallMaze1 = {
                                                    "G##G##G",
                                                    "G..G.G#",
                                                    "# GPG.G",
                                                    "#G.GOG#",
                                                    "G#G###G"
                                                };

        private MazeGame.Engine _classUnderTest;

        private Random _random;

        [TestInitialize]
        public void Initialise()
        {
            _random = new Random(0);
        }

        [TestMethod]
        public void Engine_MoveGhost_GhostIsInExpectedPositionAndDirection()
        {
            // arrange
            string[] smallMaze = {
                                     "#####",
                                     "#..G#",
                                     "#.###",
                                     "#.P.#",
                                     "###.#",
                                     "#.O.#",
                                     "#####"
                                 };

            // act
            _classUnderTest = new MazeGame.Engine(smallMaze, _random);

            // assert
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 1, 3, Direction.West, 1, 3);

            // act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 1, 2, Direction.West, 1, 3);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 1, 1, Direction.South, 1, 2);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 2, 1, Direction.South, 1, 1);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 3, 1, Direction.East, 2, 1);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 3, 2, Direction.East, 3, 1);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 3, 3, Direction.South, 3, 2);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 4, 3, Direction.South, 3, 3);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 5, 3, Direction.West, 4, 3);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 5, 2, Direction.West, 5, 3);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 5, 1, Direction.East, 5, 2);

            // hit the other end - start on its way back

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 5, 2, Direction.East, 5, 1);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 5, 3, Direction.North, 5, 2);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 4, 3, Direction.North, 5, 3);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 3, 3, Direction.West, 4, 3);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 3, 2, Direction.West, 3, 3);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 3, 1, Direction.North, 3, 2);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 2, 1, Direction.North, 3, 1);

            // arrange/act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 1, 1, Direction.East, 2, 1);

            // act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 1, 2, Direction.East, 1, 1);

            // act/Assert
            _classUnderTest.MazeData.MoveGhost(_classUnderTest.MazeData.Ghosts[0]);
            TestGhostPositionAndDirection(_classUnderTest.MazeData.Ghosts[0], 0, 1, 3, Direction.West, 1, 2);

            // Back to the start
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static void TestGhostPositionAndDirection(Entity ghost, int index, int expectedRow, int expectedColumn, Direction expectedDirection, int expectedPreviousRow, int expectedPreviousColumn)
        {
            Assert.IsTrue(ghost.Row == expectedRow && ghost.Column == expectedColumn, "Unexpected Ghost " + index + " position");
            Assert.AreEqual(expectedDirection, ghost.Direction, "Unexpected Ghost " + index + " direction");
            Assert.IsTrue(ghost.PreviousRow == expectedPreviousRow && ghost.PreviousColumn == expectedPreviousColumn, "Unexpected Ghost " + index + " previous position");
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void Engine_Setup_MazeDimensionsAreCorrect()
        {
            // arrange

            // act
            _classUnderTest = new MazeGame.Engine(_smallMaze1, _random);

            // Assert

            Assert.AreEqual(_expectedSmallMazeBlockArray.GetUpperBound(0), _classUnderTest.MazeData.MaxRowIndex, "Unexpected value returned");
            Assert.AreEqual(_expectedSmallMazeBlockArray.GetUpperBound(1), _classUnderTest.MazeData.MaxColumnIndex, "Unexpected value returned");
        }

        [TestMethod]
        public void Engine_Setup_MazeBlocksAreCorrect_PlayerAndGhostsAreInCorrectPositionsAndDirections()
        {
            // arrange

            // act
            _classUnderTest = new MazeGame.Engine(_smallMaze1, _random);

            // Assert

            // == CHECK EACH MAZE BLOCK ==
            for (int row = 0; row <= _classUnderTest.MazeData.MaxRowIndex; row++)
            {
                for (int column = 0; column <= _classUnderTest.MazeData.MaxColumnIndex; column++)
                {
                    Assert.AreEqual(_expectedSmallMazeBlockArray[row, column].BlockType, _classUnderTest.MazeData.Blocks[row, column].BlockType, "Unexpected block type at row " + row + ", column " + column);
                    Assert.AreEqual(0, _classUnderTest.MazeData.Blocks[row, column].TrailStrength, "Unexpected trailstrength at row " + row + ", column " + column);
                }
            }
        }

        [TestMethod]
        public void Engine_Setup_PlayerPositionAndDirectionAreCorrect()
        {
            // arrange

            // act
            _classUnderTest = new MazeGame.Engine(_smallMaze1, _random);

            // Assert

            // == CHECK THE PLAYER POSITION & DIRECTION ==
            Assert.IsTrue(_classUnderTest.MazeData.Player.Row == 2 && _classUnderTest.MazeData.Player.Column == 3, "Unexpected player position");
            Assert.IsTrue(_classUnderTest.MazeData.Player.Direction == Direction.West, "Unexpected player position");
            Assert.AreEqual(2, _classUnderTest.MazeData.Player.Speed, "Unexpected player speed");
        }

        [TestMethod]
        public void Engine_Setup_GhostNumberPositionsAndDirectionsAreCorrect()
        {
            // arrange

            // act
            _classUnderTest = new MazeGame.Engine(_smallMaze1, _random);

            // assert
            // == CHECK THE GHOST COUNT ==
            Assert.AreEqual(15, _classUnderTest.MazeData.Ghosts.Count, "Unexpected number of ghosts");

            // == CHECK THE GHOST POSITIONS ==
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[0].Row == 0 && _classUnderTest.MazeData.Ghosts[0].Column == 0, "Unexpected ghost 0 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[1].Row == 0 && _classUnderTest.MazeData.Ghosts[1].Column == 3, "Unexpected ghost 1 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[2].Row == 0 && _classUnderTest.MazeData.Ghosts[2].Column == 6, "Unexpected ghost 2 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[3].Row == 1 && _classUnderTest.MazeData.Ghosts[3].Column == 0, "Unexpected ghost 3 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[4].Row == 1 && _classUnderTest.MazeData.Ghosts[4].Column == 3, "Unexpected ghost 4 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[5].Row == 1 && _classUnderTest.MazeData.Ghosts[5].Column == 5, "Unexpected ghost 5 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[6].Row == 2 && _classUnderTest.MazeData.Ghosts[6].Column == 2, "Unexpected ghost 6 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[7].Row == 2 && _classUnderTest.MazeData.Ghosts[7].Column == 4, "Unexpected ghost 7 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[8].Row == 2 && _classUnderTest.MazeData.Ghosts[8].Column == 6, "Unexpected ghost 8 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[9].Row == 3 && _classUnderTest.MazeData.Ghosts[9].Column == 1, "Unexpected ghost 9 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[10].Row == 3 && _classUnderTest.MazeData.Ghosts[10].Column == 3, "Unexpected ghost 10 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[11].Row == 3 && _classUnderTest.MazeData.Ghosts[11].Column == 5, "Unexpected ghost 11 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[12].Row == 4 && _classUnderTest.MazeData.Ghosts[12].Column == 0, "Unexpected ghost 12 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[13].Row == 4 && _classUnderTest.MazeData.Ghosts[13].Column == 2, "Unexpected ghost 13 position");
            Assert.IsTrue(_classUnderTest.MazeData.Ghosts[14].Row == 4 && _classUnderTest.MazeData.Ghosts[14].Column == 6, "Unexpected ghost 14 position");

            // CHECK EACH GHOST DIRECTION ==
            Assert.AreEqual(Direction.North, _classUnderTest.MazeData.Ghosts[0].Direction, "Unexpected direction for ghost 0");
            Assert.AreEqual(Direction.South, _classUnderTest.MazeData.Ghosts[1].Direction, "Unexpected direction for ghost 1");
            Assert.AreEqual(Direction.East, _classUnderTest.MazeData.Ghosts[2].Direction, "Unexpected direction for ghost 2");
            Assert.AreEqual(Direction.East, _classUnderTest.MazeData.Ghosts[3].Direction, "Unexpected direction for ghost 3");
            Assert.AreEqual(Direction.South, _classUnderTest.MazeData.Ghosts[4].Direction, "Unexpected direction for ghost 4");
            Assert.AreEqual(Direction.South, _classUnderTest.MazeData.Ghosts[5].Direction, "Unexpected direction for ghost 5");
            Assert.AreEqual(Direction.West, _classUnderTest.MazeData.Ghosts[6].Direction, "Unexpected direction for ghost 6");
            Assert.AreEqual(Direction.West, _classUnderTest.MazeData.Ghosts[7].Direction, "Unexpected direction for ghost 7");
            Assert.AreEqual(Direction.West, _classUnderTest.MazeData.Ghosts[8].Direction, "Unexpected direction for ghost 8");
            Assert.AreEqual(Direction.East, _classUnderTest.MazeData.Ghosts[9].Direction, "Unexpected direction for ghost 9");
            Assert.AreEqual(Direction.North, _classUnderTest.MazeData.Ghosts[10].Direction, "Unexpected direction for ghost 10");
            Assert.AreEqual(Direction.West, _classUnderTest.MazeData.Ghosts[11].Direction, "Unexpected direction for ghost 11");
            Assert.AreEqual(Direction.West, _classUnderTest.MazeData.Ghosts[12].Direction, "Unexpected direction for ghost 12");
            Assert.AreEqual(Direction.North, _classUnderTest.MazeData.Ghosts[13].Direction, "Unexpected direction for ghost 13");
            Assert.AreEqual(Direction.South, _classUnderTest.MazeData.Ghosts[14].Direction, "Unexpected direction for ghost 14");

            // == CHECK ALL GHOST SPEEDS ==
            for (int i = 0; i <= 14; i++)
            {
                Assert.AreEqual(2, _classUnderTest.MazeData.Ghosts[i].Speed, "Unexpected speed for ghost " + i);
            }

            // == CHECK AVAILABLE DIRECTION FOR EACH GHOST ==
            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.South,
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[0].Row, _classUnderTest.MazeData.Ghosts[0].Column), "Ghost 0 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.South
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[1].Row, _classUnderTest.MazeData.Ghosts[1].Column), "Ghost 1 unexpected available directions");
            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.East,
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[2].Row, _classUnderTest.MazeData.Ghosts[2].Column), "Ghost 2 unexpected available directions");
            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.East,
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[3].Row, _classUnderTest.MazeData.Ghosts[3].Column), "Ghost 3 unexpected available directions");
            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.South,
                                                   Direction.East,
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[4].Row, _classUnderTest.MazeData.Ghosts[4].Column), "Ghost 4 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.South,
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[5].Row, _classUnderTest.MazeData.Ghosts[5].Column), "Ghost 5 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.South,
                                                   Direction.East,
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[6].Row, _classUnderTest.MazeData.Ghosts[6].Column), "Ghost 6 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.South,
                                                   Direction.East,
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[7].Row, _classUnderTest.MazeData.Ghosts[7].Column), "Ghost 7 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[8].Row, _classUnderTest.MazeData.Ghosts[8].Column), "Ghost 8 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.East
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[9].Row, _classUnderTest.MazeData.Ghosts[9].Column), "Ghost 9 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.East,
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[10].Row, _classUnderTest.MazeData.Ghosts[10].Column), "Ghost 10 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[11].Row, _classUnderTest.MazeData.Ghosts[11].Column), "Ghost 11 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.South,
                                                   Direction.West
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[12].Row, _classUnderTest.MazeData.Ghosts[12].Column), "Ghost 12 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.North,
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[13].Row, _classUnderTest.MazeData.Ghosts[13].Column), "Ghost 13 unexpected available directions");

            CollectionAssert.AreEquivalent(new List<Direction>
                                               {
                                                   Direction.South,
                                                   Direction.East,
                                               }, _classUnderTest.MazeData.AvailableDirections(_classUnderTest.MazeData.Ghosts[14].Row, _classUnderTest.MazeData.Ghosts[14].Column), "Ghost 14 unexpected available directions");
        }

        [TestMethod]
        public void Engine_Setup_NoPlayer_ThrowsException()
        {
            // arrange
            string[] testMaze = {
                                    "######",
                                    "#. G.#",
                                    "######"
                                };
            // Act
            AssertHelper.Throws<ApplicationException>(() => _classUnderTest = new MazeGame.Engine(testMaze, _random), "No player defined in maze");
        }

        [TestMethod]
        public void Engine_Setup_NoGhosts_ThrowsException()
        {
            // arrange
            string[] testMaze = {
                                    "#######",
                                    "#.P. .#",
                                    "#######"
                                };

            // Act
            AssertHelper.Throws<ApplicationException>(() => _classUnderTest = new MazeGame.Engine(testMaze, _random), "No ghosts defined in maze");
        }

        [TestMethod]
        public void Engine_Setup_NoDotsPlaced_ThrowsException()
        {
            // arrange
            string[] testMaze = {
                                    "######",
                                    "#G P #",
                                    "######"
                                };
            // Act
            AssertHelper.Throws<ApplicationException>(() => _classUnderTest = new MazeGame.Engine(testMaze, _random), "No dots placed in maze");
        }

        [TestMethod]
        public void Engine_Setup_CorrectNumberOfDotsPlaced()
        {
            // arrange
            string[] smallMaze = {
                                     "#####",
                                     "#..G#",
                                     "#.###",
                                     "#.P.#",
                                     "###.#",
                                     "#.O.#",
                                     "#####"
                                 };

            // act
            _classUnderTest = new MazeGame.Engine(smallMaze, _random);

            // assert
            Assert.AreEqual(8, _classUnderTest.MazeData.DotsRemaining, "Unexpected number of dots remaining");
        }
    }
}