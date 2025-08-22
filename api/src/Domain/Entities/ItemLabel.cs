using System;

namespace PulseTrack.Domain.Entities
{
    /// <summary>
    /// Represents the many-to-many relationship between items and labels.
    /// This junction table allows items to have multiple labels and labels to be assigned to multiple items.
    /// </summary>
    public class ItemLabel
    {
        /// <summary>
        /// The ID of the item in this relationship.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The ID of the label in this relationship.
        /// </summary>
        public Guid LabelId { get; set; }
    }
}
