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
    public class LabelRepositoryTests
    {
        private AppDbContext _dbContext = null!;
        private LabelRepository _repository = null!;

        [SetUp]
        public void SetUp()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PulseTrack")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _dbContext = new AppDbContext(options);
            _repository = new LabelRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Database.EnsureDeleted();
            _dbContext?.Dispose();
        }

        private static Label BuildLabel(Guid? id = null, string name = "alpha")
        {
            return new Label { Id = id ?? Guid.NewGuid(), Name = name };
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
        public async Task ListAsync_WhenNoLabels_ReturnsEmpty()
        {
            IReadOnlyList<Label> result = await _repository.ListAsync(CancellationToken.None);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task ListAsync_ReturnsAll_OrderedByNameAscending()
        {
            _dbContext.Labels.AddRange(
                BuildLabel(name: "Zeta"),
                BuildLabel(name: "Alpha"),
                BuildLabel(name: "Beta")
            );
            await _dbContext.SaveChangesAsync();

            IReadOnlyList<Label> result = await _repository.ListAsync(CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(result[0].Name, Is.EqualTo("Alpha"));
                Assert.That(result[1].Name, Is.EqualTo("Beta"));
                Assert.That(result[2].Name, Is.EqualTo("Zeta"));
            });
        }

        [Test]
        public async Task ListAsync_UsesAsNoTracking_ReturnsDetachedEntities()
        {
            _dbContext.Labels.AddRange(BuildLabel(name: "One"), BuildLabel(name: "Two"));
            await _dbContext.SaveChangesAsync();

            IReadOnlyList<Label> result = await _repository.ListAsync(CancellationToken.None);

            Assert.That(result, Is.Not.Empty);
            foreach (Label l in result)
            {
                Assert.That(_dbContext.Entry(l).State, Is.EqualTo(EntityState.Detached));
            }
        }

        [Test]
        public async Task AddAsync_InsertsEntity_AndReturnsSameInstance()
        {
            Label label = BuildLabel();

            Label saved = await _repository.AddAsync(label, CancellationToken.None);

            Label? fromDb = await _dbContext.Labels.FirstOrDefaultAsync(l => l.Id == label.Id);
            Assert.Multiple(() =>
            {
                Assert.That(saved, Is.SameAs(label));
                Assert.That(fromDb, Is.Not.Null);
                Assert.That(fromDb!.Id, Is.EqualTo(label.Id));
            });
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
        public async Task AddAsync_WhenSaveFails_RollsBackAndThrows()
        {
            Label label = BuildLabel();

            DbContextOptions<AppDbContext> throwingOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .AddInterceptors(new ThrowingSaveChangesInterceptor())
                    .Options;

            await using (AppDbContext throwingCtx = new AppDbContext(throwingOptions))
            {
                LabelRepository repo = new LabelRepository(throwingCtx);
                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await repo.AddAsync(label, CancellationToken.None)
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
                Label? fromDb = await verify.Labels.FirstOrDefaultAsync(l => l.Id == label.Id);
                Assert.That(fromDb, Is.Null);
            }
        }

        [Test]
        public async Task GetByIdAsync_WhenExists_ReturnsEntity_AsNoTracking()
        {
            Label existing = BuildLabel();
            _dbContext.Labels.Add(existing);
            await _dbContext.SaveChangesAsync();

            Label? result = await _repository.GetByIdAsync(existing.Id, CancellationToken.None);

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
            Label? result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateAsync_WhenExists_UpdatesFields()
        {
            Label existing = BuildLabel(name: "old");
            _dbContext.Labels.Add(existing);
            await _dbContext.SaveChangesAsync();

            existing.Name = "new";

            await _repository.UpdateAsync(existing, CancellationToken.None);

            Label? fromDb = await _dbContext.Labels.FirstOrDefaultAsync(l => l.Id == existing.Id);
            Assert.That(fromDb, Is.Not.Null);
            Assert.That(fromDb!.Name, Is.EqualTo("new"));
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
                seed.Labels.Add(new Label { Id = id, Name = "before" });
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
                LabelRepository repo = new LabelRepository(throwingCtx);
                Label toUpdate = new Label { Id = id, Name = "after" };

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
                Label? fromDb = await verify.Labels.FirstOrDefaultAsync(l => l.Id == id);
                Assert.That(fromDb, Is.Not.Null);
                Assert.That(fromDb!.Name, Is.EqualTo("before"));
            }
        }

        [Test]
        public async Task DeleteAsync_WhenExists_RemovesEntity()
        {
            Label existing = BuildLabel();
            _dbContext.Labels.Add(existing);
            await _dbContext.SaveChangesAsync();

            await _repository.DeleteAsync(existing.Id, CancellationToken.None);

            Label? fromDb = await _dbContext.Labels.FirstOrDefaultAsync(l => l.Id == existing.Id);
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
                seed.Labels.Add(new Label { Id = id, Name = "keep" });
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
                LabelRepository repo = new LabelRepository(throwingCtx);
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
                Label? fromDb = await verify.Labels.FirstOrDefaultAsync(l => l.Id == id);
                Assert.That(fromDb, Is.Not.Null);
            }
        }

        [Test]
        public async Task AssignAsync_WhenNotAssigned_CreatesLink()
        {
            Guid itemId = Guid.NewGuid();
            Guid labelId = Guid.NewGuid();

            _dbContext.Items.Add(BuildItem(id: itemId));
            _dbContext.Labels.Add(BuildLabel(id: labelId, name: "x"));
            await _dbContext.SaveChangesAsync();

            await _repository.AssignAsync(itemId, labelId, CancellationToken.None);

            ItemLabel? link = await _dbContext.ItemLabels.FirstOrDefaultAsync(il =>
                il.ItemId == itemId && il.LabelId == labelId
            );
            Assert.That(link, Is.Not.Null);
        }

        [Test]
        public async Task AssignAsync_WhenAlreadyAssigned_NoOp_NoDuplicate()
        {
            Guid itemId = Guid.NewGuid();
            Guid labelId = Guid.NewGuid();

            _dbContext.Items.Add(BuildItem(id: itemId));
            _dbContext.Labels.Add(BuildLabel(id: labelId, name: "y"));
            _dbContext.ItemLabels.Add(new ItemLabel { ItemId = itemId, LabelId = labelId });
            await _dbContext.SaveChangesAsync();

            await _repository.AssignAsync(itemId, labelId, CancellationToken.None);

            int count = await _dbContext.ItemLabels.CountAsync(il =>
                il.ItemId == itemId && il.LabelId == labelId
            );
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task AssignAsync_WhenSaveFails_RollsBackAndThrows()
        {
            Guid itemId = Guid.NewGuid();
            Guid labelId = Guid.NewGuid();

            // Seed item and label so FK-like expectations are met
            DbContextOptions<AppDbContext> seedOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PulseTrack")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            await using (AppDbContext seed = new AppDbContext(seedOptions))
            {
                seed.Items.Add(BuildItem(id: itemId));
                seed.Labels.Add(BuildLabel(id: labelId, name: "z"));
                await seed.SaveChangesAsync();
            }

            DbContextOptions<AppDbContext> throwingOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .AddInterceptors(new ThrowingSaveChangesInterceptor())
                    .Options;

            await using (AppDbContext throwingCtx = new AppDbContext(throwingOptions))
            {
                LabelRepository repo = new LabelRepository(throwingCtx);
                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await repo.AssignAsync(itemId, labelId, CancellationToken.None)
                );
            }

            // Verify link was not created
            DbContextOptions<AppDbContext> verifyOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            await using (AppDbContext verify = new AppDbContext(verifyOptions))
            {
                ItemLabel? link = await verify.ItemLabels.FirstOrDefaultAsync(il =>
                    il.ItemId == itemId && il.LabelId == labelId
                );
                Assert.That(link, Is.Null);
            }
        }

        [Test]
        public async Task UnassignAsync_WhenAssigned_DeletesLink()
        {
            Guid itemId = Guid.NewGuid();
            Guid labelId = Guid.NewGuid();

            _dbContext.Items.Add(BuildItem(id: itemId));
            _dbContext.Labels.Add(BuildLabel(id: labelId, name: "u"));
            _dbContext.ItemLabels.Add(new ItemLabel { ItemId = itemId, LabelId = labelId });
            await _dbContext.SaveChangesAsync();

            await _repository.UnassignAsync(itemId, labelId, CancellationToken.None);

            ItemLabel? link = await _dbContext.ItemLabels.FirstOrDefaultAsync(il =>
                il.ItemId == itemId && il.LabelId == labelId
            );
            Assert.That(link, Is.Null);
        }

        [Test]
        public async Task UnassignAsync_WhenNotAssigned_NoOp()
        {
            Guid itemId = Guid.NewGuid();
            Guid labelId = Guid.NewGuid();

            _dbContext.Items.Add(BuildItem(id: itemId));
            _dbContext.Labels.Add(BuildLabel(id: labelId, name: "v"));
            await _dbContext.SaveChangesAsync();

            await _repository.UnassignAsync(itemId, labelId, CancellationToken.None);

            Assert.Pass();
        }

        [Test]
        public async Task UnassignAsync_WhenSaveFails_RollsBackAndThrows()
        {
            Guid itemId = Guid.NewGuid();
            Guid labelId = Guid.NewGuid();

            // Seed item, label, and link
            DbContextOptions<AppDbContext> seedOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PulseTrack")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            await using (AppDbContext seed = new AppDbContext(seedOptions))
            {
                seed.Items.Add(BuildItem(id: itemId));
                seed.Labels.Add(BuildLabel(id: labelId, name: "w"));
                seed.ItemLabels.Add(new ItemLabel { ItemId = itemId, LabelId = labelId });
                await seed.SaveChangesAsync();
            }

            // Try to unassign in throwing context
            DbContextOptions<AppDbContext> throwingOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .AddInterceptors(new ThrowingSaveChangesInterceptor())
                    .Options;

            await using (AppDbContext throwingCtx = new AppDbContext(throwingOptions))
            {
                LabelRepository repo = new LabelRepository(throwingCtx);
                Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await repo.UnassignAsync(itemId, labelId, CancellationToken.None)
                );
            }

            // Verify link still exists (rollback occurred)
            DbContextOptions<AppDbContext> verifyOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "PulseTrack")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;
            await using (AppDbContext verify = new AppDbContext(verifyOptions))
            {
                ItemLabel? link = await verify.ItemLabels.FirstOrDefaultAsync(il =>
                    il.ItemId == itemId && il.LabelId == labelId
                );
                Assert.That(link, Is.Not.Null);
            }
        }
    }
}
