using System;
using System.Collections.Generic;
using System.Text;
using WebStore.WPF.Infrastructure.Commands.Base;

namespace WebStore.WPF.Infrastructure.Commands
{
    internal class LambdaCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }

        public override bool CanExecute(object parameter) => (_CanExecute?.Invoke(parameter) ?? true) && base.CanExecute(parameter);

        public override void Execute(object parameter) => _Execute(parameter);
    }
}
