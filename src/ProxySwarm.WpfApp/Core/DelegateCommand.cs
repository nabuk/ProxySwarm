using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProxySwarm.WpfApp.Core
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public DelegateCommand(Action<T> execute) : this(execute, null) { }
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region ICommand
        public bool CanExecute(object parameter = null)
        {
            return canExecute == null ? true : canExecute(parameter == null ? default(T) : (T)parameter);
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter = null)
        {
            execute(parameter == null ? default(T) : (T)parameter);
        }
        #endregion //ICommand

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }

    public class DelegateCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public DelegateCommand(Action execute) : this(execute, null) { }
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute ?? (() => true);
        }

        #region ICommand
        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            execute();
        }
        #endregion //ICommand

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public static DelegateCommand NotRunnable { get { return new DelegateCommand(() => { }, () => false); } }
    }
}
