using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment.Service.Configurations
{
    public class PhotoIdConfiguration : IEntityTypeConfiguration<PhotoId>
    {
        public void Configure(EntityTypeBuilder<PhotoId> entity)
        {

            entity.HasKey(e => e.Id);

            entity.Property(e => e.PhotoIdImage)
               .IsRequired();

            entity.Property(e => e.PhotoIdNumber)
               .IsRequired();

            entity.Property(e => e.DateOfIssue)
               .IsRequired()
               .HasColumnType("date");

            entity.HasOne(e => e.Candidate)
                .WithOne(e => e.PhotoId)
                .HasForeignKey<PhotoId>(e => e.CandidateId);
        }
    }
}