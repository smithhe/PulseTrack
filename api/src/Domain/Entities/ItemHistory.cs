using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a historical record of changes made to an item.
    /// Used for audit trails and change tracking functionality.
    /// </summary>
    public class ItemHistory
    {
        /// <summary>
        /// The unique identifier for this history record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the item this history record belongs to.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The type of change that was made (e.g., "created", "updated", "deleted", "completed").
        /// </summary>
        public string ChangeType { get; set; } = string.Empty;

        /// <summary>
        /// The JSON representation of the item state before the change (null for creation).
        /// </summary>
        public string? BeforeJson { get; set; }

        /// <summary>
        /// The JSON representation of the item state after the change (null for deletion).
        /// </summary>
        public string? AfterJson { get; set; }

        /// <summary>
        /// The timestamp when the change occurred.
        /// </summary>
        public DateTimeOffset ChangedAt { get; set; }
    }
}
