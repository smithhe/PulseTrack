using System;

namespace PulseTrack.Shared.Requests.DueDates
{
    public record UpsertDueDateRequest(
        DateTime DateUtc,
        string Timezone,
        bool IsRecurring,
        string? RecurrenceType,
        int? RecurrenceInterval,
        int? RecurrenceCount,
        DateTime? RecurrenceEndUtc,
        int? RecurrenceWeeks
    );
}
