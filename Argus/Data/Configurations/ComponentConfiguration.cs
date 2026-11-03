using Argus.Constants.Database;
using Argus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argus.Data.Configurations
{
    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            //builder.ToTable("Component");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(ComponentFieldLengths.NameMax);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(c => c.Quantity)
                .IsRequired();
                //.HasMaxLength(1000);

            builder.ToTable(t => t.HasCheckConstraint("CK_Components_Quantity_NonNegative", "quantity >= 0"));

        }
    }
}
