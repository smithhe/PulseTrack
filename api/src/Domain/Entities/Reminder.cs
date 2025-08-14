using System;

namespace PulseTrack.Domain.Entities
{
    public class Reminder
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public DateTimeOffset RemindAtUtc { get; set; }
        public string Timezone { get; set; } = "UTC";
        public string? MetaJson { get; set; }
    }
}
