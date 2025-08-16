using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Labels.Commands;

namespace PulseTrack.Application.Features.Labels.Handlers
{
    public class AssignLabelHandler : IRequestHandler<AssignLabelCommand, Unit>
    {
        private readonly ILabelRepository _repository;

        public AssignLabelHandler(ILabelRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            AssignLabelCommand request,
            CancellationToken cancellationToken
        )
        {
            await _repository.AssignAsync(request.ItemId, request.LabelId, cancellationToken);
            return Unit.Value;
        }
    }
}
