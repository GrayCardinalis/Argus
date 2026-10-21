using Argus.Constants.Database;
using Argus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argus.Data.Configurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.InventoryNumber)
                .IsRequired()
                .HasMaxLength(EquipmentFieldLengths.InventoryNumberMax);
            builder.HasIndex(e => e.InventoryNumber)
                .IsUnique();

            builder.Property(e => e.ModelName)
                .IsRequired()
                .HasMaxLength(EquipmentFieldLengths.ModelNameMax);

            builder.Property(e => e.IpAddress)
                .HasMaxLength(EquipmentFieldLengths.IpAddressMax);

            builder.Property(e => e.Type)
                .HasConversion<string>()// Store the enum as a string in the database
                .HasMaxLength(EquipmentFieldLengths.TypeMax)
                .IsRequired();

            builder.Property(e => e.Status)
                .HasConversion<string>()// Store the enum as a string in the database
                .HasMaxLength(EquipmentFieldLengths.StatusMax)
                .IsRequired();
        }
    }
}
