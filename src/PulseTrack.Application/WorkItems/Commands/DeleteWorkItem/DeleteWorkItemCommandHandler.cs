using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Commands.DeleteWorkItem;

internal sealed class DeleteWorkItemCommandHandler : IRequestHandler<DeleteWorkItemCommand, ResponseBase>
{
    private readonly IWorkItemRepository _repository;

    public DeleteWorkItemCommandHandler(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseBase> Handle(DeleteWorkItemCommand request, CancellationToken cancellationToken)
    {
        WorkItem? existing = await _repository.GetAsync(request.WorkItemId, cancellationToken);
        if (existing is null)
        {
            return ResponseBase.Failure("Work item not found.");
        }

        await _repository.DeleteAsync(request.WorkItemId, cancellationToken);

        return ResponseBase.Success("Work item deleted.");
    }
}

