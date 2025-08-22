using System;

namespace PulseTrack.Domain.Entities
{
    public class View
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? ProjectId { get; set; }
        public string ViewType { get; set; } = "scheduled"; // scheduled, list, board, etc.
        public string? FilterJson { get; set; }
        public string? SortBy { get; set; }
        public bool IsDefault { get; set; }
        public bool IsShared { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
