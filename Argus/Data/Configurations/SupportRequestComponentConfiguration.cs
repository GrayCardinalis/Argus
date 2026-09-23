using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Argus.Models;

namespace Argus.Data.Configurations
{
    public class SupportRequestComponentConfiguration : IEntityTypeConfiguration<SupportRequestComponent>
    {
        public void Configure(EntityTypeBuilder<SupportRequestComponent> builder)
        {
            // 1. Table name
            //builder.ToTable("SupportRequestComponents");

            // 2. ERROR FIX: Define the composite primary key
            // EF Core will understand that row uniqueness is determined by the Request + Component pair
            builder.HasKey(src => new { src.SupportRequestId, src.ComponentId });

            // 3. Protect the Quantity field from negative values at the DB level (Check Constraint)
            builder.Property(src => src.Quantity)
                .IsRequired();

            builder.ToTable(t => t.HasCheckConstraint("CK_Quantity_Positive", "quantity > 0"));

            // 4. Explicit relationship configuration (Foreign Keys)
            // Relationship with the request
            builder.HasOne(src => src.SupportRequest)
                .WithMany() // A request can have many used components
                .HasForeignKey(src => src.SupportRequestId)
                .OnDelete(DeleteBehavior.Cascade); // If the request is deleted, the records of its components are deleted too

            // Relationship with the component (warehouse)
            builder.HasOne(src => src.Component)
                .WithMany()
                .HasForeignKey(src => src.ComponentId)
                .OnDelete(DeleteBehavior.Restrict); // FORBID deleting a component from the warehouse if it is linked to repair history
        }
    }
}