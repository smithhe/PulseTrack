using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents due date information for an item, including recurring date support.
    /// Due dates can be one-time or recurring with various patterns.
    /// </summary>
    public class DueDate
    {
        /// <summary>
        /// The ID of the item this due date belongs to (also serves as primary key).
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The due date in UTC timezone.
        /// </summary>
        public DateTime DateUtc { get; set; }

        /// <summary>
        /// The timezone identifier for the due date (e.g., "America/New_York", "UTC").
        /// </summary>
        public string Timezone { get; set; } = "UTC";

        /// <summary>
        /// Indicates whether this due date repeats on a schedule.
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// The type of recurrence (e.g., "daily", "weekly", "monthly", "yearly").
        /// </summary>
        public string? RecurrenceType { get; set; }

        /// <summary>
        /// The interval between recurrences (e.g., every N days/weeks/months).
        /// </summary>
        public int? RecurrenceInterval { get; set; }

        /// <summary>
        /// The maximum number of times this due date should recur (null for unlimited).
        /// </summary>
        public int? RecurrenceCount { get; set; }

        /// <summary>
        /// The date when the recurring due date should end (null for no end date).
        /// </summary>
        public DateTime? RecurrenceEndUtc { get; set; }

        /// <summary>
        /// For weekly recurrences, indicates which days of the week (bitmask or comma-separated).
        /// </summary>
        public int? RecurrenceWeeks { get; set; }

        public override bool Equals(object? obj)
        {
            DueDate? other = obj as DueDate;

            return this.ItemId == other?.ItemId
                && this.DateUtc == other.DateUtc
                && this.Timezone == other.Timezone
                && this.IsRecurring == other.IsRecurring
                && this.RecurrenceType == other.RecurrenceType
                && this.RecurrenceInterval == other.RecurrenceInterval
                && this.RecurrenceCount == other.RecurrenceCount
                && this.RecurrenceEndUtc == other.RecurrenceEndUtc
                && this.RecurrenceWeeks == other.RecurrenceWeeks;
        }
    }
}
