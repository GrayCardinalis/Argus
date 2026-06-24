using Microsoft.EntityFrameworkCore;
using Argus.Models;
using System.Reflection;

namespace Argus.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auditorium> Auditoriums { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<PlacementHistory> PlacementHistories { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<SupportRequestComment> SupportRequestComments { get; set; }
        public DbSet<SupportRequestComponent> SupportRequestComponents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var dateTimeOffsetProperties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTimeOffset) || p.ClrType == typeof(DateTimeOffset?));
                foreach (var property in dateTimeOffsetProperties)
                {
                    property.SetColumnType("timestamp with time zone");
                }
            }
        }
    }
}
