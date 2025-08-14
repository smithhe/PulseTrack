using System;

namespace PulseTrack.Shared.Requests.Items
{
    public record CreateItemRequest(Guid ProjectId, Guid? SectionId, string Content, string? DescriptionMd, int Priority, bool Pinned);
}
