using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class UnCompleteItemHandler : IRequestHandler<UnCompleteItemCommand, Item?>
    {
        private readonly IItemRepository _repository;

        public UnCompleteItemHandler(IItemRepository itemRepository)
        {
            this._repository = itemRepository;
        }

        public async Task<Item?> Handle(
            UnCompleteItemCommand request,
            CancellationToken cancellationToken
        )
        {
            Item? existing = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (existing is null)
            {
                return null;
            }

            existing.Completed = false;
            existing.CompletedAt = null;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await _repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}
