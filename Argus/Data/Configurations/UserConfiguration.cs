using Argus.Constants.Database;
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
                .HasMaxLength(UserFieldLengths.FullNameMax);

            builder.Property(u => u.Department)
                .HasMaxLength(UserFieldLengths.DepartmentMax);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(UserFieldLengths.EmailMax);
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter(DatabaseFilters.NotDeleted);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(UserFieldLengths.UserNameMax);
            builder.HasIndex (u => u.UserName)
                .IsUnique()
                .HasFilter(DatabaseFilters.NotDeleted);


            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasConversion<string>() // Store the enum as a string in the database
                .HasMaxLength(UserFieldLengths.RoleMax)
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
