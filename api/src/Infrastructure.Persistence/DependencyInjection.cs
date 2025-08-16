using Microsoft.Extensions.DependencyInjection;
using PulseTrack.Application.Abstractions;
using PulseTrack.Infrastructure.Persistence.Repositories;

namespace PulseTrack.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();
        }
    }
}
