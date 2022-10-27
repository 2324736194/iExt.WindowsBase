using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using iExt.WindowsBase.Tests.Views;

namespace iExt.WindowsBase.Tests;

public class Node
{
    public object Data { get; set; }
    public IReadOnlyList<Node> Children { get; set; }
}