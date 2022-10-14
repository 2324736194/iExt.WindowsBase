using System.Threading;

namespace System.Windows.Threading
{
    /// <inheritdoc />
    public class DispatcherAsyncResult : IAsyncResult
    {
        private readonly IAsyncResult result;

        /// <summary>
        /// 
        /// </summary>
        public DispatcherOperation Operation { get; }

        /// <inheritdoc />
        public bool IsCompleted => result.IsCompleted;

        /// <inheritdoc />
        public WaitHandle AsyncWaitHandle => result.AsyncWaitHandle;

        /// <inheritdoc />
        public object AsyncState => result.AsyncState;

        /// <inheritdoc />
        public bool CompletedSynchronously => result.CompletedSynchronously;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        public DispatcherAsyncResult(DispatcherOperation operation)
        {
            Operation = operation;
            result = operation.Task;
        }
    }
}