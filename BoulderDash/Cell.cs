using static BoulderDash.CellTypes;

namespace BoulderDash
{
    public class Cell
    {
        public CellType Type { get; private set; }

        public Cell(CellType type)
        {
            Type = type;
        }
    }
}
