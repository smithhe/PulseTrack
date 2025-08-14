using System;

namespace PulseTrack.Domain.Entities
{
    public class SearchIndex
    {
        public string EntityType { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string? ContentTsvector { get; set; }
    }
}
