using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Sections.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Sections.Handlers
{
    public class CreateSectionHandler : IRequestHandler<CreateSectionCommand, Section>
    {
        private readonly ISectionRepository _repository;

        public CreateSectionHandler(ISectionRepository repository)
        {
            _repository = repository;
        }

        public Task<Section> Handle(
            CreateSectionCommand request,
            CancellationToken cancellationToken
        )
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Section section = new Section
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                Name = request.Name,
                SortOrder = request.SortOrder,
                CreatedAt = now,
                UpdatedAt = now,
            };

            return _repository.AddAsync(section, cancellationToken);
        }
    }
}
