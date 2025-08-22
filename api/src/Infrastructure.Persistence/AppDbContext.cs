using Microsoft.EntityFrameworkCore;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Section> Sections => Set<Section>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<SubItem> SubItems => Set<SubItem>();
        public DbSet<Label> Labels => Set<Label>();
        public DbSet<ItemLabel> ItemLabels => Set<ItemLabel>();
        public DbSet<DueDate> DueDates => Set<DueDate>();
        public DbSet<Reminder> Reminders => Set<Reminder>();
        public DbSet<ItemHistory> ItemHistories => Set<ItemHistory>();
        public DbSet<View> Views => Set<View>();
        public DbSet<ViewFilter> ViewFilters => Set<ViewFilter>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemLabel>().HasKey(x => new { x.ItemId, x.LabelId });
            modelBuilder.Entity<DueDate>().HasKey(x => x.ItemId);
            modelBuilder.Entity<SearchIndex>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
