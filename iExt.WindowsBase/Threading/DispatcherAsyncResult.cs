using System.Threading;

namespace System.Windows.Threading
{
    /// <inheritdoc />
    public class DispatcherAsyncResult : IAsyncResult
    {
        private readonly IAsyncResult _result;

        /// <summary>
        /// 
        /// </summary>
        public DispatcherOperation Operation { get; }

        /// <inheritdoc />
        public bool IsCompleted => _result.IsCompleted;

        /// <inheritdoc />
        public WaitHandle AsyncWaitHandle => _result.AsyncWaitHandle;

        /// <inheritdoc />
        public object AsyncState => _result.AsyncState;

        /// <inheritdoc />
        public bool CompletedSynchronously => _result.CompletedSynchronously;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        public DispatcherAsyncResult(DispatcherOperation operation)
        {
            Operation = operation;
            _result = operation.Task;
        }
    }
}