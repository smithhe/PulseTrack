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
    public class ItemRepositoryTests
    {
        private AppDbContext _dbContext = null!;
        private ItemRepository _repository = null!;

        [SetUp]
        public void SetUp()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PulseTrack")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _dbContext = new AppDbContext(options);
            _repository = new ItemRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Database.EnsureDeleted();
            _dbContext?.Dispose();
        }

        private static Item BuildItem(
            Guid? id = null,
            Guid? projectId = null,
            DateTimeOffset? createdAt = null
        )
        {
            return new Item
            {
                Id = id ?? Guid.NewGuid(),
                ProjectId = projectId ?? Guid.NewGuid(),
                CreatedAt = createdAt ?? new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
            };
        }

        [Test]
        public async Task ListAsync_WhenNoItems_ReturnsEmpty()
        {
            IReadOnlyList<Item> result = await _repository.ListAsync(null, CancellationToken.None);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task ListAsync_NoFilter_ReturnsAll_OrderedByCreatedAtAscending()
        {
            _dbContext.Items.AddRange(
                BuildItem(createdAt: new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero)),
                BuildItem(createdAt: new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero)),
                BuildItem(createdAt: new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero))
            );
            await _dbContext.SaveChangesAsync();

            IReadOnlyList<Item> result = await _repository.ListAsync(null, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(
                    result[0].CreatedAt,
                    Is.EqualTo(new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero))
                );
                Assert.That(
                    result[1].CreatedAt,
                    Is.EqualTo(new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero))
                );
                Assert.That(
                    result[2].CreatedAt,
                    Is.EqualTo(new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero))
                );
            });
        }

        [Test]
        public async Task ListAsync_WithProjectFilter_FiltersAndOrders()
        {
            Guid projectA = Guid.NewGuid();
            Guid projectB = Guid.NewGuid();

            _dbContext.Items.AddRange(
                BuildItem(
                    projectId: projectA,
                    createdAt: new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero)
                ),
                BuildItem(
                    projectId: projectA,
                    createdAt: new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero)
                ),
                BuildItem(
                    projectId: projectA,
                    createdAt: new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero)
                ),
                BuildItem(
                    projectId: projectB,
                    createdAt: new DateTimeOffset(2025, 2, 1, 0, 0, 0, TimeSpan.Zero)
                )
            );
            await _dbContext.SaveChangesAsync();

            IReadOnlyList<Item> result = await _repository.ListAsync(
                projectA,
                CancellationToken.None
            );

            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(result[0].ProjectId, Is.EqualTo(projectA));
                Assert.That(result[1].ProjectId, Is.EqualTo(projectA));
                Assert.That(result[2].ProjectId, Is.EqualTo(projectA));
                Assert.That(
                    result[0].CreatedAt,
                    Is.EqualTo(new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero))
                );
                Assert.That(
                    result[1].CreatedAt,
                    Is.EqualTo(new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero))
                );
                Assert.That(
                    result[2].CreatedAt,
                    Is.EqualTo(new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero))
                );
            });
        }

        [Test]
        public async Task AddAsync_InsertsEntity_AndReturnsSameInstance()
        {
            Item item = BuildItem();

            Item saved = await _repository.AddAsync(item, CancellationToken.None);

            Item? fromDb = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == item.Id);
            Assert.Multiple(() =>
            {
                Assert.That(saved, Is.SameAs(item));
                Assert.That(fromDb, Is.Not.Null);
                Assert.That(fromDb!.Id, Is.EqualTo(item.Id));
            });
        }

        private sealed class ThrowingSaveChangesInterceptor : SaveChangesInterceptor
        {
            public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
                DbContextEventData eventData,
                InterceptionResult<int> result,
                CancellationToken cancellationToken = default(CancellationToken)
            )
            {
                throw new InvalidOperationException("Test induced failure during SaveChanges");
            }
        }

        [Test]
        public async Task AddAsync_WhenSaveFails_RollsBackAndThrows()
        {
            Item item = BuildItem();

            DbContextOptions<AppDbContext> throwingOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .AddInterceptors(new ThrowingSaveChangesInterceptor())
                    .Options;

            await using (AppDbContext throwingCtx = new AppDbContext(throwingOptions))
            {
                ItemRepository repo = new ItemRepository(throwingCtx);
                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await repo.AddAsync(item, CancellationToken.None)
                );
            }

            // verify no persistence
            DbContextOptions<AppDbContext> verifyOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            await using (AppDbContext verify = new AppDbContext(verifyOptions))
            {
                Item? fromDb = await verify.Items.FirstOrDefaultAsync(i => i.Id == item.Id);
                Assert.That(fromDb, Is.Null);
            }
        }

        [Test]
        public async Task GetByIdAsync_WhenExists_ReturnsEntity_AsNoTracking()
        {
            Item existing = BuildItem();
            _dbContext.Items.Add(existing);
            await _dbContext.SaveChangesAsync();

            Item? result = await _repository.GetByIdAsync(existing.Id, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Id, Is.EqualTo(existing.Id));
                Assert.That(_dbContext.Entry(result).State, Is.EqualTo(EntityState.Detached));
            });
        }

        [Test]
        public async Task GetByIdAsync_WhenNotExists_ReturnsNull()
        {
            Item? result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateAsync_WhenExists_UpdatesFields()
        {
            Item existing = BuildItem(
                createdAt: new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
            );
            _dbContext.Items.Add(existing);
            await _dbContext.SaveChangesAsync();

            existing.CreatedAt = new DateTimeOffset(2025, 2, 2, 0, 0, 0, TimeSpan.Zero);

            await _repository.UpdateAsync(existing, CancellationToken.None);

            Item? fromDb = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == existing.Id);
            Assert.That(fromDb, Is.Not.Null);
            Assert.That(
                fromDb!.CreatedAt,
                Is.EqualTo(new DateTimeOffset(2025, 2, 2, 0, 0, 0, TimeSpan.Zero))
            );
        }

        [Test]
        public async Task UpdateAsync_WhenSaveFails_RollsBackAndThrows()
        {
            Guid id = Guid.NewGuid();

            // Seed initial value
            DbContextOptions<AppDbContext> seedOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PulseTrack")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            await using (AppDbContext seed = new AppDbContext(seedOptions))
            {
                seed.Items.Add(
                    new Item
                    {
                        Id = id,
                        ProjectId = Guid.NewGuid(),
                        CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    }
                );
                await seed.SaveChangesAsync();
            }

            // Try to update in throwing context
            DbContextOptions<AppDbContext> throwingOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .AddInterceptors(new ThrowingSaveChangesInterceptor())
                    .Options;

            await using (AppDbContext throwingCtx = new AppDbContext(throwingOptions))
            {
                ItemRepository repo = new ItemRepository(throwingCtx);
                Item toUpdate = new Item
                {
                    Id = id,
                    ProjectId = Guid.NewGuid(),
                    CreatedAt = new DateTimeOffset(2025, 3, 3, 0, 0, 0, TimeSpan.Zero),
                };

                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await repo.UpdateAsync(toUpdate, CancellationToken.None)
                );
            }

            // Verify update did not persist
            DbContextOptions<AppDbContext> verifyOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            await using (AppDbContext verify = new AppDbContext(verifyOptions))
            {
                Item? fromDb = await verify.Items.FirstOrDefaultAsync(i => i.Id == id);
                Assert.That(fromDb, Is.Not.Null);
                Assert.That(
                    fromDb!.CreatedAt,
                    Is.EqualTo(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero))
                );
            }
        }

        [Test]
        public async Task DeleteAsync_WhenExists_RemovesEntity()
        {
            Item existing = BuildItem();
            _dbContext.Items.Add(existing);
            await _dbContext.SaveChangesAsync();

            await _repository.DeleteAsync(existing.Id, CancellationToken.None);

            Item? fromDb = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == existing.Id);
            Assert.That(fromDb, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_WhenNotExists_NoOp()
        {
            await _repository.DeleteAsync(Guid.NewGuid(), CancellationToken.None);
            Assert.Pass();
        }

        [Test]
        public async Task DeleteAsync_WhenSaveFails_RollsBackAndThrows()
        {
            Guid id = Guid.NewGuid();

            // Seed initial value
            DbContextOptions<AppDbContext> seedOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PulseTrack")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            await using (AppDbContext seed = new AppDbContext(seedOptions))
            {
                seed.Items.Add(
                    new Item
                    {
                        Id = id,
                        ProjectId = Guid.NewGuid(),
                        CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    }
                );
                await seed.SaveChangesAsync();
            }

            // Try to delete in throwing context
            DbContextOptions<AppDbContext> throwingOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .AddInterceptors(new ThrowingSaveChangesInterceptor())
                    .Options;

            await using (AppDbContext throwingCtx = new AppDbContext(throwingOptions))
            {
                ItemRepository repo = new ItemRepository(throwingCtx);
                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await repo.DeleteAsync(id, CancellationToken.None)
                );
            }

            // Verify entity still exists
            DbContextOptions<AppDbContext> verifyOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            await using (AppDbContext verify = new AppDbContext(verifyOptions))
            {
                Item? fromDb = await verify.Items.FirstOrDefaultAsync(i => i.Id == id);
                Assert.That(fromDb, Is.Not.Null);
            }
        }
    }
}
