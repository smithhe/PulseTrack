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
        {
            return null;
        }

        Type viewModelType = param.GetType();
        string viewModelName = viewModelType.Name;

        if (viewModelName.EndsWith("ViewModel", StringComparison.Ordinal))
        {
            viewModelName = viewModelName[..^9];
        }

        string viewName = $"{typeof(App).Namespace}.Views.{viewModelName}View";

        Type? type = Type.GetType($"{viewName}, {typeof(App).Assembly.FullName}") ??
                     typeof(App).Assembly.GetType(viewName);

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
