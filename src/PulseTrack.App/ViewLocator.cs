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

        Type viewModelType = param.GetType();
        string viewName = viewModelType.FullName!
            .Replace("ViewModel", "View", StringComparison.Ordinal)
            .Replace("Presentation.ViewModels", "App.Views", StringComparison.Ordinal);

        string? viewAssembly = typeof(App).Assembly.FullName;
        Type? type = Type.GetType($"{viewName}, {viewAssembly}");

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
