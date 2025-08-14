using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class CompleteItemHandler : IRequestHandler<CompleteItemCommand, Item?>,
                                        IRequestHandler<UncompleteItemCommand, Item?>
    {
        private readonly IItemRepository _repository;

        public CompleteItemHandler(IItemRepository repository) { _repository = repository; }

        public async Task<Item?> Handle(CompleteItemCommand request, CancellationToken cancellationToken)
        {
            Item? existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return null;
            existing.Completed = true;
            existing.CompletedAt = DateTimeOffset.UtcNow;
            existing.UpdatedAt = existing.CompletedAt.Value;
            await _repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }

        public async Task<Item?> Handle(UncompleteItemCommand request, CancellationToken cancellationToken)
        {
            Item? existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return null;
            existing.Completed = false;
            existing.CompletedAt = null;
            existing.UpdatedAt = DateTimeOffset.UtcNow;
            await _repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}


