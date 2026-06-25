using Microsoft.EntityFrameworkCore;
using Argus.Models;
using System.Reflection;

namespace Argus.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auditorium> Auditorium { get; set; }
        public DbSet<Component> Component { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<PlacementHistory> PlacementHistory { get; set; }
        public DbSet<SupportRequest> SupportRequest { get; set; }
        public DbSet<SupportRequestComment> SupportRequestComment { get; set; }
        public DbSet<SupportRequestComponent> SupportRequestComponent { get; set; }

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
