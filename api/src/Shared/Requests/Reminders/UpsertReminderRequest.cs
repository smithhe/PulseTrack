using System;

namespace PulseTrack.Shared.Requests.Reminders
{
    public record UpsertReminderRequest(
        DateTimeOffset RemindAtUtc,
        string Timezone,
        string? MetaJson
    );
}
