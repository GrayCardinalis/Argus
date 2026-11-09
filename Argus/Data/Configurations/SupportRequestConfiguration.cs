using Argus.Constants.Database;
using Argus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Argus.Data.Configurations
{
    public class SupportRequestConfiguration : IEntityTypeConfiguration<SupportRequest>
    {
        public void Configure(EntityTypeBuilder<SupportRequest> builder)
        {
            builder.HasKey(sr => sr.Id);

            builder.Property(sr => sr.Status)
                .IsRequired();

            builder.Property(sr => sr.CreatedAt)
                .IsRequired();

            builder.HasOne(sr => sr.Client)
                .WithMany()
                .HasForeignKey(sr => sr.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sr => sr.Executor)
                .WithMany()
                .HasForeignKey(sr => sr.ExecutorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sr => sr.Equipment)
                .WithMany()
                .HasForeignKey(sr => sr.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sr => sr.Auditorium)
                .WithMany()
                .HasForeignKey(sr => sr.AuditoriumId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
