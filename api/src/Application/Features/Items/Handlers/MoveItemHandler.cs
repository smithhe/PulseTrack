using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class MoveItemHandler : IRequestHandler<MoveItemCommand, Item?>
    {
        private readonly IItemRepository _repository;

        public MoveItemHandler(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<Item?> Handle(
            MoveItemCommand request,
            CancellationToken cancellationToken
        )
        {
            Item? existing = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (existing is null)
            {
                return null;
            }

            existing.ProjectId = request.ProjectId;
            existing.SectionId = request.SectionId;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await _repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}
