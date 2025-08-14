using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class ListItemsHandler : IRequestHandler<ListItemsQuery, IReadOnlyList<Item>>
    {
        private readonly IItemRepository _repository;

        public ListItemsHandler(IItemRepository repository) { _repository = repository; }

        public Task<IReadOnlyList<Item>> Handle(ListItemsQuery request, CancellationToken cancellationToken)
        {
            return _repository.ListAsync(request.ProjectId, cancellationToken);
        }
    }
}
