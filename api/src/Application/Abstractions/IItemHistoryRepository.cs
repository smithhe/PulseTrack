using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Abstractions
{
    public interface IItemHistoryRepository
    {
        Task<IReadOnlyList<ItemHistory>> ListByItemAsync(Guid itemId, CancellationToken cancellationToken);
    }
}


