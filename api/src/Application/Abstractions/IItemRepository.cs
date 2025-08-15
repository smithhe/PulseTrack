using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Abstractions
{
    public interface IItemRepository
    {
        Task<IReadOnlyList<Item>> ListAsync(Guid? projectId, CancellationToken cancellationToken);
        Task<Item> AddAsync(Item item, CancellationToken cancellationToken);
        Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(Item item, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
