using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents an external account or service integration for syncing data.
    /// Source accounts enable importing items from external systems or services.
    /// </summary>
    public class SourceAccount
    {
        /// <summary>
        /// The unique identifier for the source account.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The credentials and connection details for the external service in JSON format.
        /// </summary>
        public string CredentialsJson { get; set; } = string.Empty;

        /// <summary>
        /// The current status of the source account (e.g., "active", "inactive", "error").
        /// </summary>
        public string Status { get; set; } = "active";

        /// <summary>
        /// The timestamp of the last successful synchronization with the external service.
        /// </summary>
        public DateTimeOffset? LastSyncAt { get; set; }

        /// <summary>
        /// The timestamp when the source account was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when the source account was last updated.
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
