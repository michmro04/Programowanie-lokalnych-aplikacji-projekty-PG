using LifeEcosystem_WPF.ViewModels;

namespace LifeEcosystem_WPF.Models
{
    public class Cell : ViewModelBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        private bool hasPlant;
        public bool HasPlant
        {
            get => hasPlant;
            set
            {
                hasPlant = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEmpty));
            }
        }
        private Animal? animal;
        public Animal? Animal
        {
            get => animal;
            set
            {
                animal = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEmpty));
            }

        }
        public bool IsEmpty => !HasPlant && Animal is null;

        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

    }
}
