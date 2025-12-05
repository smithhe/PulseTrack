using System;
using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Enums;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Commands.UpdateWorkItem;

public sealed record UpdateWorkItemCommand(
    Guid WorkItemId,
    string? Title,
    string? DescriptionMarkdown,
    string? Notes,
    Guid? FeatureId,
    Guid? OwnerId,
    WorkItemPriority? Priority,
    decimal? EstimatePoints,
    bool UpdateEstimate,
    DateTime? DueAtUtc,
    bool UpdateDueDate,
    IReadOnlyCollection<string>? Tags) : IRequest<ResponseBase>;

