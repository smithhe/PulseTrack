using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.Views.Commands;

namespace PulseTrack.Application.Features.Views.Handlers
{
    public class DeleteViewHandler : IRequestHandler<DeleteViewCommand>
    {
        private readonly IViewRepository _repository;

        public DeleteViewHandler(IViewRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteViewCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
