using System;
using System.Collections.Generic;

namespace PulseTrack.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid? SourceAccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public int SortOrder { get; set; }
        public bool IsInbox { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public List<Section> Sections { get; set; } = new();
    }
}
