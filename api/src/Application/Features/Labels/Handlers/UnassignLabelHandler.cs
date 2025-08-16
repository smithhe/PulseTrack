using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Labels.Commands;

namespace PulseTrack.Application.Features.Labels.Handlers;

public class UnassignLabelHandler : IRequestHandler<UnassignLabelCommand, Unit>
{
    private readonly ILabelRepository _repository;

    public UnassignLabelHandler(ILabelRepository repository)
    {
        this._repository = repository;
    }

    public async Task<Unit> Handle(
        UnassignLabelCommand request,
        CancellationToken cancellationToken
    )
    {
        await _repository.UnassignAsync(request.ItemId, request.LabelId, cancellationToken);
        return Unit.Value;
    }
}
