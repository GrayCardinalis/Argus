using Argus.Constants.Database;
using Argus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argus.Data.Configurations
{
    public class PlacementHistoryConfiguration : IEntityTypeConfiguration<PlacementHistory>
    {
        public void Configure(EntityTypeBuilder<PlacementHistory> builder)
        {
            builder.HasKey(ph => ph.Id);

            builder.HasIndex(ph => ph.EquipmentId)
                .IsUnique()
                .HasFilter(DatabaseFilters.ActivePlacement);
        }
    }
}
