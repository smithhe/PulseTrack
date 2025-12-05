using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Commands.UpdateWorkItem;

internal sealed class UpdateWorkItemCommandHandler : IRequestHandler<UpdateWorkItemCommand, ResponseBase>
{
    private readonly IWorkItemRepository _repository;

    public UpdateWorkItemCommandHandler(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseBase> Handle(UpdateWorkItemCommand request, CancellationToken cancellationToken)
    {
        WorkItem? workItem = await _repository.GetAsync(request.WorkItemId, cancellationToken);
        if (workItem is null)
        {
            return ResponseBase.Failure("Work item not found.");
        }

        DateTime now = DateTime.UtcNow;

        if (request.Title is not null)
        {
            workItem.UpdateTitle(request.Title, now);
        }

        if (request.DescriptionMarkdown is not null)
        {
            workItem.UpdateDescription(request.DescriptionMarkdown, now);
        }

        if (request.Notes is not null)
        {
            workItem.UpdateNotes(request.Notes, now);
        }

        if (request.FeatureId.HasValue)
        {
            workItem.AssignFeature(request.FeatureId, now);
        }

        if (request.OwnerId.HasValue)
        {
            workItem.AssignOwner(request.OwnerId, now);
        }

        if (request.Priority.HasValue)
        {
            workItem.ChangePriority(request.Priority.Value, now);
        }

        if (request.UpdateEstimate)
        {
            workItem.SetEstimate(request.EstimatePoints, now);
        }

        if (request.UpdateDueDate)
        {
            workItem.SetDueDate(request.DueAtUtc, now);
        }

        if (request.Tags is not null)
        {
            workItem.ReplaceTags(request.Tags, now);
        }

        await _repository.UpdateAsync(workItem, cancellationToken);

        return ResponseBase.Success("Work item updated.");
    }
}

