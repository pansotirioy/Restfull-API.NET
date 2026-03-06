using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment.Service.Configurations
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> entity)
        {

            // Primary key
            //entity.HasKey(c => c.CandidateNumber);

            // Properties
            entity.Property(c => c.FirstName)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(c => c.MiddleName)
                  .HasMaxLength(20);

            entity.Property(c => c.LastName)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(c => c.Gender)
                  .IsRequired();

            entity.Property(c => c.DateOfBirth)
                  .HasColumnType("date")
                  .IsRequired();

            entity.Property(c => c.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.NativeLanguage)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_DATE");

        }
    }
}
