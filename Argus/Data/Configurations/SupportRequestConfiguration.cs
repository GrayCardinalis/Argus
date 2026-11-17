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
                .HasConversion<string>() // Store the enum as a string in the database
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

            builder.Property(sr => sr.Title)
                .HasMaxLength(SupportRequestFieldLengths.TitleMax)
                .IsRequired();

            builder.Property(sr => sr.Description)
                .HasMaxLength(SupportRequestFieldLengths.DescriptionMax)
                .IsRequired();
        }
    }
}
