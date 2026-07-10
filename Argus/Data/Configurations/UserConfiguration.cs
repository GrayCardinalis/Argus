using Argus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argus.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.ToTable("User");
            builder.HasKey(u => u.Id);

            builder.Property(u=>u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.Department)
                .HasMaxLength(200);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter($"\"{nameof(User.IsDeleted)}\" = false");

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex (u => u.UserName)
                .IsUnique()
                .HasFilter($"\"{nameof(User.IsDeleted)}\" = false");


            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasConversion<string>() // Store the enum as a string in the database
                .HasMaxLength(50)
                .IsRequired();

            builder.HasQueryFilter(u => !u.IsDeleted); // Global query filter to exclude deleted users

            builder.Property(u => u.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(u => u.DeletedAt)
                .IsRequired(false);

            builder.Property(u => u.DeletedBy)
                .IsRequired(false);
        }
    }
}
