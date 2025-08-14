using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Sections.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Sections.Handlers
{
    public class ListSectionsByProjectHandler : IRequestHandler<ListSectionsByProjectQuery, IReadOnlyList<Section>>
    {
        private readonly ISectionRepository _repository;

        public ListSectionsByProjectHandler(ISectionRepository repository) { _repository = repository; }

        public Task<IReadOnlyList<Section>> Handle(ListSectionsByProjectQuery request, CancellationToken cancellationToken)
        {
            return _repository.ListByProjectAsync(request.ProjectId, cancellationToken);
        }
    }
}


