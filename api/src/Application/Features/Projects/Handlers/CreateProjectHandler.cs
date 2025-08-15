using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Projects.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Projects.Handlers
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, Project>
    {
        private readonly IProjectRepository _repository;

        public CreateProjectHandler(IProjectRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Project> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken
        )
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Project project = new Project
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Color = request.Color,
                Icon = request.Icon,
                IsInbox = request.IsInbox,
                SortOrder = 0,
                CreatedAt = now,
                UpdatedAt = now,
            };

            return await this._repository.AddAsync(project, cancellationToken);
        }
    }
}
