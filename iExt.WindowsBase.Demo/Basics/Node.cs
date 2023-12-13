using System.Collections.Generic;

namespace iExt.WindowsBase.Demo.Basics
{
    public class Node
    {
        public object Data { get; set; }
        public IReadOnlyList<Node> Children { get; set; }
    }
}

