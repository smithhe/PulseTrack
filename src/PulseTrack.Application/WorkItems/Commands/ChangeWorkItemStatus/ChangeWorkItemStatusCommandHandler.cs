using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Commands.ChangeWorkItemStatus;

internal sealed class ChangeWorkItemStatusCommandHandler : IRequestHandler<ChangeWorkItemStatusCommand, ResponseBase>
{
    private readonly IWorkItemRepository _repository;

    public ChangeWorkItemStatusCommandHandler(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseBase> Handle(ChangeWorkItemStatusCommand request, CancellationToken cancellationToken)
    {
        WorkItem? workItem = await _repository.GetAsync(request.WorkItemId, cancellationToken);
        if (workItem is null)
        {
            return ResponseBase.Failure("Work item not found.");
        }

        workItem.ChangeStatus(request.Status, request.ChangedAtUtc);

        await _repository.UpdateAsync(workItem, cancellationToken);

        return ResponseBase.Success("Work item status updated.");
    }
}

