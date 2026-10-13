using System.Windows.Input;

namespace LifeEcosystem_WPF.ViewModels
{
    public class RelayCommand : ICommand
    {
        private Action<object?> executeAction;
        private Func<object?, bool>? canExecuteFunc;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            executeAction = execute;
            canExecuteFunc = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (canExecuteFunc == null)
            {
                return true;
            }
            return canExecuteFunc(parameter);
        }

        public void Execute(object? parameter)
        {
            executeAction(parameter);
        }
    }
}