using System.Collections.Generic;
using System.Threading.Tasks;
using static BoulderDash.CellTypes;

namespace BoulderDash
{
    public class Game
    {
        private MainWindow mainWindow;
        private Gems gems;
        private Boulders boulders;

        public enum MovementDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        // Lists to track boulders and gems
        private List<(int x, int y)> boulderPositions;
        private List<(int x, int y)> gemPositions;

        public Game(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.gems = new Gems(mainWindow);
            this.boulders = new Boulders(mainWindow);
            boulderPositions = new List<(int x, int y)>();
            gemPositions = new List<(int x, int y)>();
            InitializeBouldersAndGems();
        }

        private void InitializeBouldersAndGems()
        {
            for (int col = 0; col < MainWindow.Columns; col++)
            {
                for (int row = 0; row < MainWindow.Rows; row++)
                {
                    if (MainWindow.map[col, row] == CellType.Boulder)
                        boulderPositions.Add((col, row));
                    else if (MainWindow.map[col, row] == CellType.Gem)
                        gemPositions.Add((col, row));
                }
            }
        }

        public async Task Run(System.Windows.Input.Key key)
        {
            MovementDirection direction = MovementDirection.Up;

            switch (key)
            {
                case System.Windows.Input.Key.Up:
                    direction = MovementDirection.Up;
                    break;
                case System.Windows.Input.Key.Down:
                    direction = MovementDirection.Down;
                    break;
                case System.Windows.Input.Key.Left:
                    direction = MovementDirection.Left;
                    break;
                case System.Windows.Input.Key.Right:
                    direction = MovementDirection.Right;
                    break;
            }

            Player.Move(direction);
            gems.PlayerCollision(Player.PositionX, Player.PositionY);

            await Task.WhenAll(FallCheckForAllBoulders(), FallCheckForAllGems());
            mainWindow.UpdateMap(); // Update the UI once after processing all movements
        }

        public async Task StartContinuousFallChecks()
        {
            while (!MainWindow.isDead)
            {
                await Task.WhenAll(FallCheckForAllBoulders(), FallCheckForAllGems());
                mainWindow.UpdateMap(); // Update the UI once after processing all movements
                await Task.Delay(500); // Reduced delay for performance balance
            }
        }

        private async Task FallCheckForAllBoulders()
        {
            // Process each boulder in the list
            List<(int x, int y)> newPositions = new List<(int x, int y)>();
            for (int i = 0; i < boulderPositions.Count; i++)
            {
                var (x, y) = boulderPositions[i];
                if (await boulders.FallCheck(x, y))
                {
                    // Update the position in the list if it moved
                    newPositions.Add((x, y + 1));
                }
                else
                {
                    newPositions.Add((x, y));
                }
            }
            boulderPositions = newPositions;
        }

        private async Task FallCheckForAllGems()
        {
            // Process each gem in the list
            List<(int x, int y)> newPositions = new List<(int x, int y)>();
            for (int i = 0; i < gemPositions.Count; i++)
            {
                var (x, y) = gemPositions[i];
                if (await gems.GemFallCheck(x, y))
                {
                    // Update the position in the list if it moved
                    newPositions.Add((x, y + 1));
                }
                else
                {
                    newPositions.Add((x, y));
                }
            }
            gemPositions = newPositions;
        }
    }
}
