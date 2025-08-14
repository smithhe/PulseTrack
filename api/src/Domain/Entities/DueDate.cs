using System;

namespace PulseTrack.Domain.Entities
{
    public class DueDate
    {
        public Guid ItemId { get; set; }
        public DateTime DateUtc { get; set; }
        public string Timezone { get; set; } = "UTC";
        public bool IsRecurring { get; set; }
        public string? RecurrenceType { get; set; }
        public int? RecurrenceInterval { get; set; }
        public int? RecurrenceCount { get; set; }
        public DateTime? RecurrenceEndUtc { get; set; }
        public int? RecurrenceWeeks { get; set; }
    }
}
