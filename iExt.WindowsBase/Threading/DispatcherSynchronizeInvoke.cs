using System.ComponentModel;

namespace System.Windows.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public class DispatcherSynchronizeInvoke : ISynchronizeInvoke
    {
        private readonly Dispatcher dispatcher;

        /// <inheritdoc />
        public bool InvokeRequired => !dispatcher.CheckAccess();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        public DispatcherSynchronizeInvoke(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        /// <inheritdoc />
        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            return new DispatcherAsyncResult(
                dispatcher.BeginInvoke(method, DispatcherPriority.Normal, args));
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
            return dispatcher.Invoke(method, DispatcherPriority.Normal, args);
        }
    }
}