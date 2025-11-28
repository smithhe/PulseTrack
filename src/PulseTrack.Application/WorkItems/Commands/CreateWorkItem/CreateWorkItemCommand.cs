using System;
using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Enums;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Commands.CreateWorkItem;

public sealed record CreateWorkItemCommand(
    Guid ProjectId,
    string Title,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    Guid? FeatureId,
    Guid? OwnerId,
    string? DescriptionMarkdown,
    decimal? EstimatePoints,
    DateTime? DueAtUtc,
    IReadOnlyCollection<string>? Tags) : IRequest<Response<WorkItemDetails>>;

