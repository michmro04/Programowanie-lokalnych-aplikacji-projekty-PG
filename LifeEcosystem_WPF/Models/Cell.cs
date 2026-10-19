using LifeEcosystem_WPF.ViewModels;
using System.Windows.Input;

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
                if(hasPlant == value) return;   //odświeżanie tylko przy zmianie 
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
                if (animal == value) return;    //odświeżanie tylko przy zmianie 
                animal = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEmpty));
                OnPropertyChanged(nameof(HasAnimal));
            }

        }
        public bool HasAnimal => Animal != null;
        public bool IsEmpty => !HasPlant && Animal is null;

        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }


        public void TogglePlant()
        {
            HasPlant = !HasPlant;   //dodanie/usunięcie zwierzęcia
        }

        public void ToggleAnimal()
        {
            if(Animal == null)
            {
                    Animal = new Animal(0); //dodanie zwierzęcia
            }   
            else
            {
                Animal = null; //usunięcie zwierzęcia
            }
        }




    }
}
