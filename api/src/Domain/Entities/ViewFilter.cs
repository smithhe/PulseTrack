using System;

namespace PulseTrack.Domain.Entities
{
    public class ViewFilter
    {
        public Guid Id { get; set; }
        public Guid ViewId { get; set; }
        public string Field { get; set; } = string.Empty; // content, description, priority, due_date, etc.
        public string Operator { get; set; } = string.Empty; // equals, contains, gt, lt, etc.
        public string Value { get; set; } = string.Empty;
    }
}
