namespace LifeEcosystem_WPF.Models
{
    public class Board
    {
        public int Width { get; }
        public int Height { get; }

        public Cell[,] Grid { get; }
        public SimulationRules Rules { get; set; }

        public Board(int width, int height, SimulationRules? rules = null)
        {
            this.Width = width;
            this.Height = height;
            this.Rules = rules ?? new SimulationRules();

            Grid = new Cell[width, height];
            for (int x=0; x<width; x++)
            {
                for(int y=0; y<height; y++)
                {
                    Grid[x, y] = new Cell(x, y);
                }
            }
        }


        public void Clear()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Grid[x,y].HasPlant=false;
                    Grid[x,y].Animal=null;
                }
            }
        }

        public void Randomize(double plantProbability, double animalProbability)
        {
            Random rand = new Random();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    double plantRandomValue = rand.NextDouble();
                    if (plantRandomValue < plantProbability)
                    {
                        Grid[x, y].HasPlant = true;
                    }
                    else
                    {
                        Grid[x, y].HasPlant = false;
                    }

                    double animalRandomValue = rand.NextDouble();
                    if(animalRandomValue < animalProbability)
                    {
                        Grid[x, y].Animal = new Animal(0);
                    }
                    else
                    {
                        Grid[x, y].Animal = null;
                    }

                }
            }
        }




    }
}
