using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a custom view for filtering and organizing items.
    /// Views can be project-specific or global and support different visualization types.
    /// </summary>
    public class View
    {
        /// <summary>
        /// The unique identifier for the view.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The display name of the view.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A detailed description of what this view shows or filters.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The ID of the project this view belongs to (optional for global views).
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// The type of view (e.g., "scheduled", "list", "board", "calendar").
        /// </summary>
        public string ViewType { get; set; } = "scheduled";

        /// <summary>
        /// JSON representation of the filter criteria for this view.
        /// </summary>
        public string? FilterJson { get; set; }

        /// <summary>
        /// The field to sort by (e.g., "created_at", "due_date", "priority").
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// Indicates whether this is the default view for the project/user.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Indicates whether this view is shared with other users.
        /// </summary>
        public bool IsShared { get; set; }

        /// <summary>
        /// The timestamp when the view was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when the view was last updated.
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
