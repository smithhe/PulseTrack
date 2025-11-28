using System;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Shared.Dtos;

public sealed record ChangeWorkItemStatusRequest(
    Guid WorkItemId,
    WorkItemStatus Status,
    DateTime ChangedAtUtc);