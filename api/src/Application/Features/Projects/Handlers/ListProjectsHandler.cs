using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Projects.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Projects.Handlers
{
    public class ListProjectsHandler : IRequestHandler<ListProjectsQuery, IReadOnlyList<Project>>
    {
        private readonly IProjectRepository _repository;

        public ListProjectsHandler(IProjectRepository repository)
        {
            this._repository = repository;
        }

        public Task<IReadOnlyList<Project>> Handle(
            ListProjectsQuery request,
            CancellationToken cancellationToken
        )
        {
            return this._repository.ListAsync(cancellationToken);
        }
    }
}
