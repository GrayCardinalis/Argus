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
                .HasMaxLength(150);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(c => c.Quantity)
                .IsRequired();
                //.HasMaxLength(1000);

        }
    }
}
