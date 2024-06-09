using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using static BoulderDash.CellTypes;

namespace BoulderDash
{
    public partial class MainWindow : Window
    {
        public const int Rows = 22;
        public const int Columns = 40;
        public static CellType[,] map = new CellType[Columns, Rows];
        private bool isGameStarted = false;
        public static bool isDead = false;
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            CreateGameGrid();
            LoadLevel();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isGameStarted && (e.Key == Key.Enter || e.Key == Key.Space))
            {
                await StartGame();
            }
            else if (isGameStarted && !isDead)
            {
                await game.Run(e.Key);
                UpdateMap();
                CheckWinCondition();
            }
            if (isDead)
            {
                UpdateText("You lose!");
            }
            e.Handled = true;
        }

        private async Task StartGame()
        {
            isGameStarted = true;
            Overlay.Visibility = Visibility.Collapsed;
            game = new Game(this);
            ReplaceSpawnWithPlayer();
            await game.Run(Key.None);
            UpdateMap();
            _ = Task.Run(async () => await game.StartContinuousFallChecks());
        }

        private void CreateGameGrid()
        {
            for (int i = 0; i < Rows; i++)
                GameGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < Columns; i++)
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        private void LoadLevel()
        {
            string mapPath = "C:\\Users\\Bodnarchuk Bohdan\\source\\repos\\BoulderDash\\BoulderDash\\Resources\\Maps\\LevelMap.txt";
            try
            {
                string[] lines = File.ReadAllLines(mapPath);
                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Columns; col++)
                    {
                        char cellType = lines[row][col];
                        map[col, row] = MapCharacterToCellType(cellType);
                        LoadMapElement(map[col, row], row, col);
                        if (cellType == 'S')
                        {
                            Player.PositionX = col;
                            Player.PositionY = row;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading level: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
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

        public void LoadMapElement(CellType cellType, int row, int col)
        {
            string imageName = $"{cellType}.png";
            Image image = CreateImageElement(imageName, row, col);
            GameGrid.Children.Add(image);
        }

        public void UpdateMap()
        {
            GameGrid.Children.Clear();
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    CellType cellType = map[col, row];
                    string imageName = $"{cellType}.png";
                    if (cellType == CellType.Player)
                        imageName = "Player.png";
                    Image image = CreateImageElement(imageName, row, col);
                    GameGrid.Children.Add(image);
                }
            }
        }

        private void ReplaceSpawnWithPlayer()
        {
            map[Player.PositionX, Player.PositionY] = CellType.Player;
        }

        public static Image CreateImageElement(string imageName, int row, int col)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "../../../Resources", imageName);
            BitmapImage bitmap = new BitmapImage(new Uri(path));
            Image image = new Image { Source = bitmap };
            Grid.SetRow(image, row);
            Grid.SetColumn(image, col);
            return image;
        }

        public void UpdateText(string text)
        {
            Dispatcher.Invoke(() =>
            {
                startGameOverlay.Text = text;
                Overlay.Visibility = Visibility.Visible;
            });
        }

        private void CheckWinCondition()
        {
            if (Gems.CollectedGems == Gems.TotalGems && map[Player.PositionX, Player.PositionY] == CellType.Exit)
                UpdateText("You Win!");
        }
    }
}
