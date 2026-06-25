using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Argus.Models;

namespace Argus.Data.Configurations
{
    public class AuditoriumConfiguration : IEntityTypeConfiguration<Auditorium>
    {
        public void Configure(EntityTypeBuilder<Auditorium> builder)
        {
            //Explicity set the name of the table in the database
            //builder.ToTable("Auditorium");

            //Specify the Primary Key
            builder.HasKey(a => a.Id);

            //Hard-tuning the cabinet number column
            builder.Property(a => a.RoomNumber)
                .IsRequired() //Translates to NOT NULL in the database
                .HasMaxLength(50); // Translates to varchar(50) instead of infitite text
            
            //Setting the case number
            builder.Property(a => a.BuildingNumber)
                .IsRequired();
        }
    }
}
