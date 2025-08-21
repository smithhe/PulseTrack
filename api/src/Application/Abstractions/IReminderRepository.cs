using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Abstractions
{
    public interface IReminderRepository
    {
        Task<IReadOnlyList<Reminder>> ListByItemAsync(
            Guid itemId,
            CancellationToken cancellationToken
        );
        Task<Reminder> AddAsync(Reminder reminder, CancellationToken cancellationToken);
        Task<Reminder?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(Reminder reminder, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
