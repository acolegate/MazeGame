using System;
using System.Timers;

namespace MazeGame
{
    public class Engine
    {
        public enum GameState
        {
            Waiting = 0,
            Running = 1,
            GameOver = 2,
            Quit = 3
        }

        private const int GameLoopTimerIntervalMilliseconds = 200;
        private Timer _gameLoopTimer;

        public Engine(string[] mazeStrings, Random random)
        {
            MazeData = new Maze(mazeStrings, random);
        }

        public Maze MazeData { get; }

        public void StartGame()
        {
            _gameLoopTimer = new Timer(GameLoopTimerIntervalMilliseconds)
                                 {
                                     AutoReset = false
                                 };
            _gameLoopTimer.Elapsed += GameLoopTimer_Elapsed;
            _gameLoopTimer.Start();
        }

        private void GameLoopTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MazeData.MovePlayer();

            foreach (Entity ghost in MazeData.Ghosts)
            {
                MazeData.MoveGhost(ghost);
            }

            _gameLoopTimer.Start();
        }
    }
}