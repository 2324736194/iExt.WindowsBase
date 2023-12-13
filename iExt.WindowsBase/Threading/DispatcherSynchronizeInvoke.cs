using System.ComponentModel;

namespace System.Windows.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public class DispatcherSynchronizeInvoke : ISynchronizeInvoke
    {
        private readonly Dispatcher _dispatcher;

        /// <inheritdoc />
        public bool InvokeRequired => !_dispatcher.CheckAccess();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        public DispatcherSynchronizeInvoke(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        /// <inheritdoc />
        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            return new DispatcherAsyncResult(
                _dispatcher.BeginInvoke(method, DispatcherPriority.Normal, args));
        }

        /// <inheritdoc />
        public object EndInvoke(IAsyncResult result)
        {
            var dispatcherAsyncResult = (DispatcherAsyncResult)result;
            dispatcherAsyncResult.Operation.Wait();
            return dispatcherAsyncResult.Operation.Result;
        }

        /// <inheritdoc />
        public object Invoke(Delegate method, object[] args)
        {
            return _dispatcher.Invoke(method, DispatcherPriority.Normal, args);
        }
    }
}