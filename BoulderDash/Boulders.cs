using System.Threading.Tasks;
using static BoulderDash.CellTypes;

namespace BoulderDash
{
    public class Boulders
    {
        private MainWindow mainWindow;

        public Boulders(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public async Task<bool> FallCheck(int x, int y)
        {
            if (MainWindow.map[x, y] == CellType.Boulder)
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
                    MainWindow.map[x, y] = CellType.Boulder;
                    fallDistance++;
                }

                if (fallDistance > 0)
                {
                    MainWindow.map[x, originalY] = CellType.FreeSpace;
                }

                if (playerBelow && fallDistance > 0)
                {
                    DeathLogic(x, y);
                }
                else if (!playerBelow && y < MainWindow.Rows - 1 && (MainWindow.map[x, y + 1] == CellType.Boulder || MainWindow.map[x, y + 1] == CellType.Gem))
                {
                    await SideFallCheck(x, y);
                }

                return fallDistance > 0; // Return true if the boulder moved
            }
            return false;
        }

        private async Task SideFallCheck(int x, int y)
        {
            if (x > 0 && MainWindow.map[x - 1, y] == CellType.FreeSpace && MainWindow.map[x - 1, y + 1] == CellType.FreeSpace)
            {
                MainWindow.map[x, y] = CellType.FreeSpace;
                MainWindow.map[x - 1, y] = CellType.Boulder;
                await FallCheck(x - 1, y);
            }
            else if (x < MainWindow.Columns - 1 && MainWindow.map[x + 1, y] == CellType.FreeSpace && MainWindow.map[x + 1, y + 1] == CellType.FreeSpace)
            {
                MainWindow.map[x, y] = CellType.FreeSpace;
                MainWindow.map[x + 1, y] = CellType.Boulder;
                await FallCheck(x + 1, y);
            }
        }

        public void DeathLogic(int x, int y)
        {
            MainWindow.isDead = true;
            MainWindow.map[x, y] = CellType.FreeSpace;
            mainWindow.UpdateText("You lose!");
        }
    }
}
