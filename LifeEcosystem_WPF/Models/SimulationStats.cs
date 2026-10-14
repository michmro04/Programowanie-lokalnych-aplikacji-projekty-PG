using LifeEcosystem_WPF.ViewModels;

namespace LifeEcosystem_WPF.Models
{
    public class SimulationStats : ViewModelBase
    {
        public int TotalPlants { get; set; }
        public int PlantBirths { get; set; }
        public int PlantsDeaths { get; set; }
        public int TotalAnimals { get; set; }
        public int AnimalBirths{ get; set; }
        public int AnimalDeaths{ get; set; }



        public void Refresh()
        {
            OnPropertyChanged(string.Empty);
        }
    }
}
