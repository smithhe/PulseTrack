using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class UnPinItemHandler : IRequestHandler<UnPinItemCommand, Item?>
    {
        private readonly IItemRepository _repository;

        public UnPinItemHandler(IItemRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Item?> Handle(
            UnPinItemCommand request,
            CancellationToken cancellationToken
        )
        {
            Item? existing = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (existing is null)
            {
                return null;
            }

            existing.Pinned = false;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await _repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}
