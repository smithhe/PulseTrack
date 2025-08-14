using System;

namespace PulseTrack.Domain.Entities
{
    public class Label
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Color { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
