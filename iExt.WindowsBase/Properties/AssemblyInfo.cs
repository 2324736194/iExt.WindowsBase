using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsDefinition(Namespace.Presentation, "System.Windows.Markup")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

internal static class Namespace
{
    public const string Presentation = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
} 