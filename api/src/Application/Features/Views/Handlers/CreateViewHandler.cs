using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Views.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Views.Handlers
{
    public class CreateViewHandler : IRequestHandler<CreateViewCommand, View>
    {
        private readonly IViewRepository _repository;

        public CreateViewHandler(IViewRepository repository)
        {
            _repository = repository;
        }

        public async Task<View> Handle(
            CreateViewCommand request,
            CancellationToken cancellationToken
        )
        {
            View view = new View
            {
                Id = Guid.NewGuid(),
                Name = request.Request.Name,
                Description = request.Request.Description,
                ProjectId = request.Request.ProjectId,
                ViewType = request.Request.ViewType,
                FilterJson = request.Request.FilterJson,
                SortBy = request.Request.SortBy,
                IsDefault = request.Request.IsDefault,
                IsShared = request.Request.IsShared,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
            };

            return await _repository.AddAsync(view, cancellationToken);
        }
    }
}
