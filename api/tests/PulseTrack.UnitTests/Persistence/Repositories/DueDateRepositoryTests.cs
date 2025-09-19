using System;
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
    public class DueDateRepositoryTests
    {
        private AppDbContext _dbContext = null!;
        private DueDateRepository _repository = null!;

        [SetUp]
        public void SetUp()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PulseTrack")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _dbContext = new AppDbContext(options);
            _repository = new DueDateRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Database.EnsureDeleted();
            _dbContext?.Dispose();
        }

        private static DueDate BuildDueDate(Guid? itemId = null)
        {
            return new DueDate
            {
                ItemId = itemId ?? Guid.NewGuid(),
                DateUtc = new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc),
                Timezone = "UTC",
                IsRecurring = true,
                RecurrenceType = "weekly",
                RecurrenceInterval = 2,
                RecurrenceCount = 5,
                RecurrenceEndUtc = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                RecurrenceWeeks = 0b0101010,
            };
        }

        [Test]
        public async Task GetByItemIdAsync_WhenExists_ReturnsEntity()
        {
            Guid itemId = Guid.NewGuid();
            _dbContext.DueDates.Add(BuildDueDate(itemId));
            await _dbContext.SaveChangesAsync();

            DueDate? result = await _repository.GetByItemIdAsync(itemId, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ItemId, Is.EqualTo(itemId));
        }

        [Test]
        public async Task GetByItemIdAsync_WhenNotExists_ReturnsNull()
        {
            DueDate? result = await _repository.GetByItemIdAsync(
                Guid.NewGuid(),
                CancellationToken.None
            );

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpsertAsync_WhenNew_InsertsEntity()
        {
            DueDate due = BuildDueDate();

            DueDate saved = await _repository.UpsertAsync(due, CancellationToken.None);

            DueDate? fromDb = await _dbContext.DueDates.FirstOrDefaultAsync(x =>
                x.ItemId == due.ItemId
            );

            Assert.Multiple(() =>
            {
                Assert.That(saved.ItemId, Is.EqualTo(due.ItemId));
                Assert.That(fromDb, Is.Not.Null);
            });
        }

        [Test]
        public async Task UpsertAsync_WhenExists_UpdatesFields()
        {
            Guid itemId = Guid.NewGuid();
            _dbContext.DueDates.Add(BuildDueDate(itemId));
            await _dbContext.SaveChangesAsync();

            DueDate updated = new DueDate
            {
                ItemId = itemId,
                DateUtc = new DateTime(2026, 2, 3, 4, 5, 6, DateTimeKind.Utc),
                Timezone = "America/New_York",
                IsRecurring = false,
                RecurrenceType = null,
                RecurrenceInterval = null,
                RecurrenceCount = null,
                RecurrenceEndUtc = null,
                RecurrenceWeeks = null,
            };

            await _repository.UpsertAsync(updated, CancellationToken.None);

            DueDate? fromDb = await _dbContext.DueDates.FirstOrDefaultAsync(x =>
                x.ItemId == itemId
            );
            Assert.Multiple(() =>
            {
                Assert.That(fromDb, Is.Not.Null);
                Assert.That(fromDb?.DateUtc, Is.EqualTo(updated.DateUtc));
                Assert.That(fromDb?.Timezone, Is.EqualTo(updated.Timezone));
                Assert.That(fromDb?.IsRecurring, Is.EqualTo(updated.IsRecurring));
                Assert.That(fromDb?.RecurrenceType, Is.EqualTo(updated.RecurrenceType));
                Assert.That(fromDb?.RecurrenceInterval, Is.EqualTo(updated.RecurrenceInterval));
                Assert.That(fromDb?.RecurrenceCount, Is.EqualTo(updated.RecurrenceCount));
                Assert.That(fromDb?.RecurrenceEndUtc, Is.EqualTo(updated.RecurrenceEndUtc));
                Assert.That(fromDb?.RecurrenceWeeks, Is.EqualTo(updated.RecurrenceWeeks));
            });
        }

        [Test]
        public async Task RemoveAsync_WhenExists_RemovesEntity()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            _dbContext.DueDates.Add(BuildDueDate(itemId));
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.RemoveAsync(itemId, CancellationToken.None);

            // Assert
            DueDate? fromDb = await _dbContext.DueDates.FirstOrDefaultAsync(x =>
                x.ItemId == itemId
            );
            Assert.That(fromDb, Is.Null);
        }

        [Test]
        public async Task RemoveAsync_WhenNotExists_NoOp()
        {
            // Act
            await _repository.RemoveAsync(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.Pass();
        }

        private sealed class ThrowingSaveChangesInterceptor : SaveChangesInterceptor
        {
            public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
                DbContextEventData eventData,
                InterceptionResult<int> result,
                CancellationToken cancellationToken = default
            )
            {
                throw new InvalidOperationException("Test induced failure during SaveChanges");
            }
        }

        [Test]
        public async Task UpsertAsync_WhenSaveFails_RollsBackAndThrows()
        {
            // Arrange
            Guid itemId = Guid.NewGuid();
            DueDate due = BuildDueDate(itemId);

            DbContextOptions<AppDbContext> throwingOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .AddInterceptors(new ThrowingSaveChangesInterceptor())
                    .Options;

            await using (AppDbContext throwingCtx = new AppDbContext(throwingOptions))
            {
                DueDateRepository repo = new DueDateRepository(throwingCtx);
                // Act + Assert
                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await repo.UpsertAsync(due, CancellationToken.None)
                );
            }

            // Assert no persistence occurred
            DbContextOptions<AppDbContext> verifyOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            await using (AppDbContext verify = new AppDbContext(verifyOptions))
            {
                DueDate? fromDb = await verify.DueDates.FirstOrDefaultAsync(x =>
                    x.ItemId == itemId
                );
                Assert.That(fromDb, Is.Null);
            }
        }

        [Test]
        public async Task RemoveAsync_WhenSaveFails_RollsBackAndThrows()
        {
            // Arrange: seed an entity
            Guid itemId = Guid.NewGuid();
            _dbContext.DueDates.Add(BuildDueDate(itemId));
            await _dbContext.SaveChangesAsync();

            DbContextOptions<AppDbContext> throwingOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .AddInterceptors(new ThrowingSaveChangesInterceptor())
                    .Options;

            await using (AppDbContext throwingCtx = new AppDbContext(throwingOptions))
            {
                DueDateRepository repo = new DueDateRepository(throwingCtx);
                // Act + Assert
                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await repo.RemoveAsync(itemId, CancellationToken.None)
                );
            }

            // Assert entity still exists (delete rolled back)
            DbContextOptions<AppDbContext> verifyOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            await using (AppDbContext verify = new AppDbContext(verifyOptions))
            {
                DueDate? fromDb = await verify.DueDates.FirstOrDefaultAsync(x =>
                    x.ItemId == itemId
                );
                Assert.That(fromDb, Is.Not.Null);
            }
        }
    }
}
