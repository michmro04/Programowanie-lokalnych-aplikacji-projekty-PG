namespace LifeEcosystem_WPF.Models
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasPlant { get; set; } = false;
        public Animal? Animal { get; set; } = null;
        public bool IsEmpty { get; set; } = false;

        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

    }
}
