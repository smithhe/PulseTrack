using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Sections.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Sections.Handlers
{
    public class UpdateSectionHandler : IRequestHandler<UpdateSectionCommand, Section?>
    {
        private readonly ISectionRepository _repository;

        public UpdateSectionHandler(ISectionRepository repository) { _repository = repository; }

        public async Task<Section?> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
        {
            Section? existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return null;
            existing.Name = request.Name;
            existing.SortOrder = request.SortOrder;
            existing.UpdatedAt = DateTimeOffset.UtcNow;
            await _repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}


