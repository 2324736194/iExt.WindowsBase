using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml.Linq;
using iExt.WindowsBase.Tests.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace iExt.WindowsBase.Tests.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private string title = "Prism Application";
        private readonly char separator = '.';
        private readonly IReadOnlyList<Type> types;
        private readonly IReadOnlyList<string> namespaces;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ICommand LoadedCommand { get; }

        public IReadOnlyList<Node> Views { get; }

        public MainWindowViewModel(IRegionManager manager)
        {
            var t = typeof(MainWindow);
            var ns = t.Namespace;
            types = t.Assembly.GetTypes();
            namespaces = types.Select(p => p.Namespace).Distinct().ToList();
            regionManager = manager;
            //
            LoadedCommand = new DelegateCommand<Node>(LoadedCommandExecuteMethod);
            Views = GetViews(ns);
        }

        private IReadOnlyList<Node> GetViews(string ns)
        {
            // ReSharper disable once PossibleNullReferenceException
            var split = ns.Split(separator);
            var nsNode = new Node()
            {
                Data = split.Last(),
                Children = GetChildren(ns)
            };
            var list = new List<Node>();
            list.Add(nsNode);
            return list;
        }

        private IReadOnlyList<Node> GetChildren(string ns)
        {
            var children = new List<Node>();
            // 下一级命名空间
            var namespaceChildrean = GetNamespaceChildren(ns);
            foreach (var child in namespaceChildrean)
            {
                var node = new Node()
                {
                    Data = child.TrimStart(ns.ToArray()),
                    Children = GetChildren(child)
                };
                children.Add(node);
            }

            // 命名空间中的视图
            var views = types.Where(p => p.Namespace == ns && p.Name.EndsWith("View"));
            foreach (var view in views)
            {
                var viewNode = new Node()
                {
                    Data = view
                };
                children.Add(viewNode);
            }

            return children;
        }

        private IReadOnlyList<string> GetNamespaceChildren(string ns)
        {
            return namespaces.Where(p =>
            {
                var result = false;
                if (p.StartsWith(ns))
                {
                    var trimChars = ns.ToArray();
                    var trimNs = p.TrimStart(trimChars);
                    if (!string.IsNullOrEmpty(trimNs))
                    {
                        var split = trimNs.Split(separator);
                        result = split.Length == 1;
                    }
                }
                return result;
            }).ToList();
        }

        private void LoadedCommandExecuteMethod(Node node)
        {
            if (null == node)
            {
                return;
            }
            if (node.Data is Type view)
            {
                regionManager.RequestNavigate(MainWindow.ContentRegion, view.Name);
            }
        }
    }

   
}
