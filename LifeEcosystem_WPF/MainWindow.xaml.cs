using LifeEcosystem_WPF.Models;
using LifeEcosystem_WPF.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LifeEcosystem_WPF
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
        }

        //lewym przyciskiem myszki dodajemy/usuwamy ROŚLINĘ
        public void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs a)
        {
            if(viewModel.CanEdit &&sender is FrameworkElement element && element.DataContext is Cell cell)
            {
                cell.TogglePlant();
                viewModel.Board.RecalculateTotals();
            }
        }

        //prawym przyciskiem myszki dodajemy/usuwamy ZWIERZĘ
        public void Cell_MouseRightButtonDown(object sender, MouseButtonEventArgs a)
        {
            if (viewModel.CanEdit && sender is FrameworkElement element && element.DataContext is Cell cell)
            {
                cell.ToggleAnimal();
                viewModel.Board.RecalculateTotals();
            }
        }

    }
}