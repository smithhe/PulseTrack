using System;

namespace PulseTrack.Domain.Entities
{
    public class SourceAccount
    {
        public Guid Id { get; set; }
        public string CredentialsJson { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        public DateTimeOffset? LastSyncAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
