namespace MazeGame
{
    public class Entity
    {
        public Entity(int startRow, int startColumn, int startSpeed, Direction startDirection)
        {
            Row = startRow;
            Column = startColumn;
            Direction = startDirection;
            Speed = startSpeed;
            PreviousColumn = startColumn;
            PreviousRow = startRow;
        }

        public int Speed { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public Direction Direction { get; set; }
        public int PreviousColumn { get; set; }
        public int PreviousRow { get; set; }
    }
}