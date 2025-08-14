using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class UpdateItemHandler : IRequestHandler<UpdateItemCommand, Item?>
    {
        private readonly IItemRepository _repository;

        public UpdateItemHandler(IItemRepository repository) { _repository = repository; }

        public async Task<Item?> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            Item? existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return null;

            existing.ProjectId = request.ProjectId;
            existing.SectionId = request.SectionId;
            existing.Content = request.Content;
            existing.DescriptionMd = request.DescriptionMd ?? string.Empty;
            existing.Priority = request.Priority;
            existing.Pinned = request.Pinned;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await _repository.UpdateAsync(existing, cancellationToken);
            return existing;
        }
    }
}


