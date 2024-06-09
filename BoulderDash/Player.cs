using static BoulderDash.CellTypes;

namespace BoulderDash
{
    public class Player
    {
        public static int PositionX { get; set; }
        public static int PositionY { get; set; }

        public Player(int startX, int startY)
        {
            PositionX = startX;
            PositionY = startY;
        }

        public static void Move(Game.MovementDirection direction)
        {
            int newX = PositionX;
            int newY = PositionY;

            switch (direction)
            {
                case Game.MovementDirection.Left:
                    newX--;
                    break;
                case Game.MovementDirection.Right:
                    newX++;
                    break;
                case Game.MovementDirection.Up:
                    newY--;
                    break;
                case Game.MovementDirection.Down:
                    newY++;
                    break;
            }

            if (IsValidMove(newX, newY))
            {
                MainWindow.map[PositionX, PositionY] = CellType.FreeSpace;
                PositionX = newX;
                PositionY = newY;
                MainWindow.map[PositionX, PositionY] = CellType.Player;
            }
        }

        private static bool IsValidMove(int x, int y)
        {
            return x >= 0 && x < MainWindow.Columns &&
                   y >= 0 && y < MainWindow.Rows &&
                   CanMoveTo(x, y);
        }

        private static bool CanMoveTo(int x, int y)
        {
            CellType cellType = MainWindow.map[x, y];
            return cellType == CellType.FreeSpace ||
                   cellType == CellType.Earth ||
                   cellType == CellType.Gem ||
                   cellType == CellType.Exit;
        }
    }
}
