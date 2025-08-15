using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class GetItemByIdHandler : IRequestHandler<GetItemByIdQuery, Item?>
    {
        private readonly IItemRepository _repository;

        public GetItemByIdHandler(IItemRepository repository)
        {
            _repository = repository;
        }

        public Task<Item?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
