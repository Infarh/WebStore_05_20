using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WebStore.WPF.Infrastructure.Commands.Base
{
    internal abstract class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested += value;
        }

        public virtual bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);
    }
}
