using System.IO;
using static BoulderDash.CellTypes;
using System.Windows;

namespace BoulderDash
{
    public class Map
    {
        private readonly string mapPath;

        public Map(string path)
        {
            mapPath = path;
        }

        public void LoadMap(CellType[,] map)
        {
            try
            {
                string[] lines = File.ReadAllLines(mapPath);
                for (int row = 0; row < MainWindow.Rows; row++)
                {
                    for (int col = 0; col < MainWindow.Columns; col++)
                    {
                        char cellType = lines[row][col];
                        map[col, row] = MapCharacterToCellType(cellType);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading level: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1); // Terminate the application on error
            }
        }

        private CellType MapCharacterToCellType(char cellType)
        {
            return cellType switch
            {
                'E' => CellType.Earth,
                'B' => CellType.Boulder,
                'G' => CellType.Gem,
                'W' => CellType.Wall,
                'X' => CellType.Exit,
                'S' => CellType.Spawn,
                _ => CellType.FreeSpace,
            };
        }
    }
}
