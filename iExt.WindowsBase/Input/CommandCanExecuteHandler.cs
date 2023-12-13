namespace System.Windows.Input
{
    /// <summary>
    /// 命令是否执行委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public delegate bool CommandCanExecuteHandler<in T>(T parameter);
}