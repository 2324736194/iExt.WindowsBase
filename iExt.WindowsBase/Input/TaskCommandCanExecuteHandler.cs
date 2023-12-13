using System.Threading.Tasks;

namespace System.Windows.Input
{
    /// <summary>
    /// 任务命令是否执行委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public delegate Task<bool> TaskCommandCanExecuteHandler<in T>(T parameter);
}