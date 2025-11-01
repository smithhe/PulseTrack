using System;
using Microsoft.Extensions.DependencyInjection;
using PulseTrack.Presentation.ViewModels;

namespace PulseTrack.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<MainWindowViewModel>();

        return services;
    }
}

