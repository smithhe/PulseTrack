using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Abstractions;
using PulseTrack.Application.Features.DueDates.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.DueDates.Handlers
{
    public class UpsertDueDateHandler : IRequestHandler<UpsertDueDateCommand, DueDate>
    {
        private readonly IDueDateRepository _repository;

        public UpsertDueDateHandler(IDueDateRepository repository)
        {
            _repository = repository;
        }

        public async Task<DueDate> Handle(
            UpsertDueDateCommand request,
            CancellationToken cancellationToken
        )
        {
            DueDate due = new DueDate
            {
                ItemId = request.ItemId,
                DateUtc = request.DateUtc,
                Timezone = request.Timezone,
                IsRecurring = request.IsRecurring,
                RecurrenceType = request.RecurrenceType,
                RecurrenceInterval = request.RecurrenceInterval,
                RecurrenceCount = request.RecurrenceCount,
                RecurrenceEndUtc = request.RecurrenceEndUtc,
                RecurrenceWeeks = request.RecurrenceWeeks,
            };

            return await _repository.UpsertAsync(due, cancellationToken);
        }
    }
}
