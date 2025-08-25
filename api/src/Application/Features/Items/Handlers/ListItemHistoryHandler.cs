using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class ListItemHistoryHandler
        : IRequestHandler<ListItemHistoryQuery, IReadOnlyList<ItemHistory>>
    {
        private readonly IItemHistoryRepository _repository;

        public ListItemHistoryHandler(IItemHistoryRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<ItemHistory>> Handle(
            ListItemHistoryQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.ListByItemAsync(request.ItemId, cancellationToken);
        }
    }
}
