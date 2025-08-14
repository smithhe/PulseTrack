using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Projects.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Projects.Handlers
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, Project?>
    {
        private readonly IProjectRepository _repository;

        public GetProjectByIdHandler(IProjectRepository repository)
        {
            this._repository = repository;
        }

        public Task<Project?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            return this._repository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}


