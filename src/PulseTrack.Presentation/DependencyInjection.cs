using System;
using Microsoft.Extensions.DependencyInjection;
using PulseTrack.Presentation.Navigation;
using PulseTrack.Presentation.ViewModels;
using PulseTrack.Presentation.ViewModels.WorkItems;
using PulseTrack.Presentation.WorkItems.Services;

namespace PulseTrack.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IWorkItemsDataProvider, InMemoryWorkItemsDataProvider>();
        services.AddSingleton<WorkItemsViewModel>();
        services.AddSingleton<WorkItemDetailViewModel>();
        services.AddSingleton<INavigationSection, WorkItemsNavigationSection>();
        services.AddSingleton<MainWindowViewModel>();

        return services;
    }
}

