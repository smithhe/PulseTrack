using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Projects.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Projects.Handlers
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, Project?>
    {
        private readonly IProjectRepository _repository;

        public UpdateProjectHandler(IProjectRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Project?> Handle(
            UpdateProjectCommand request,
            CancellationToken cancellationToken
        )
        {
            Project? existing = await this._repository.GetByIdAsync(request.Id, cancellationToken);

            if (existing is null)
            {
                return null;
            }

            existing.Name = request.Name;
            existing.Color = request.Color;
            existing.Icon = request.Icon;
            existing.IsInbox = request.IsInbox;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await this._repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}
