using System;

namespace PulseTrack.Domain.Entities
{
    public class Backup
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
