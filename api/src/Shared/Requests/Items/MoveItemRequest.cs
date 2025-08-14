using System;

namespace PulseTrack.Shared.Requests.Items
{
    public record MoveItemRequest(Guid ProjectId, Guid? SectionId);
}
