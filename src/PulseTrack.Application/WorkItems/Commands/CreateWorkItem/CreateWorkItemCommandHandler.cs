using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.WorkItems.Shared;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Commands.CreateWorkItem;

internal sealed class CreateWorkItemCommandHandler : IRequestHandler<CreateWorkItemCommand, Response<WorkItemDetails>>
{
    private readonly IWorkItemRepository _repository;

    public CreateWorkItemCommandHandler(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<WorkItemDetails>> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
    {
        DateTime now = DateTime.UtcNow;

        WorkItem workItem = new WorkItem(
            id: Guid.NewGuid(),
            projectId: request.ProjectId,
            title: request.Title,
            status: request.Status,
            priority: request.Priority,
            createdAtUtc: now);

        if (request.FeatureId.HasValue)
        {
            workItem.AssignFeature(request.FeatureId, now);
        }

        if (request.OwnerId.HasValue)
        {
            workItem.AssignOwner(request.OwnerId, now);
        }

        workItem.UpdateDescription(request.DescriptionMarkdown, now);

        if (request.Notes is not null)
        {
            workItem.UpdateNotes(request.Notes, now);
        }

        if (request.EstimatePoints.HasValue)
        {
            workItem.SetEstimate(request.EstimatePoints, now);
        }

        if (request.DueAtUtc.HasValue)
        {
            workItem.SetDueDate(request.DueAtUtc, now);
        }

        if (request.Tags is not null)
        {
            workItem.ReplaceTags(request.Tags, now);
        }

        WorkItem created = await _repository.CreateAsync(workItem, cancellationToken);

        return Response<WorkItemDetails>.Success(created.ToDetails(), "Work item created.");
    }
}

