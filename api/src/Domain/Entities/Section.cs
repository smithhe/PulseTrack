using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a section within a project that groups related items together.
    /// Sections provide organization within projects and can be reordered.
    /// </summary>
    public class Section
    {
        /// <summary>
        /// The unique identifier for the section.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the project this section belongs to.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// The display name of the section.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The order in which this section should be displayed relative to other sections within the same project.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// The timestamp when the section was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when the section was last updated.
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
