using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents a backup of the application's data.
    /// Backups are created periodically to ensure data recovery capabilities.
    /// </summary>
    public class Backup
    {
        /// <summary>
        /// The unique identifier for the backup.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The URL or file path where the backup file is stored.
        /// </summary>
        public string FileUrl { get; set; } = string.Empty;

        /// <summary>
        /// The timestamp when the backup was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
    }
}
