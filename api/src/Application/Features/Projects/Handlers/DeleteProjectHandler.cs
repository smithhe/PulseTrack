using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Projects.Commands;

namespace PulseTrack.Application.Features.Projects.Handlers
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, Unit>
    {
        private readonly IProjectRepository _repository;

        public DeleteProjectHandler(IProjectRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Unit> Handle(
            DeleteProjectCommand request,
            CancellationToken cancellationToken
        )
        {
            await this._repository.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}
