using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;


//https://docs.microsoft.com/en-us/archive/msdn-magazine/2014/april/async-programming-patterns-for-asynchronous-mvvm-applications-commands#handling-asynchronous-command-completion-via-data-binding

namespace NetChat2.Commands
{
    
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }

    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public abstract bool CanExecute(object parameter);
        public abstract Task ExecuteAsync(object parameter);
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class AsyncCommand : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<Task> _command;
        private readonly Predicate<object> _canExecute;
        private NotifyTaskCompletion _execution;

        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncCommand(Func<Task> command) : this(command, null) { }
        public AsyncCommand(Func<Task> command, Predicate<object> canExecute)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (Execution == null || !Execution.IsNotCompleted && _canExecute == null) return true;
            return (!Execution.IsNotCompleted && _canExecute(parameter));
        }
        public override Task ExecuteAsync(object parameter)
        {
            Execution = new NotifyTaskCompletion(_command());
            return Execution.TaskCompletion;
        }
        // Raises PropertyChanged
        public NotifyTaskCompletion Execution { get; private set; }
    }

    public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<Task<TResult>> _command;
        private readonly Predicate<object> _canExecute;
        private NotifyTaskCompletion<TResult> _execution;

        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncCommand(Func<Task<TResult>> command) : this (command, null) { }
        public AsyncCommand(Func<Task<TResult>> command, Predicate<object> canExecute)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (!Execution.IsNotCompleted && _canExecute == null) return true;
            return (!Execution.IsNotCompleted && _canExecute(parameter));
        }
        public override Task ExecuteAsync(object parameter)
        {
            Execution = new NotifyTaskCompletion<TResult>(_command());
            return Execution.TaskCompletion;
        }
        // Raises PropertyChanged
        public NotifyTaskCompletion<TResult> Execution { get; private set; }
    }



    public class RelayCommandAsync : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Predicate<object> _canExecute;
        private bool isExecuting;

        public RelayCommandAsync(Func<Task> execute) : this(execute, null) { }

        public RelayCommandAsync(Func<Task> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (!isExecuting && _canExecute == null) return true;
            return (!isExecuting && _canExecute(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public async void Execute(object parameter)
        {
            isExecuting = true;
            try { await _execute(); }
            finally { isExecuting = false; }
        }
    }
}
