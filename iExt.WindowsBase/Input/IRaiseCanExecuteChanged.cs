namespace System.Windows.Input
{
    /// <summary>
    /// 接口：可主动引发 <see cref="ICommand.CanExecuteChanged"/> 事件
    /// </summary>
    public interface IRaiseCanExecuteChanged:ICommand
    {
        /// <summary>
        /// 引发 <see cref="ICommand.CanExecuteChanged"/> 事件
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}