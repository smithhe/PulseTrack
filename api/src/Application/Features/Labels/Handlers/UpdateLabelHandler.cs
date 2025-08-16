using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Labels.Handlers
{
    public class UpdateLabelHandler : IRequestHandler<UpdateLabelCommand, Label?>
    {
        private readonly ILabelRepository _repository;

        public UpdateLabelHandler(ILabelRepository repository)
        {
            _repository = repository;
        }

        public async Task<Label?> Handle(
            UpdateLabelCommand request,
            CancellationToken cancellationToken
        )
        {
            Label? existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null)
            {
                return null;
            }

            existing.Name = request.Name;
            existing.Color = request.Color;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await _repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}
