using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Views.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Views.Handlers
{
    public class GetViewByIdHandler : IRequestHandler<GetViewByIdQuery, View?>
    {
        private readonly IViewRepository _repository;

        public GetViewByIdHandler(IViewRepository repository)
        {
            _repository = repository;
        }

        public Task<View?> Handle(
            GetViewByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
