using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a label that can be assigned to items for categorization and filtering.
    /// Labels help organize items across projects and provide visual identification.
    /// </summary>
    public class Label
    {
        /// <summary>
        /// The unique identifier for the label.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The display name of the label.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The color associated with the label for UI display purposes.
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// The timestamp when the label was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when the label was last updated.
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
