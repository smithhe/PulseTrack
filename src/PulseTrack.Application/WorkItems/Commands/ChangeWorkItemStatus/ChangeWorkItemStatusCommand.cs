using System;
using MediatR;
using PulseTrack.Domain.Enums;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Commands.ChangeWorkItemStatus;

public sealed record ChangeWorkItemStatusCommand(
    Guid WorkItemId,
    WorkItemStatus Status,
    DateTime ChangedAtUtc) : IRequest<ResponseBase>;

