using System;
using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Enums;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Queries.ListWorkItems;

public sealed record ListWorkItemsQuery(
    Guid? ProjectId,
    Guid? FeatureId,
    Guid? OwnerId,
    WorkItemStatus? Status,
    WorkItemPriority? Priority,
    string? Tag) : IRequest<Response<IReadOnlyList<WorkItemSummary>>>;

