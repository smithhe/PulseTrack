using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Items.Commands;

namespace PulseTrack.Application.Features.Items.Handlers
{
    public class DeleteItemHandler : IRequestHandler<DeleteItemCommand, Unit>
    {
        private readonly IItemRepository _repository;

        public DeleteItemHandler(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            DeleteItemCommand request,
            CancellationToken cancellationToken
        )
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}
