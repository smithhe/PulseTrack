using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.DueDates.Commands
{
    public record UpsertDueDateCommand(
        Guid ItemId,
        DateTime DateUtc,
        string Timezone,
        bool IsRecurring,
        string? RecurrenceType,
        int? RecurrenceInterval,
        int? RecurrenceCount,
        DateTime? RecurrenceEndUtc,
        int? RecurrenceWeeks
    ) : IRequest<DueDate>;
}
