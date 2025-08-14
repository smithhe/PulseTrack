using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Sections.Commands;

namespace PulseTrack.Application.Features.Sections.Handlers
{
    public class ReorderSectionsHandler : IRequestHandler<ReorderSectionsCommand, Unit>
    {
        private readonly ISectionRepository _repository;

        public ReorderSectionsHandler(ISectionRepository repository) { _repository = repository; }

        public async Task<Unit> Handle(ReorderSectionsCommand request, CancellationToken cancellationToken)
        {
            await _repository.ReorderAsync(request.ProjectId, request.OrderedSectionIds, cancellationToken);
            return Unit.Value;
        }
    }
}


