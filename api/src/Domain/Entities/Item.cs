using System;
using System.Collections.Generic;

namespace PulseTrack.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? SectionId { get; set; }
        public Guid? SourceAccountId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string DescriptionMd { get; set; } = string.Empty;
        public bool Pinned { get; set; }
        public int Priority { get; set; }
        public bool Completed { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public List<SubItem> SubItems { get; set; } = new();
        public List<ItemLabel> ItemLabels { get; set; } = new();
        public DueDate? DueDate { get; set; }
        public List<Reminder> Reminders { get; set; } = new();
    }
}
