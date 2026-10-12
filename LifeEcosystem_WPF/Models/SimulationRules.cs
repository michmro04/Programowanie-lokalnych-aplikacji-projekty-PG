namespace LifeEcosystem_WPF.Models
{
    public class SimulationRules
    {
        public HashSet<int> R { get; set; } = new() { 1, 2, 3 };    //roślina powstaje przy sąsiedztwie 1-3 roślin
        public HashSet<int> W { get; set; } = new() { 1, 2, 3, 4, 5, 6 };   //roślina wegetuje gdy sąsiedztwo roślin wynosi 1-6
        public HashSet<int> B { get; set; } = new() { 2, 3 };   //zwierze powstaje przy sąsiedztwie 2-3 zwierząt
        public HashSet<int> P { get; set; } = new() { 0, 1 };   //liczba roślin na danym polu przy narodzinach zwierzęcia
        public HashSet<int> Z { get; set; } = new() { 0, 1, 2, 3 };     //zwierze przeżywa gdy sąsiedztwo zwierząt wynosi 0-3

        public int M { get; set; } = 1;     //ruch zwierzęcia w kierunku rośliny lub losowo
        public int L { get; set; } = 10;    //długość życia zwierzęcia

    }
}
