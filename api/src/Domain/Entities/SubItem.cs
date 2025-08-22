using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a sub-task or child item within a parent item.
    /// Sub-items allow breaking down larger tasks into smaller, manageable components.
    /// </summary>
    public class SubItem
    {
        /// <summary>
        /// The unique identifier for the sub-item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the parent item this sub-item belongs to.
        /// </summary>
        public Guid ParentItemId { get; set; }

        /// <summary>
        /// The content or description of the sub-item.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether this sub-item has been completed.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// The order in which this sub-item should be displayed relative to other sub-items.
        /// </summary>
        public int SortOrder { get; set; }
    }
}
