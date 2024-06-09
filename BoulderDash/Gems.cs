using System.Threading.Tasks;
using static BoulderDash.CellTypes;

namespace BoulderDash
{
    public class Gems
    {
        public static int TotalGems = 0;
        public static int CollectedGems = 0;
        private MainWindow mainWindow;

        public Gems(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public async Task<bool> GemFallCheck(int x, int y)
        {
            if (MainWindow.map[x, y] == CellType.Gem)
            {
                bool playerBelow = false;
                int fallDistance = 0;

                int originalY = y;

                while (y < MainWindow.Rows - 1 && (MainWindow.map[x, y + 1] == CellType.FreeSpace || MainWindow.map[x, y + 1] == CellType.Player))
                {
                    if (MainWindow.map[x, y + 1] == CellType.Player)
                    {
                        playerBelow = true;
                        break;
                    }

                    MainWindow.map[x, y] = CellType.FreeSpace;
                    y++;
                    MainWindow.map[x, y] = CellType.Gem;
                    fallDistance++;
                }

                if (fallDistance > 0)
                {
                    MainWindow.map[x, originalY] = CellType.FreeSpace;
                }

                if (playerBelow && fallDistance > 0)
                {
                    PlayerCollision(x, y);
                }
                else if (!playerBelow && y < MainWindow.Rows - 1 && (MainWindow.map[x, y + 1] == CellType.Boulder || MainWindow.map[x, y + 1] == CellType.Gem))
                {
                    await SideFallCheck(x, y);
                }

                return fallDistance > 0; // Return true if the gem moved
            }
            return false;
        }

        private async Task SideFallCheck(int x, int y)
        {
            if (x > 0 && MainWindow.map[x - 1, y] == CellType.FreeSpace && MainWindow.map[x - 1, y + 1] == CellType.FreeSpace)
            {
                MainWindow.map[x, y] = CellType.FreeSpace;
                MainWindow.map[x - 1, y] = CellType.Gem;
                await GemFallCheck(x - 1, y);
            }
            else if (x < MainWindow.Columns - 1 && MainWindow.map[x + 1, y] == CellType.FreeSpace && MainWindow.map[x + 1, y + 1] == CellType.FreeSpace)
            {
                MainWindow.map[x, y] = CellType.FreeSpace;
                MainWindow.map[x + 1, y] = CellType.Gem;
                await GemFallCheck(x + 1, y);
            }
        }

        public void PlayerCollision(int x, int y)
        {
            if (MainWindow.map[x, y] == CellType.Gem)
            {
                MainWindow.map[x, y] = CellType.FreeSpace;
                CollectedGems++;
                mainWindow.UpdateText($"Gems Collected: {CollectedGems}/{TotalGems}");
            }
        }
    }
}
