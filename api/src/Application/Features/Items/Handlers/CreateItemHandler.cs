using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class CreateItemHandler : IRequestHandler<CreateItemCommand, Item>
    {
        private readonly IItemRepository _repository;

        public CreateItemHandler(IItemRepository repository)
        {
            _repository = repository;
        }

        public Task<Item> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Item item = new Item
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                SectionId = request.SectionId,
                Content = request.Content,
                DescriptionMd = request.DescriptionMd ?? string.Empty,
                Priority = request.Priority,
                Pinned = request.Pinned,
                CreatedAt = now,
                UpdatedAt = now,
                Completed = false,
            };

            return _repository.AddAsync(item, cancellationToken);
        }
    }
}
