using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Labels.Commands;

namespace PulseTrack.Application.Features.Labels.Handlers
{
    public class DeleteLabelHandler : IRequestHandler<DeleteLabelCommand, Unit>
    {
        private readonly ILabelRepository _repository;

        public DeleteLabelHandler(ILabelRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            DeleteLabelCommand request,
            CancellationToken cancellationToken
        )
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}
