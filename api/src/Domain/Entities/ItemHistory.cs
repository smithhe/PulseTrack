using System;

namespace PulseTrack.Domain.Entities
{
    public class ItemHistory
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ChangeType { get; set; } = string.Empty;
        public string? BeforeJson { get; set; }
        public string? AfterJson { get; set; }
        public DateTimeOffset ChangedAt { get; set; }
    }
}
