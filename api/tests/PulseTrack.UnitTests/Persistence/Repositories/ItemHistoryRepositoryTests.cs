using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Persistence;
using PulseTrack.Infrastructure.Persistence.Repositories;

namespace PulseTrack.UnitTests.Persistence.Repositories
{
    [TestFixture]
    public class ItemHistoryRepositoryTests
    {
        private AppDbContext _dbContext = null!;
        private ItemHistoryRepository _repository = null!;

        [SetUp]
        public void SetUp()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PulseTrack")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _dbContext = new AppDbContext(options);
            _repository = new ItemHistoryRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Database.EnsureDeleted();
            _dbContext?.Dispose();
        }

        private static ItemHistory BuildHistory(
            Guid? itemId = null,
            DateTimeOffset? changedAt = null,
            string changeType = "updated"
        )
        {
            return new ItemHistory
            {
                Id = Guid.NewGuid(),
                ItemId = itemId ?? Guid.NewGuid(),
                ChangeType = changeType,
                BeforeJson = "{\"a\":1}",
                AfterJson = "{\"a\":2}",
                ChangedAt = changedAt ?? new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.Zero),
            };
        }

        [Test]
        public async Task ListByItemAsync_WhenNone_ReturnsEmpty()
        {
            Guid itemId = Guid.NewGuid();

            IReadOnlyList<ItemHistory> result = await _repository.ListByItemAsync(
                itemId,
                CancellationToken.None
            );

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task ListByItemAsync_FiltersByItemId_AndOrdersByChangedAtDescending()
        {
            Guid itemA = Guid.NewGuid();
            Guid itemB = Guid.NewGuid();

            _dbContext.ItemHistories.AddRange(
                BuildHistory(itemA, new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), "a1"),
                BuildHistory(itemA, new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), "a2"),
                BuildHistory(itemA, new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), "a3"),
                BuildHistory(itemB, new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), "b1")
            );
            await _dbContext.SaveChangesAsync();

            IReadOnlyList<ItemHistory> result = await _repository.ListByItemAsync(
                itemA,
                CancellationToken.None
            );

            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(result[0].ChangedAt, Is.EqualTo(new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero)));
                Assert.That(result[1].ChangedAt, Is.EqualTo(new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero)));
                Assert.That(result[2].ChangedAt, Is.EqualTo(new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero)));
                Assert.That(result[0].ItemId, Is.EqualTo(itemA));
                Assert.That(result[1].ItemId, Is.EqualTo(itemA));
                Assert.That(result[2].ItemId, Is.EqualTo(itemA));
            });
        }

        [Test]
        public async Task ListByItemAsync_UsesAsNoTracking_ReturnsDetachedEntities()
        {
            Guid itemId = Guid.NewGuid();

            _dbContext.ItemHistories.AddRange(
                BuildHistory(itemId, new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)),
                BuildHistory(itemId, new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero))
            );
            await _dbContext.SaveChangesAsync();

            IReadOnlyList<ItemHistory> result = await _repository.ListByItemAsync(
                itemId,
                CancellationToken.None
            );

            Assert.That(result, Is.Not.Empty);
            foreach (ItemHistory h in result)
            {
                Assert.That(_dbContext.Entry(h).State, Is.EqualTo(EntityState.Detached));
            }
        }
    }
}
