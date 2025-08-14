using System;
using System.Collections.Generic;

namespace PulseTrack.Shared.Requests.Sections
{
    public record ReorderSectionsRequest(IReadOnlyList<Guid> OrderedSectionIds);
}
