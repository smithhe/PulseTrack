using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.WorkItems.Shared;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Queries.ListWorkItems;

internal sealed class ListWorkItemsQueryHandler : IRequestHandler<ListWorkItemsQuery, Response<IReadOnlyList<WorkItemSummary>>>
{
    private readonly IWorkItemRepository _repository;

    public ListWorkItemsQueryHandler(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<IReadOnlyList<WorkItemSummary>>> Handle(ListWorkItemsQuery request, CancellationToken cancellationToken)
    {
        WorkItemRepositoryFilter filter = new WorkItemRepositoryFilter(
            request.ProjectId,
            request.FeatureId,
            request.OwnerId,
            request.Status,
            request.Priority,
            request.Tag);

        IReadOnlyList<WorkItemSummary> summaries = (await _repository.ListAsync(filter, cancellationToken))
            .Select(workItem => workItem.ToSummary())
            .ToArray();

        return Response<IReadOnlyList<WorkItemSummary>>.Success(summaries);
    }
}

