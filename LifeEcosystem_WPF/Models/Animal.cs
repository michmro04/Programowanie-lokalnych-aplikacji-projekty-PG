namespace LifeEcosystem_WPF.Models
{
    public class Animal
    {
        public int Age { get; set; } = 0;

        public Animal(int age)
        {
            this.Age = age;
        }

        public void AgeOneYear()
        {
            Age++;
        }

    }
}
