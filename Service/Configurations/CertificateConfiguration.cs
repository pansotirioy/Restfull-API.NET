using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment.Service.Configurations
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> entity)
        {

            entity.HasKey(e => e.Id); // Primary key

            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.AssessmentTestCode)
                  .IsRequired();

            entity.Property(e => e.MaximumScore);

            entity.Property(e => e.ExaminationDate)
                  .HasColumnType("date");

            entity.Property(e => e.ScoreReportDate)
                  .HasColumnType("date");

            entity.Property(e => e.Price)
                  .IsRequired();

            // Relationships
            entity.HasOne(c => c.Candidates)
                  .WithMany(c => c.Certificates)
                  .HasForeignKey(c => c.CandidateId);

            entity.HasMany(c => c.CandidatesAnalytics)
                  .WithMany(ca => ca.Certificate);
        }
    }
}