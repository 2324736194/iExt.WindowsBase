using System.Threading.Tasks;

namespace System.Windows.Input
{
    /// <summary>
    /// 任务命令执行委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public delegate Task TaskCommandExecuteHandler<in T>(T parameter);
}   