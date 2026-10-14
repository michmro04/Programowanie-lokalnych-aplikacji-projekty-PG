using LifeEcosystem_WPF.Models;
using LifeEcosystem_WPF.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public void Call_MouseLeftButton(object sender, MouseButton a)
        {
            if(viewModel.CanEdit &&sender is FrameworkElement element && element.DataContext is Cell cell)
            {
                cell.TogglePlant();
            }
        }

        //prawym przyciskiem myszki dodajemy/usuwamy ZWIERZĘ
        public void Call_MouseRightButton(object sender, MouseButton a)
        {
            if (viewModel.CanEdit && sender is FrameworkElement element && element.DataContext is Cell cell)
            {
                cell.ToggleAnimal();
            }
        }






    }
}