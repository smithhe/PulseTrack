using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Views.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Views.Handlers
{
    public class UpdateViewHandler : IRequestHandler<UpdateViewCommand>
    {
        private readonly IViewRepository _repository;

        public UpdateViewHandler(IViewRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateViewCommand request, CancellationToken cancellationToken)
        {
            View view = new View
            {
                Id = request.Id,
                Name = request.Request.Name,
                Description = request.Request.Description,
                ProjectId = request.Request.ProjectId,
                ViewType = request.Request.ViewType,
                FilterJson = request.Request.FilterJson,
                SortBy = request.Request.SortBy,
                IsDefault = request.Request.IsDefault,
                IsShared = request.Request.IsShared,
                UpdatedAt = DateTimeOffset.UtcNow,
            };

            await _repository.UpdateAsync(view, cancellationToken);
        }
    }
}
