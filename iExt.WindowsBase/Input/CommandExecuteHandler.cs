namespace System.Windows.Input
{
    /// <summary>
    /// 命令执行委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public delegate void CommandExecuteHandler<in T>(T parameter);
}