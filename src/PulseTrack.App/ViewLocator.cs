using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PulseTrack.Presentation.ViewModels;

namespace PulseTrack.App;

public class ViewLocator : IDataTemplate
{

    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var viewModelType = param.GetType();
        var viewName = viewModelType.FullName!
            .Replace("ViewModel", "View", StringComparison.Ordinal)
            .Replace("Presentation.ViewModels", "App.Views", StringComparison.Ordinal);

        var viewAssembly = typeof(App).Assembly.FullName;
        var type = Type.GetType($"{viewName}, {viewAssembly}");

        if (type is not null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + viewName };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
