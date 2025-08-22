using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a full-text search index entry for efficient searching across entities.
    /// Uses PostgreSQL tsvector for optimized text search capabilities.
    /// </summary>
    public class SearchIndex
    {
        /// <summary>
        /// The type of entity being indexed (e.g., "Item", "Project", "Label").
        /// </summary>
        public string EntityType { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier of the indexed entity.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// The PostgreSQL tsvector containing the searchable content of the entity.
        /// </summary>
        public string? ContentTsvector { get; set; }
    }
}
