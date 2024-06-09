using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace BoulderDash
{
    class LoadMap
    {
        public static void Load()
        {
            string mapFullPath = "C:\\\\Users\\\\Bodnarchuk Bohdan\\\\source\\\\repos\\\\BoulderDash\\\\BoulderDash\\\\Resources\\\\Maps\\\\LevelMap.txt";
            LoadLevel(mapFullPath);

        }

        private void LoadLevel(string mapPath)
        {
            string[] lines = File.ReadAllLines(mapPath);

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    char cellType = lines[row][col];
                    UIElement element = CreateElement(cellType);
                    if (element != null)
                    {
                        Grid.SetRow(element, row);
                        Grid.SetColumn(element, col);
                        GameGrid.Children.Add(element);
                    }
                }
            }
        }

        private UIElement CreateElement(char cellType)
        {
            switch (cellType)
            {
                case 'E':
                    return CreateImageElement("Earth.png");
                case 'B':
                    return CreateImageElement("Boulder.png");
                case 'G':
                    return CreateImageElement("Gem.png");
                case 'W':
                    return CreateImageElement("Wall.png");
                case 'X':
                    return CreateImageElement("Exit.png");
                case 'S':
                    return CreateImageElement("Spawn.png");
                default:
                    return null; // Free space
            }
        }

        private Image CreateImageElement(string imageName)
        {
            /**  BitmapImage bitmapImage = new BitmapImage();
              bitmapImage.BeginInit();
              bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
              bitmapImage.EndInit(); **/

            Image image = new Image();

            var path = Path.Combine(Environment.CurrentDirectory, "../../../Resources", imageName);
            var uri = new Uri(path);
            var bitmap = new BitmapImage(uri);

            // Assuming you have an Image control named "image1" in your XAML
            image.Source = bitmap;

            return image;
        }
    }
}
