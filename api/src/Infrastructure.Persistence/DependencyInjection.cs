using Microsoft.Extensions.DependencyInjection;
using PulseTrack.Application.Abstractions;
using PulseTrack.Infrastructure.Persistence.Repositories;

namespace PulseTrack.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            return services;
        }
    }
}


