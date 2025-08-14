using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Sections.Commands;

namespace PulseTrack.Application.Features.Sections.Handlers
{
    public class DeleteSectionHandler : IRequestHandler<DeleteSectionCommand, Unit>
    {
        private readonly ISectionRepository _repository;

        public DeleteSectionHandler(ISectionRepository repository) { _repository = repository; }

        public async Task<Unit> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}


