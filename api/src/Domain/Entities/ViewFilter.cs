using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents an individual filter condition within a view.
    /// Multiple filters can be combined to create complex filtering logic.
    /// </summary>
    public class ViewFilter
    {
        /// <summary>
        /// The unique identifier for this filter condition.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the view this filter belongs to.
        /// </summary>
        public Guid ViewId { get; set; }

        /// <summary>
        /// The field to filter on (e.g., "content", "description", "priority", "due_date", "labels").
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// The comparison operator (e.g., "equals", "contains", "gt", "lt", "before", "after").
        /// </summary>
        public string Operator { get; set; } = string.Empty;

        /// <summary>
        /// The value to compare against.
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}
