using System;

namespace PulseTrack.Domain.Entities
{
    public class SubItem
    {
        public Guid Id { get; set; }
        public Guid ParentItemId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public int SortOrder { get; set; }
    }
}
