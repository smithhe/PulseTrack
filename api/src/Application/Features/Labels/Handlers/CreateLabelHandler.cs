using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Labels.Handlers
{
    public class CreateLabelHandler : IRequestHandler<CreateLabelCommand, Label>
    {
        private readonly ILabelRepository _repository;

        public CreateLabelHandler(ILabelRepository repository)
        {
            _repository = repository;
        }

        public Task<Label> Handle(CreateLabelCommand request, CancellationToken cancellationToken)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Label label = new Label
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Color = request.Color,
                CreatedAt = now,
                UpdatedAt = now,
            };

            return _repository.AddAsync(label, cancellationToken);
        }
    }
}
