using System;
using System.Collections.Generic;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a task or item within a project and optional section.
    /// Items can have sub-items, labels, due dates, reminders, and various states.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The unique identifier for the item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the project this item belongs to.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// The ID of the section this item belongs to (optional).
        /// </summary>
        public Guid? SectionId { get; set; }

        /// <summary>
        /// The main content or title of the item.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The detailed description of the item in Markdown format.
        /// </summary>
        public string DescriptionMd { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether this item is pinned to the top of its section/project.
        /// </summary>
        public bool Pinned { get; set; }

        /// <summary>
        /// The priority level of the item (higher values indicate higher priority).
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Indicates whether this item has been completed.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// The timestamp when the item was completed (null if not completed).
        /// </summary>
        public DateTimeOffset? CompletedAt { get; set; }

        /// <summary>
        /// The timestamp when the item was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when the item was last updated.
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// The sub-items or child tasks associated with this item.
        /// </summary>
        public List<SubItem> SubItems { get; set; } = new List<SubItem>();

        /// <summary>
        /// The labels associated with this item for categorization.
        /// </summary>
        public List<ItemLabel> ItemLabels { get; set; } = new List<ItemLabel>();

        /// <summary>
        /// The due date information for this item (if any).
        /// </summary>
        public DueDate? DueDate { get; set; }

        /// <summary>
        /// The reminders associated with this item.
        /// </summary>
        public List<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
