using System;
using System.Collections.Generic;
using MediatR;

namespace PulseTrack.Application.Features.Sections.Commands
{
    public record ReorderSectionsCommand(Guid ProjectId, IReadOnlyList<Guid> OrderedSectionIds)
        : IRequest<Unit>;
}
