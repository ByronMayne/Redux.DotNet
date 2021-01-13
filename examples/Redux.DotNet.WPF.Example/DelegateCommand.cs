using System;
using System.Windows.Input;

namespace ReduxSharp.Wpf
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action m_action;

        public DelegateCommand(Action action)
        {
            m_action = action;
        }
        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
        => m_action();
    }
}
