using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.WorkItems.Shared;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Queries.GetWorkItem;

internal sealed class GetWorkItemQueryHandler : IRequestHandler<GetWorkItemQuery, Response<WorkItemDetails>>
{
    private readonly IWorkItemRepository _repository;

    public GetWorkItemQueryHandler(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<WorkItemDetails>> Handle(GetWorkItemQuery request, CancellationToken cancellationToken)
    {
        WorkItem? workItem = await _repository.GetAsync(request.WorkItemId, cancellationToken);
        if (workItem is null)
        {
            return Response<WorkItemDetails>.Failure("Work item not found.");
        }

        return Response<WorkItemDetails>.Success(workItem.ToDetails());
    }
}

