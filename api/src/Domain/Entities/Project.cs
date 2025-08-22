using System;
using System.Collections.Generic;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a project that contains sections and items for task management.
    /// Projects can be regular projects or special inbox projects for uncategorized items.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// The unique identifier for the project.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the source account this project belongs to (optional for single-tenant scenarios).
        /// </summary>
        public Guid? SourceAccountId { get; set; }

        /// <summary>
        /// The display name of the project.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The color associated with the project for UI display purposes.
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// The icon associated with the project for UI display purposes.
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// The order in which this project should be displayed relative to other projects.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Indicates whether this project serves as the inbox for uncategorized items.
        /// </summary>
        public bool IsInbox { get; set; }

        /// <summary>
        /// The timestamp when the project was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when the project was last updated.
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// The sections contained within this project.
        /// </summary>
        public List<Section> Sections { get; set; } = new List<Section>();
    }
}
