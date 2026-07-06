using Microsoft.EntityFrameworkCore;
using Argus.Models;
using System.Reflection;

namespace Argus.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        #region 1. Directories (Independent entities)
        public DbSet<User> Users => Set<User>();
        public DbSet<Auditorium> Auditoriums => Set<Auditorium>();
        public DbSet<Component> Components => Set<Component>();
        public DbSet<Equipment> Equipments => Set<Equipment>();
        #endregion

        #region 2. Operational Tables (Dependent Entities)
        public DbSet<PlacementHistory> PlacementHistories => Set<PlacementHistory>();
        public DbSet<SupportRequest> SupportRequests => Set<SupportRequest>();
        #endregion

        #region 3.Intermediate tables and details (Join Tables)
        public DbSet<SupportRequestComment> SupportRequestComments => Set<SupportRequestComment>();
        public DbSet<SupportRequestComponent> SupportRequestComponent => Set<SupportRequestComponent>();
        #endregion

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
