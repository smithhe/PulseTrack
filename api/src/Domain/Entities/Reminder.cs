using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a reminder notification for an item.
    /// Reminders can be scheduled to alert users about upcoming due dates or tasks.
    /// </summary>
    public class Reminder
    {
        /// <summary>
        /// The unique identifier for the reminder.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the item this reminder is associated with.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The date and time when the reminder should be triggered (in UTC).
        /// </summary>
        public DateTimeOffset RemindAtUtc { get; set; }

        /// <summary>
        /// The timezone identifier for the reminder (e.g., "America/New_York", "UTC").
        /// </summary>
        public string Timezone { get; set; } = "UTC";

        /// <summary>
        /// Additional metadata about the reminder in JSON format (e.g., notification preferences).
        /// </summary>
        public string? MetaJson { get; set; }
    }
}
