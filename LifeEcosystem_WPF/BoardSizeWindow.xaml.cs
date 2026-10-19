using System.Windows;

namespace LifeEcosystem_WPF
{
   
    public partial class BoardSizeWindow : Window
    {
        public int BoardWidth { get; private set; } = 50;
        public int BoardHeight { get; private set; } = 50;

        private const int MinSize = 20;
        private const int MaxSize = 100;

        public BoardSizeWindow()
        {
            InitializeComponent();
        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(WidthTextBox.Text.Trim(), out int width) || !int.TryParse(HeightTextBox.Text.Trim(), out int height))  
            {
                MessageBox.Show("Podaj poprawne liczby całkowite", "Błąd danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(width<MinSize || width>MaxSize || height<MinSize || height > MaxSize)
            {
                MessageBox.Show($"Wymiary planysz musza się mieścić w przedziale [{MinSize},{MaxSize}]", "Przekroczenie przedziału", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BoardWidth = width;
            BoardHeight = height;
            DialogResult = true;
            Close();
        }


    }
}
