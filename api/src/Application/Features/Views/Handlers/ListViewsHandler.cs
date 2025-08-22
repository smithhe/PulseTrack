using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Views.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Views.Handlers
{
    public class ListViewsHandler : IRequestHandler<ListViewsQuery, IReadOnlyList<View>>
    {
        private readonly IViewRepository _repository;

        public ListViewsHandler(IViewRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<View>> Handle(
            ListViewsQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.ListAsync(request.ProjectId, cancellationToken);
        }
    }
}
