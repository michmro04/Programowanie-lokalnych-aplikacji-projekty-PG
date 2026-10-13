using System.IO;

namespace LifeEcosystem_WPF.Models
{
    public class Board
    {
        public int Width { get; }
        public int Height { get; }

        public Cell[,] Grid { get; }
        public SimulationRules Rules { get; set; }

        public SimulationStats Stats { get; private set; } = new SimulationStats();
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

        public bool IsInBoard(int x, int y)
        {
            return (x >= 0 && x < Width && y >= 0 && y < Height);
        }


        public (int plants, int animals) CountNeighbours(int x, int y)
        {
            int plants = 0;
            int animals = 0;


            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (IsInBoard(x+i, y+j))
                    {
                        if (i == 0 && j == 0)   //pominięcie pola na którym jest organizm
                            continue;
                        if (Grid[x + i, y + j].HasPlant == true)
                            plants++;

                        if (Grid[x + i, y + j].Animal != null)
                            animals++;
                    }
                }
            }
            return (plants,animals);
        }


        public (int targetX, int targetY) FindTargetToMove(int currX, int currY, Random rand)
        { 
            //zwierze na polu z rośliną
            if (Grid[currX, currY].HasPlant)    
            {
                return (currX, currY);     //zwierzę zostaje na tym polu
            }

            int? targetPlantX = null;
            int? targetPlantY = null;

            //przeszukiwanie sąsiednich pól o prominiu M w poszukiwaniu roślin
            for (int i=-Rules.M; i<=Rules.M; i++)
            {
                for(int j=-Rules.M; j<=Rules.M; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    if (IsInBoard(currX + i, currY + j) && Grid[currX + i, currY + j].HasPlant)
                    {
                        targetPlantX = currX + i;
                        targetPlantY= currY + j;
                        break;
                    }
                }
                if (targetPlantX != null) break;
            }

            if(targetPlantX.HasValue && targetPlantY.HasValue)
            {
                int offsetX = Math.Sign(targetPlantX.Value - currX);
                int offsetY = Math.Sign(targetPlantY.Value - currY);

                int targetX = currX + offsetX;
                int targetY = currY + offsetY;
                

                if(IsInBoard(targetX, targetY) && Grid[targetX, targetY].Animal == null)
                {
                    return (targetX, targetY);
                }
            }


            //ruch losowy w przypadku braku roslin w okolicy

            //dodanie wolnych miejsc do listy
            List<(int, int)> freeNeighbourFields = new List<(int, int)>();

            for(int i = -1; i<=1; i++)
            {
                for(int j=-1; j<=1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int nx = currX + i;
                    int ny = currY + j;
                    if(IsInBoard(nx,ny) && Grid[nx,ny].Animal == null)
                    {
                        freeNeighbourFields.Add((nx, ny));
                    }
                }
            }

            //losowanie pola z listy
            if(freeNeighbourFields.Count > 0)
            {
                return freeNeighbourFields[rand.Next(freeNeighbourFields.Count)];
            }

            //zwrócenie obecnej pozycji w przypadku nieznalezienia rośliny w okolicy && braku wolnych miejsc w okolicy
            return (currX, currY);
        }

        //funckcja pomocnicza do zliczania sąsaidów dla przyjętej tablicy zwierząt
        private int CountAnimalNeighboursInArray(Animal?[,] array, int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int nx = x + i;
                    int ny = y + j;

                    if (IsInBoard(nx, ny) && array[nx, ny] != null)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void Step()
        {
            Random rand = new Random();
            //reset statystyk total, bo liczymy je od nowa
            Stats.TotalAnimals = 0;
            Stats.AnimalBirths = 0;
            Stats.AnimalDeaths = 0;
            Stats.TotalPlants = 0;
            Stats.PlantBirths = 0;
            Stats.PlantsDeaths = 0;

            //1 - ruch zwierząt i zjadanie roślin
            Animal?[,] nextAnimals = new Animal?[Width,Height];
           
            //sprawdzenie zwierząt na tablicy i dodanie ich do Animals
            for(int i=0; i<Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (Grid[i, j].Animal != null)
                    {
                        Animal? currAnimal = Grid[i, j].Animal;

                        var (targetX, targetY) = FindTargetToMove(i, j, rand);

                        //sprawdzenie czy pole w nextAnimals jest wolne
                        if (nextAnimals[targetX, targetY] == null)
                        {
                            nextAnimals[targetX, targetY] = currAnimal;
                        }
                        //kolizja - zwierze które chce się przemieścić zostaje na swojej pozycji
                        else
                        {
                            nextAnimals[i, j] = currAnimal;
                            targetX = i;
                            targetY = j;
                        }
                
                        //zjadanie rośliny
                        if (Grid[targetX, targetY].HasPlant)
                        {
                            Grid[targetX, targetY].HasPlant = false;
                            Stats.PlantsDeaths++;
                        }
                
                        
                    }
                }//for Y
            }//for X


            //2 - ewolucja roślin
            bool[,] nextPlants = new bool[Width, Height];
            
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var (countPlants, _) = CountNeighbours(i,j);

                    if (Grid[i, j].HasPlant)
                    {
                        if (Rules.W.Contains(countPlants))
                            nextPlants[i, j] = true;
                        else
                        {
                            nextPlants[i, j] = false;    //jeśli roślina nie ma W sąsiadów roślin to usycha
                            Stats.PlantsDeaths++;
                        }
                    }
                    //powstanie nowej rośliny
                    else
                    {
                        if (Rules.R.Contains(countPlants) && nextAnimals[i, j] == null)
                        {
                            nextPlants[i, j] = true;
                            Stats.PlantBirths++;
                        }
                    }
                }//for Y
            }//for X


            //3 - ewolucja zwierząt (śmierć i narodziny)
            Animal?[,] finalAnimals = new Animal?[Width, Height];

            //śmierć
            for(int i=0; i<Width; i++)
            {
                for(int j=0; j<Height; j++)
                {
                    if (nextAnimals[i, j] != null)
                    {
                        nextAnimals[i, j].AgeOneYear();

                        //śmierć zwierzęcia po prekroczeniu wieku L
                        if (nextAnimals[i,j].Age >= Rules.L)
                        {
                            nextAnimals[i, j] = null;
                            Stats.AnimalDeaths++;
                        }
                        else
                        {
                            int animalNeighbours = CountAnimalNeighboursInArray(nextAnimals,i, j);
                            if (Rules.Z.Contains(animalNeighbours))
                            {
                                finalAnimals[i, j] = nextAnimals[i, j];
                            }
                            else
                            {
                                finalAnimals[i, j] = null;
                                Stats.AnimalDeaths++;
                            }
                        }
                    }
                }
            }

            //narodziny
            for(int i = 0; i<Width; i++)
            {
                for(int j = 0; j<Height; j++)
                {

                    if (nextAnimals[i, j] == null)
                    {
                        int animalNeighbours = CountAnimalNeighboursInArray(nextAnimals, i, j);
                        int plantOnFiled = nextPlants[i, j] ? 1 : 0;

                        //warunki co do ilości zwierząt w sąsiedztwie oraz obecności rośliny na polu
                        if(Rules.B.Contains(animalNeighbours) && Rules.P.Contains(plantOnFiled))
                        {
                            finalAnimals[i, j] = new Animal(0);
                            Stats.AnimalBirths++;
                        }
                    }
                }
            }


            //4 - aktualizacja tablic i statystyk
            for(int i=0; i<Width; i++)
            {
                for(int j=0; j<Height; j++) {
                    Grid[i, j].HasPlant = nextPlants[i, j];
                    Grid[i, j].Animal = finalAnimals[i, j];

                    if (Grid[i, j].HasPlant) Stats.TotalPlants++;
                    if (Grid[i, j].Animal != null) Stats.TotalAnimals++;
               }
            }
        }


        //zapis do pliku / odczyt z pliku
        
        //zapis
        public void SaveToFile(string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath);

            writer.WriteLine($"Width:{Width}");
            writer.WriteLine($"Height:{Height}");
            writer.WriteLine($"R:{string.Join(",", Rules.R)}");
            writer.WriteLine($"W:{string.Join(",", Rules.W)}");
            writer.WriteLine($"B:{string.Join(",", Rules.B)}");
            writer.WriteLine($"P:{string.Join(",", Rules.P)}");
            writer.WriteLine($"Z:{string.Join(",", Rules.Z)}");
            writer.WriteLine($"M:{Rules.M}");
            writer.WriteLine($"L:{Rules.L}");
            writer.WriteLine("--- --- ---");

            for(int y=0; y<Height; y++)
            {
                char[] row = new char[Width];
                for(int x=0; x<Width; x++)
                {
                    Cell cell = Grid[x, y];
                    if (cell.HasPlant && cell.Animal != null) row[x] = 'B'; //both
                    else if (cell.HasPlant) row[x] = 'P';                   //plant
                    else if (cell.Animal != null) row[x] = 'A';             //animal
                    else row[x] = '-';                                      //puste
                }
                writer.WriteLine(new string(row));
            }
        }

        //odczyt
        public static Board LoadFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            SimulationRules rules = new SimulationRules();
            int width = 50;
            int height = 50;
            int lineIndex = 0;

            while (lineIndex < lines.Length && !lines[lineIndex].StartsWith("---"))
            {
                string line = lines[lineIndex].Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string val = parts[1].Trim();

                        switch (key)
                        {
                            case "Width":
                                width = int.Parse(val);
                                break;
                            case "Height":
                                height = int.Parse(val);
                                break;
                            case "R":
                                rules.R = ParseSet(val);
                                break;
                            case "W":
                                rules.W = ParseSet(val);
                                break;
                            case "B":
                                rules.B = ParseSet(val);
                                break;
                            case "P":
                                rules.P = ParseSet(val);
                                break;
                            case "Z":
                                rules.Z = ParseSet(val);
                                break;
                            case "M":
                                rules.M = int.Parse(val);
                                break;
                            case "L":
                                rules.L = int.Parse(val);
                                break;
                        }
                    }
                }
                lineIndex++;
            }

            lineIndex++;

            Board board = new Board(width, height, rules);

            int y = 0;
            while (lineIndex < lines.Length && y < height)
            {
                string row = lines[lineIndex];
                for (int x = 0; x < Math.Min(row.Length, width); x++)
                {
                    char symbol = row[x];
                    switch (symbol)
                    {
                        case 'B': //both
                            board.Grid[x, y].HasPlant = true;
                            board.Grid[x, y].Animal = new Animal(0);
                            break;
                        case 'P': //roślina
                            board.Grid[x, y].HasPlant = true;
                            board.Grid[x, y].Animal = null;
                            break;
                        case 'A': //zwierzę
                            board.Grid[x, y].HasPlant = false;
                            board.Grid[x, y].Animal = new Animal(0);
                            break;
                        default:  //puste
                            board.Grid[x, y].HasPlant = false;
                            board.Grid[x, y].Animal = null;
                            break;
                    }
                }
                y++;
                lineIndex++;
            }

            return board;
        }

        //funkcja pomocnicza zamieniająca 1,2,3 na HashSet<int>
        private static HashSet<int> ParseSet(string value)
        {
            HashSet<int> set = new HashSet<int>();
            if (string.IsNullOrWhiteSpace(value)) return set;

            string[] elements = value.Split(',');
            foreach (string el in elements)
            {
                if (int.TryParse(el.Trim(), out int num))
                {
                    set.Add(num);
                }
            }
            return set;
        }






    }
}
