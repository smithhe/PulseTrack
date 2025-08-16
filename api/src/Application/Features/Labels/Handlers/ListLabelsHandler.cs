using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Labels.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Labels.Handlers
{
    public class ListLabelsHandler : IRequestHandler<ListLabelsQuery, IReadOnlyList<Label>>
    {
        private readonly ILabelRepository _repository;

        public ListLabelsHandler(ILabelRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<Label>> Handle(
            ListLabelsQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.ListAsync(cancellationToken);
        }
    }
}
