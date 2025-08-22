using System;

namespace PulseTrack.Shared.Requests.Views
{
    public record UpdateViewRequest(
        string Name,
        string? Description,
        Guid? ProjectId,
        string ViewType,
        string? FilterJson,
        string? SortBy,
        bool IsDefault,
        bool IsShared
    );
}
